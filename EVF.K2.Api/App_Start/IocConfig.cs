using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using EVF.Helper;
using EVF.Helper.Interfaces;
using EVF.Helper.Windsor;
using EVF.K2.Bll;
using EVF.K2.Bll.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace EVF.K2.Api.App_Start
{
    /// <summary>
    /// The WindsorConfig class provide windsor functionalities such as container.
    /// </summary>
    public static class WindsorConfig
    {
        /// <summary>
        /// The container.
        /// </summary>
        private readonly static IWindsorContainer _container = new WindsorContainer();

        /// <summary>
        /// Gets or sets the container contains all components.
        /// </summary>
        /// <value>The container.</value>
        public static IWindsorContainer CONTAINER
        {
            get
            {
                // Checks components to avoid duplicate registration component.
                if (!_container.Kernel.GraphNodes.Any())
                {
                    // Install all components needed.
                    _container.Install(new ComponentInstaller());
                }

                return _container;
            }
        }

        /// <summary>
        /// Registers DI resolver to web api.
        /// </summary>
        /// <param name="config">The global configuration.</param>
        public static void Register(HttpConfiguration config)
        {
            config.DependencyResolver = new WindsorDependencyResolver(CONTAINER.Kernel);
        }

        /// <summary>
        /// Class ComponentInstaller for installing all components needed to windsor container.
        /// </summary>
        private class ComponentInstaller : IWindsorInstaller
        {
            /// <summary>
            /// Performs the installation in the windsor container.
            /// </summary>
            /// <param name="container">The container.</param>
            /// <param name="store">The configuration store.</param>
            public void Install(IWindsorContainer container, IConfigurationStore store)
            {
                container.Register(
                    // Registers all api controllers.
                    Classes.FromThisAssembly().BasedOn<ApiController>().LifestyleScoped()

                    // Registers loggers.
                    , Component.For<NLog.ILogger>()
                               .UsingFactoryMethod(kernel =>
                               {
                                   var logger = NLog.LogManager.GetLogger("webApiLogger");
                                   if (Convert.ToBoolean(ConfigurationManager.AppSettings["DebugMode"]))
                                   {
                                       // Sets debug log level.
                                       logger.Factory.Configuration.LoggingRules
                                             .First(rule => rule.LoggerNamePattern == logger.Name &&
                                                            !rule.IsLoggingEnabledForLevel(NLog.LogLevel.Error))
                                             .EnableLoggingForLevel(NLog.LogLevel.Debug);
                                       // Reloads modified config.
                                       logger.Factory.Configuration.Reload();
                                   }

                                   return logger;
                               })
                               .Named("nlogWebApiLogger")
                               .LifestyleSingleton()
                    , Component.For<ILogger>() // Uses NLog config for web api.
                               .ImplementedBy<Logger>()
                               .Named("webApiLogger")
                               .DependsOn(Dependency.OnComponent(typeof(NLog.ILogger), "nlogWebApiLogger"))
                               .LifestyleSingleton()
                    , Component.For(typeof(ErrorHandlerFilterAttribute))
                               .DependsOn(Dependency.OnComponent(typeof(ILogger), "webApiLogger")) // Uses NLog config for web api.
                               .LifestyleTransient()
                    , Component.For<IApiErrorHandler>()
                               .ImplementedBy<ApiErrorHandler>()
                               .LifestyleSingleton()
                    , Component.For(typeof(LogFilterAttribute))
                               .DependsOn(Dependency.OnComponent(typeof(ILogger), "webApiLogger")) // Uses NLog config for web api.
                               .LifestyleTransient()
                    , Component.For(typeof(MvcLogFilterAttribute))
                               .DependsOn(Dependency.OnComponent(typeof(ILogger), "webApiLogger")) // Uses NLog config for web api.
                               .LifestyleTransient()
                    , Component.For(typeof(MvcLogErrorFilterAttribute))
                               .DependsOn(Dependency.OnComponent(typeof(ILogger), "webApiLogger")) // Uses NLog config for web api.
                               .LifestyleTransient()
                    , Component.For<IWorkflow>()
                               .ImplementedBy<WorkflowBll>()
                               .LifestyleScoped()
                    , Component.For<ISmartObject>()
                               .ImplementedBy<SmartObjectBll>()
                               .LifestyleScoped()
                );
            }
        }
    }
}