using EVF.K2.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EVF.K2.Bll.Interfaces
{
    public interface IWorkflow
    {

        /// <summary>
        /// Initial for k2 connection.
        /// </summary>
        /// <param name="model"></param>
        void Initial(K2ProfileModel model);

        /// <summary>
        /// Start workflow process.
        /// </summary>
        /// <param name="processName">The workflow process name.</param>
        /// <param name="folio">The title workflow.</param>
        /// <param name="dataFields">The data fields workflow.</param>
        /// <returns>process instance id.</returns>
        int StartWorkflow(string processName, string folio, System.Collections.Generic.Dictionary<string, object> dataFields);

        /// <summary>
        /// Action workflow item.
        /// </summary>
        /// <param name="serialNumber">The identity workflow.</param>
        /// <param name="action">The action workflow value.</param>
        /// <param name="datafields">The data fields workflow.</param>
        /// <param name="allocatedUser">The allocated user.</param>
        /// <returns></returns>
        string ActionWorkflow(string serialNumber, string action, Dictionary<string, object> datafields, string allocatedUser);

        /// <summary>
        /// Get work list item from k2.
        /// </summary>
        /// <param name="fromUser">The allocated user.</param>
        /// <returns></returns>
        List<TaskViewModel> GetWorkList(string fromUser, string processFolder, int retry = 0);

        /// <summary>
        /// Set out of office worklist.
        /// </summary>
        /// <param name="workflowDelegate">The delegate value.</param>
        /// <returns></returns>
        string SetOutOfOffice(WorkflowDelegateModel workflowDelegate);

    }
}
