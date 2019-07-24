using EVF.Helper.Interfaces;
using System;
using System.Web.Http.Filters;

namespace EVF.Helper
{
    public class ErrorHandlerFilterAttribute : ExceptionFilterAttribute
    {

        #region [Fields]

        /// <summary>
        /// The api error handler.
        /// </summary>
        private readonly IApiErrorHandler _apiErrorHandler;
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger _logger;

        #endregion


        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandlerFilterAttribute" /> class.
        /// </summary>
        /// <param name="apiErrorHandler">The api error handler.</param>
        /// <param name="logger">The logger.</param>
        public ErrorHandlerFilterAttribute(IApiErrorHandler apiErrorHandler, ILogger logger)
        {
            _apiErrorHandler = apiErrorHandler;
            _logger = logger;
        }

        #endregion


        #region [Methods]

        /// <summary>
        /// Raises the exception event.
        /// </summary>
        /// <param name="actionExecutedContext">The context for the action.</param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            // LOG: Logs error.
            _logger.Error(actionExecutedContext.Exception,
                          String.Format("{0}/{1}",
                                        actionExecutedContext.ActionContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                                        actionExecutedContext.ActionContext.ActionDescriptor.ActionName));

            // Sets http response message by an error model.
            actionExecutedContext.Response = _apiErrorHandler.GetHttpResponseMessage(actionExecutedContext.Exception);
            actionExecutedContext.Response.RequestMessage = actionExecutedContext.Request;
        }

        #endregion

    }
}
