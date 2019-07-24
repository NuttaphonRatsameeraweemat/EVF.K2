using EVF.Helper.Interfaces;
using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace EVF.Helper
{
    public class LogFilterAttribute : ActionFilterAttribute
    {

        #region [Fields]

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger _logger;

        #endregion


        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="LogFilterAttribute" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public LogFilterAttribute(ILogger logger)
        {
            _logger = logger;
        }

        #endregion


        #region [Methods]

        /// <summary>
        /// Occurs before the action method is invoked.
        /// </summary>
        /// <param name="actionContext">The action context.</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // Logs before running action.
            _logger.Info(String.Format("{0} Start call action method.",
                                       String.Format("{0}/{1}",
                                                     actionContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                                                     actionContext.ActionDescriptor.ActionName)));
        }

        /// <summary>
        /// Occurs after the action method is invoked.
        /// </summary>
        /// <param name="actionExecutedContext">The action executed context.</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            // Logs after finished action.
            _logger.Info(String.Format("{0} End call action method.",
                                       String.Format("{0}/{1}",
                                                     actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                                                     actionExecutedContext.ActionContext.ActionDescriptor.ActionName)));
        }

        #endregion

    }
}
