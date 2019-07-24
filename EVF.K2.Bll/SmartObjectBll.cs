using EVF.K2.Bll.Content;
using EVF.K2.Bll.Interfaces;
using EVF.K2.Bll.Models;
using SourceCode.Hosting.Client.BaseAPI;
using SourceCode.SmartObjects.Client;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace EVF.K2.Bll
{
    public class SmartObjectBll : ISmartObject
    {

        #region [Methods]

        /// <summary>
        /// Execute SmartObject by Name and Method Name.
        /// </summary>
        /// <param name="model">The information name and method name for execute smartobject.</param>
        /// <returns></returns>
        public List<Dictionary<string, object>> GetSmartObject(SmartObjectModel model)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            var soServer = this.GetServer();
            using (soServer.Connection)
            {
                var workflowSmartObject = WorkflowSmartObject(soServer, model);
                var smartObject = soServer.ExecuteList(workflowSmartObject);
                result = this.ConvertToModel(smartObject.SmartObjectsList);
            }
            return result;
        }

        /// <summary>
        /// Map all properties in smartobject to list dictionary.
        /// </summary>
        /// <param name="smItem">The SmartObject Collection.</param>
        /// <returns></returns>
        private List<Dictionary<string, object>> ConvertToModel(SmartObjectCollection smItem)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            foreach (SmartObject item in smItem)
            {
                Dictionary<string, object> temp = new Dictionary<string, object>();
                foreach (SmartProperty propertie in item.Properties)
                {
                    temp.Add(propertie.Name, propertie.Value);
                }
                result.Add(temp);
            }
            return result;
        }

        /// <summary>
        /// Set Smartobject Name and Method Name.
        /// </summary>
        /// <param name="soServer">The SmartObjectClientServer.</param>
        /// <param name="model">The Information smart object.</param>
        /// <returns></returns>
        private SmartObject WorkflowSmartObject(SmartObjectClientServer soServer, SmartObjectModel model)
        {
            var result = soServer.GetSmartObject(model.SmartObjectName);
            result.MethodToExecute = model.ExecuteMethodName;
            return result;
        }

        /// <summary>
        /// Get Connection String Connect to K2.
        /// </summary>
        /// <returns>The connection string.</returns>
        private string GetConnectionString()
        {
            var hostServerConnectionString = new SCConnectionStringBuilder
            {
                Host = ConfigurationManager.AppSettings[ConstantValueService.K2_URL],
                Port = Convert.ToUInt32(ConfigurationManager.AppSettings[ConstantValueService.K2_MANAGEMENT_PORT]),
                IsPrimaryLogin = true,
                Integrated = false,
                UserID = ConfigurationManager.AppSettings[ConstantValueService.K2_ADMINUSERNAME],
                Password = ConfigurationManager.AppSettings[ConstantValueService.K2_ADMINPASSWORD],
                SecurityLabelName = ConfigurationManager.AppSettings[ConstantValueService.K2_SECURITYLABEL]
            };
            return hostServerConnectionString.ToString();
        }

        /// <summary>
        /// Create Connection to K2 Server.
        /// </summary>
        /// <returns></returns>
        private SmartObjectClientServer GetServer()
        {
            var soServer = new SmartObjectClientServer();
            soServer.CreateConnection();
            soServer.Connection.Open(this.GetConnectionString());
            return soServer;
        }

        #endregion

    }
}
