using SourceCode.Hosting.Client.BaseAPI;
using SourceCode.Workflow.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using EVF.K2.Bll.Interfaces;
using EVF.K2.Bll.Models;
using EVF.K2.Bll.Content;
using System.Linq;
using EVF.K2.Bll.Components;

namespace EVF.K2.Bll
{
    public class WorkflowBll : IWorkflow
    {
        #region [Fields]

        /// <summary>
        /// The connection k2 value.
        /// </summary>
        private SourceCode.Workflow.Client.Connection _connection;
        /// <summary>
        /// The connectionmanagement for k2.
        /// </summary>
        private SourceCode.Workflow.Management.WorkflowManagementServer _connectionManagement;
        /// <summary>
        /// The value of json for connection k2.
        /// </summary>
        private K2ConnectModel _model;

        #endregion

        #region [Methods]

        /// <summary>
        /// Initial for k2 connection.
        /// </summary>
        /// <param name="model"></param>
        public void Initial(K2ProfileModel model)
        {
            _model = this.SetValue(model.UserName, model.Password, model.ImpersonateUser);
            if (model.Management)
            {
                _connectionManagement = this.ConnectionManagement(_model);
            }
            else _connection = this.Connection(_model);
        }

        /// <summary>
        /// Start workflow process.
        /// </summary>
        /// <param name="processName">The workflow process name.</param>
        /// <param name="folio">The title workflow.</param>
        /// <param name="dataFields">The data fields workflow.</param>
        /// <returns>process instance id.</returns>
        public int StartWorkflow(string processName, string folio, Dictionary<string, object> dataFields)
        {
            ProcessInstance processInstance = _connection.CreateProcessInstance(processName);
            processInstance.Folio = folio;

            if (dataFields != null)
            {
                foreach (KeyValuePair<string, object> current in dataFields)
                {
                    processInstance.DataFields[current.Key].Value = current.Value;
                }
            }
            _connection.StartProcessInstance(processInstance);

            return processInstance.ID;
        }

