using EVF.Helper.Interfaces;
using EVF.Helper;
using System;
using System.Collections.Generic;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace EVF.K2.Api.App_Start
{
    /// <summary>
    /// Class ApiFilterConfig is class for register filter.
    /// </summary>
    public static class ApiFilterConfig
    {
        /// <summary>
        /// Registers the specified filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        public static void Register(HttpFilterCollection filters)
        {
            // Error handler.
            filters.Add((ErrorHandlerFilterAttribute)WindsorConfig.CONTAINER.Resolve(typeof(ErrorHandlerFilterAttribute)));

            // Log handler.
            filters.Add((LogFilterAttribute)WindsorConfig.CONTAINER.Resolve(typeof(LogFilterAttribute)));
        }
    }

    /// <summary>
    /// Class MvcFilterConfig is class for register MVC filter.
    /// </summary>
    public static class MvcFilterConfig
    {
        /// <summary>
        /// Registers the specified filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        public static void Register(GlobalFilterCollection filters)
        {
            // Log handlers.
            filters.Add((MvcLogFilterAttribute)WindsorConfig.CONTAINER.Resolve(typeof(MvcLogFilterAttribute)));
            filters.Add((MvcLogErrorFilterAttribute)WindsorConfig.CONTAINER.Resolve(typeof(MvcLogErrorFilterAttribute)));
        }
    }

    /// <summary>
    /// Class MvcLogFilterAttribute provides logging for all MVC controllers.
    /// </summary>
    public class MvcLogFilterAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        #region [Fields]

        /// <summary>
        /// The logger.
        /// </summary>
        protected readonly ILogger _logger;

        #endregion


        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="MvcLogFilterAttribute" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public MvcLogFilterAttribute(ILogger logger)
        {
            _logger = logger;
        }

        #endregion


        #region [Methods]

        /// <summary>
        /// Logs on start invoking method.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // LOG: Logs before running action.
            _logger.Info(String.Format("{0}.{1}|Start call action method.",
                                       filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                                       filterContext.ActionDescriptor.ActionName));
        }

        /// <summary>
        /// Logs on end invoking method.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // LOG: Logs before running action.
            _logger.Info(String.Format("{0}.{1} status code {2}|End call action method.",
                                       filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                                       filterContext.ActionDescriptor.ActionName,
                                       filterContext.HttpContext.Response.StatusCode));
        }

        #endregion
    }

    /// <summary>
    /// Class MvcLogErrorFilterAttribute provides logging for all MVC controllers.
    /// </summary>
    public class MvcLogErrorFilterAttribute : HandleErrorAttribute
    {
        #region [Fields]

        /// <summary>
        /// The api error handler.
        /// </summary>
        private readonly IApiErrorHandler _apiErrorHandler;
        /// <summary>
        /// The logger.
        /// </summary>
        protected readonly ILogger _logger;

        #endregion


        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="MvcLogErrorFilterAttribute" /> class.
        /// </summary>
        /// <param name="apiErrorHandler">The api error handler.</param>
        /// <param name="logger">The logger.</param>
        public MvcLogErrorFilterAttribute(IApiErrorHandler apiErrorHandler, ILogger logger)
        {
            _apiErrorHandler = apiErrorHandler;
            _logger = logger;
        }

        #endregion


        #region [Methods]

        /// <summary>
        /// Raises the exception event.
        /// </summary>
        /// <param name="filterContext">The exception filter context.</param>
        public override void OnException(ExceptionContext filterContext)
        {
            // LOG:
            _logger.Error(
                filterContext.Exception,
                $"{filterContext.RouteData.Values["controller"]}.{filterContext.RouteData.Values["action"]}|");

            filterContext.ExceptionHandled = true;
            filterContext.Result = new RedirectToRouteResult("ExceptionHandler", new System.Web.Routing.RouteValueDictionary(new
            {
                statuscode = (int)_apiErrorHandler.GetHttpStatusCode(filterContext.Exception),
                message = filterContext.Exception.Message
            }));
        }

        #endregion
    }
}