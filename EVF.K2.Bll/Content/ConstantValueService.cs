using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVF.K2.Bll.Content
{
    public class ConstantValueService
    {
        /// <summary>
        /// The key validate case user type connect to k2.
        /// </summary>
        public const string USERTYPE_EMPLOYEE = "EMPLOYEE";

        /// <summary>
        /// The key of activity k2 approve.
        /// </summary>
        public const string K2_APPROVE = "Approve";
        /// <summary>
        /// The key of activity k2 reject.
        /// </summary>
        public const string K2_REJECT = "Reject";
        /// <summary>
        /// The key of activity k2 cancel.
        /// </summary>
        public const string K2_CANCEL = "Cancel";

        /// <summary>
        /// The key of create out of office in k2. 
        /// </summary>
        public const string K2_SHARING_CREATE = "CREATE";
        /// <summary>
        /// The key of edit out of office in k2.
        /// </summary>
        public const string K2_SHARING_EDIT = "EDIT";
        /// <summary>
        /// The key of delete out of office in k2.
        /// </summary>
        public const string K2_SHARING_DELETE = "DELETE";

        /// <summary>
        /// The key name of webconfig k2 url.
        /// </summary>
        public const string K2_URL = "K2Url";
        /// <summary>
        /// The key name of webconfig k2 port.
        /// </summary>
        public const string K2_PORT = "K2WorkflowPort";
        /// <summary>
        /// The key name of webconfig k2 management port.
        /// </summary>
        public const string K2_MANAGEMENT_PORT = "K2ManagementPort";
        /// <summary>
        /// The key name of webconifg k2 security label.
        /// </summary>
        public const string K2_SECURITYLABEL = "K2SecurityLabel";
        /// <summary>
        /// The key name of webconifg k2 admin username.
        /// </summary>
        public const string K2_ADMINUSERNAME = "K2Admin";
        /// <summary>
        /// The key name of webconifg k2 admin password.
        /// </summary>
        public const string K2_ADMINPASSWORD = "K2AdminPassword";
        /// <summary>
        /// The key name of webconfig k2 work flow employee.
        /// </summary>
        public const string K2_WORKFLOWEMPLOYEE = "K2WorkflowEmployee";
        /// <summary>
        /// The key name of webconfig k2 work flow iis.
        /// </summary>
        public const string K2_WORKFLOWIIS = "K2WorkflowIIS";
        /// <summary>
        /// The key name of webconfig k2 task url.
        /// </summary>
        public const string K2_TASKURL = "K2TaskUrl";
        /// <summary>
        /// The K2 prefix parameter.
        /// </summary>
        public const string K2_PREFIX = "K2:";
        /// <summary>
        /// The key name of webconfig k2 process folder.
        /// </summary>
        public const string K2_PROCESSFODLER = "K2ProcessFolder";
        /// <summary>
        /// The value of sharing worklist.
        /// </summary>
        public const string K2_SHARING_NAME = "K2AsyncShareTask";

        /// <summary>
        /// The workflow complete message.
        /// </summary>
        public const string MSG_WORKFLOW_ACTION_COMPLETE = "Workflow Action Complete.";

        /// <summary>
        /// The Error message worklist item is null.
        /// </summary>
        public const string MSG_ERR_CANNOT_FOUND_WORKLISTITEM = "Not found work list item.";

        /// <summary>
        /// The Delegate from user field.
        /// </summary>
        public const string DELEGATE_FROM_USER = "DelegateFromUser";

        /// <summary>
        /// The Delegate to user field.
        /// </summary>
        public const string DELEGATE_TO_USER = "DelegateToUser";

        /// <summary>
        /// The Template not found format.
        /// </summary>
        public const string NOT_FOUND_TEMPLATE = "The {0} is empty.";

        /// <summary>
        /// The Error message invalid set out of office operation.
        /// </summary>
        public const string MSG_ERR_INVALID_OPERATION_ACTION = "The action is invalid operation.";
        /// <summary>
        /// The Error message can't save status out of office. 
        /// </summary>
        public const string MSG_ERR_CANNOT_SET_STATUS = "Can't save user status.";
        /// <summary>
        /// The Error message can't disable worklist sharing.
        /// </summary>
        public const string MSG_ERR_CANNOT_DISABLE_SHARING = "Can't disable sharing worklist.";
        /// <summary>
        /// The Error message can't save sharing worklist.
        /// </summary>
        public const string MSG_ERR_CANNOT_SAVE_SHARING_WORKLIST = "Can't save sharing worklist.";

        /// <summary>
        /// The key name of webconfig encrypt key. 
        /// </summary>
        public const string ENCRYPTION_KEY = "EncryptionKey";

    }
}