        /// <summary>
        /// Action workflow item.
        /// </summary>
        /// <param name="serialNumber">The identity workflow.</param>
        /// <param name="action">The action workflow value.</param>
        /// <param name="datafields">The data fields workflow.</param>
        /// <param name="allocatedUser">The allocated user.</param>
        /// <returns></returns>
        public string ActionWorkflow(string serialNumber, string action, Dictionary<string, object> datafields, string allocatedUser)
        {
            try
            {
                bool isSharingItem = false;
                if ((!string.IsNullOrEmpty(allocatedUser) && !string.IsNullOrEmpty(_model.K2Profile.UserName))
                    && !string.Equals(allocatedUser, _model.K2Profile.UserName, StringComparison.OrdinalIgnoreCase))
                {
                    isSharingItem = true;
                }
                WorklistItem worklistItem;
                if (isSharingItem)
                {
                    worklistItem = _connection.OpenSharedWorklistItem(allocatedUser, _model.K2Profile.UserName, serialNumber);
                }
                else worklistItem = _connection.OpenWorklistItem(serialNumber, "ASP", true);
                if (worklistItem == null)
                {
                    throw new ArgumentNullException(ConstantValueService.MSG_ERR_CANNOT_FOUND_WORKLISTITEM);
                }
                foreach (var datafield in datafields)
                {
                    worklistItem.ProcessInstance.DataFields[datafield.Key].Value = datafield.Value;
                    worklistItem.ProcessInstance.Update();
                }
                worklistItem.Actions[action].Execute();

                //Force to Reject for round robin case.
                if (string.Equals(action, ConstantValueService.K2_REJECT, StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        worklistItem.GotoActivity(ConstantValueService.K2_REJECT);
                    }
                    catch (Exception)
                    {
                        /* Ignore this exception */
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
            return ConstantValueService.MSG_WORKFLOW_ACTION_COMPLETE;
        }

        /// <summary>
        /// Get work list item from k2.
        /// </summary>
        /// <param name="fromUser">The allocated user.</param>
        /// <returns></returns>
        public List<TaskViewModel> GetWorkList(string fromUser, string processFolder, int retry = 0)
        {
            List<TaskViewModel> result = new List<TaskViewModel>();
            try
            {
                Worklist taskList;

                WorklistCriteria worklistCriteria = new WorklistCriteria();
                worklistCriteria.AddFilterField(0, WCCompare.NotEqual, 2);
                worklistCriteria.AddFilterField(WCField.ProcessFolder, WCCompare.Equal, processFolder);

                //For Share Worklist Items
                worklistCriteria.AddFilterField(WCLogical.AndBracket, WCField.WorklistItemOwner,
                                  WCWorklistItemOwner.Me.ToString(), WCCompare.Equal, 0);
                worklistCriteria.AddFilterField(WCLogical.Or, WCField.WorklistItemOwner,
                                  WCWorklistItemOwner.Other.ToString(), WCCompare.Equal, 0);

                taskList = _connection.OpenWorklist(worklistCriteria);
                result = this.ConvertTaskList(taskList);

                if (!string.IsNullOrEmpty(fromUser))
                {
                    var formUserK2Format = ConstantValueService.K2_PREFIX + fromUser;
                    result = result.Where(m => string.Equals(m.AllocatedUser, formUserK2Format, StringComparison.OrdinalIgnoreCase)).ToList();
                }
            }
            catch (Exception ex)
            {
                if (retry >= 2)
                {
                    throw ex;
                }
                System.Threading.Thread.Sleep(50);
                this.ReConnection();
                result = this.GetWorkList(fromUser, processFolder, ++retry);
            }

            return result;
        }

        /// <summary>
        /// Set out of office worklist.
        /// </summary>
        /// <param name="workflowDelegate">The delegate value.</param>
        /// <returns></returns>
        public string SetOutOfOffice(WorkflowDelegateModel workflowDelegate)
        {
            string result = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(workflowDelegate.FromUser))
                {
                    throw new Exception(string.Format(ConstantValueService.NOT_FOUND_TEMPLATE,
                                                      ConstantValueService.DELEGATE_FROM_USER));
                }
                if (string.IsNullOrEmpty(workflowDelegate.ToUser))
                {
                    throw new Exception(string.Format(ConstantValueService.NOT_FOUND_TEMPLATE,
                                                      ConstantValueService.DELEGATE_TO_USER));
                }


                var shareUser = ConstantValueService.K2_PREFIX + workflowDelegate.FromUser;
                var destinationUser = ConstantValueService.K2_PREFIX + workflowDelegate.ToUser;

                // ALL Work that remains which does not form part of any 'WorkTypeException' Filter
                SourceCode.Workflow.Management.WorklistCriteria worklistcriteria = new SourceCode.Workflow.Management.WorklistCriteria
                {
                    Platform = "ASP"
                };
                // Send ALL Work based on the above Filter to the following User
                SourceCode.Workflow.Management.Destinations worktypedestinations = new SourceCode.Workflow.Management.Destinations
                {
                    new SourceCode.Workflow.Management.Destination(destinationUser, SourceCode.Workflow.Management.DestinationType.User)
                };


                // Link the filters and destinations to the Work
                SourceCode.Workflow.Management.WorkType worktype = new SourceCode.Workflow.Management.WorkType(ConstantValueService.K2_SHARING_NAME, worklistcriteria, worktypedestinations);

                SourceCode.Workflow.Management.WorklistShare worklistshare = new SourceCode.Workflow.Management.WorklistShare
                {
                    ShareType = SourceCode.Workflow.Management.ShareType.OOF,
                    // Sharing dates as per the sample indicates that work will always be shared as long as the correct status is set
                    // These dates may also be used to only share work for a specific period
                    StartDate = workflowDelegate.StartDate.Value,
                    EndDate = workflowDelegate.EndDate.Value
                };
                worklistshare.WorkTypes.Add(worktype);

                var k2Result = true;
                switch (workflowDelegate.Action)
                {
                    case ConstantValueService.K2_SHARING_CREATE:
                        // K2Server will create the user's Worklist Sharing, but no sharing will take place unless the Status of the Share is updated
                        k2Result = _connectionManagement.ShareWorkList(shareUser, worklistshare);

                        if (!k2Result) throw new InvalidOperationException(ConstantValueService.MSG_ERR_CANNOT_SAVE_SHARING_WORKLIST);

                        // Once this status is updated, each time a user, which the sharing user is sharing any work with opens their Worklist, K2Server will combine the Worklist(s) based on the opening user(s) Worklist Filters
                        k2Result = _connectionManagement.SetUserStatus(shareUser, SourceCode.Workflow.Management.UserStatuses.OOF);

                        if (!k2Result) throw new InvalidOperationException(ConstantValueService.MSG_ERR_CANNOT_SET_STATUS);
                        break;
                    case ConstantValueService.K2_SHARING_EDIT:
                        //Delete All Old Workflow
                        k2Result = _connectionManagement.UnShareAll(shareUser);

                        if (!k2Result) throw new InvalidOperationException(ConstantValueService.MSG_ERR_CANNOT_DISABLE_SHARING);

                        //Update New WorkType
                        k2Result = _connectionManagement.ShareWorkList(shareUser, worklistshare);

                        if (!k2Result) throw new InvalidOperationException(ConstantValueService.MSG_ERR_CANNOT_SAVE_SHARING_WORKLIST);

                        //Update OOF
                        k2Result = _connectionManagement.SetUserStatus(shareUser, SourceCode.Workflow.Management.UserStatuses.OOF);

                        if (!k2Result) throw new InvalidOperationException(ConstantValueService.MSG_ERR_CANNOT_SET_STATUS);
                        break;
                    case ConstantValueService.K2_SHARING_DELETE:
                        //Delete All Old Workflow
                        k2Result = _connectionManagement.UnShareAll(shareUser);

                        if (!k2Result) throw new InvalidOperationException(ConstantValueService.MSG_ERR_CANNOT_DISABLE_SHARING);

                        //Set User Sharing To Available
                        k2Result = _connectionManagement.SetUserStatus(shareUser, SourceCode.Workflow.Management.UserStatuses.Available);

                        if (!k2Result) throw new InvalidOperationException(ConstantValueService.MSG_ERR_CANNOT_SET_STATUS);
                        break;
                    default:
                        throw new InvalidOperationException(ConstantValueService.MSG_ERR_INVALID_OPERATION_ACTION);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// Set value for open connection k2.
        /// </summary>
        /// <returns></returns>
        private K2ConnectModel SetValue(string userName, string password, string impersonateUser)
        {
            K2ConnectModel result = new K2ConnectModel
            {
                K2Profile = new K2ProfileModel
                {
                    UserName = userName,
                    Password = UtilityService.DecryptString(password, ConfigSetting.EncryptionKey)
                },
                Port = Convert.ToInt32(ConfigSetting.K2WorkflowPort),
                Url = ConfigSetting.K2Url,
                SecurityLabelName = ConfigSetting.K2SecurityLabel
            };
            if (!string.IsNullOrEmpty(impersonateUser))
            {
                result.K2Profile.Impersonate = true;
                result.K2Profile.ImpersonateUser = impersonateUser;
            }
            return result;
        }

        /// <summary>
        /// Open connection k2.
        /// </summary>
        /// <param name="model">The value for connect k2.</param>
        /// <param name="retry">The round of try to connect k2.</param>
        /// <returns></returns>
        private SourceCode.Workflow.Client.Connection Connection(K2ConnectModel model, int retry = 0)
        {
            var connection = new SourceCode.Workflow.Client.Connection();
            int retryNum = retry;
            try
            {
                string connectionString = this.GetConnectionString(model);
                connection.Open(model.Url, connectionString);
                if (model.K2Profile.Impersonate)
                {
                    connection.ImpersonateUser(model.SecurityLabelName + ":" + model.K2Profile.ImpersonateUser);
                }

            }
            catch (System.Exception ex)
            {
                connection = null;
                retryNum++;

                if (retryNum == 2)
                {
                    throw new System.Exception("Unable to create connection (retry:" + retryNum.ToString() + ") : " + ex.ToString());
                }
                else
                {
                    System.Threading.Thread.Sleep(50);
                    return Connection(model, retryNum);
                }
            }
            return connection;
        }

        /// <summary>
        /// Reconnection k2.
        /// </summary>
        private void ReConnection()
        {
            if (_connection != null)
            {
                _connection.Close();
            }
            _connection = Connection(_model);
        }

        /// <summary>
        /// Convert work list item to task list viewmodel.
        /// </summary>
        /// <param name="worklist">The worklist item.</param>
        /// <returns></returns>
        private List<TaskViewModel> ConvertTaskList(Worklist worklist)
        {
            List<TaskViewModel> taskList = new List<TaskViewModel>();
            foreach (WorklistItem item in worklist)
            {
                taskList.Add(this.ConvertTask(item));
            }
            return taskList;
        }

        /// <summary>
        /// Convert work item to task view model.
        /// </summary>
        /// <param name="item">The work item.</param>
        /// <returns></returns>
        private TaskViewModel ConvertTask(WorklistItem item)
        {
            TaskViewModel task = new TaskViewModel
            {
                AllocatedUser = item.AllocatedUser,
                StartDate = item.ProcessInstance.StartDate,
                Folder = item.ProcessInstance.Folder,
                Name = item.ProcessInstance.Name,
                FullName = item.ProcessInstance.FullName,
                Folio = item.ProcessInstance.Folio
            };
            foreach (DataField dataField in item.ProcessInstance.DataFields)
            {
                task.DataFields.Add(dataField.Name, dataField.Value);
            }
            return task;
        }

        /// <summary>
        /// Open connection management k2.
        /// </summary>
        /// <param name="model">The value for connect k2.</param>
        /// <returns></returns>
        private SourceCode.Workflow.Management.WorkflowManagementServer ConnectionManagement(K2ConnectModel model)
        {
            var managementServer = new SourceCode.Workflow.Management.WorkflowManagementServer();
            try
            {
                model.K2Profile.UserName = ConfigSetting.K2AdminUserName;
                model.K2Profile.Password = ConfigSetting.K2AdminPassword;
                model.Port = Convert.ToInt32(ConfigSetting.K2ManagementPort);
                string connectionString = this.GetConnectionString(model);
                managementServer.Open(connectionString);

            }
            catch (Exception ex)
            {
                throw new Exception("Unable to create management connection : " + ex.ToString());
            }
            return managementServer;
        }

        /// <summary>
        /// Get k2 url connection string.
        /// </summary>
        /// <param name="model">The value for connect k2.</param>
        /// <returns></returns>
        private string GetConnectionString(K2ConnectModel model)
        {
            return string.Format(ConfigSetting.K2ConnectionString,
                                   model.Url, model.Port, model.SecurityLabelName, model.K2Profile.UserName, model.K2Profile.Password);
        }

        #endregion
    }
}
