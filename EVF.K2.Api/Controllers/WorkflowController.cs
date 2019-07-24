using EVF.K2.Api.Models;
using EVF.K2.Bll.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace EVF.K2.Api.Controllers
{
    public class WorkflowController : ApiController
    {

        #region [Fields]

        /// <summary>
        /// The workflow manager provides workflow functionality.
        /// </summary>
        private readonly IWorkflow _workflow;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="WorkflowController" /> class.
        /// </summary>
        /// <param name="workflow"></param>
        public WorkflowController(IWorkflow workflow)
        {
            _workflow = workflow;
        }

        #endregion

        #region [Methods]

        [HttpPost]
        [Route("StartWorkflow")]
        public IHttpActionResult StartWorkflow([FromBody]StartWorkflowViewModel model)
        {
            _workflow.Initial(model.K2Connect);
            return Ok(_workflow.StartWorkflow(model.ProcessName, model.Folio, model.DataFields));
        }

        [HttpPost]
        [Route("ActionWorkflow")]
        public IHttpActionResult ActionWorkflow([FromBody]ActionWorkflowViewModel model)
        {
            _workflow.Initial(model.K2Connect);
            return Ok(_workflow.ActionWorkflow(model.SerialNumber, model.Action, model.Datafields, model.AllocatedUser));
        }

        [HttpPost]
        [Route("GetWorkList")]
        public IHttpActionResult GetWorkList([FromBody]WorklistViewModel model)
        {
            _workflow.Initial(model.K2Connect);
            return Ok(_workflow.GetWorkList(model.FromUser, model.ProcessFolder));
        }

        [HttpPost]
        [Route("SetOutOfOffice")]
        public IHttpActionResult SetOutOfOffice([FromBody]SetOutOfOfficeViewModel model)
        {
            _workflow.Initial(model.K2Connect);
            return Ok(_workflow.SetOutOfOffice(model.WorkflowDelegate));
        }

        #endregion

    }
}