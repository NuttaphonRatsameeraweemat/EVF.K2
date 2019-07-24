using Castle.MicroKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;

namespace EVF.Helper.Windsor
{
    /// <summary>
    /// WindsorDependencyResolver class provides resolving dependencies by using Windsor.
    /// </summary>
    public sealed class WindsorDependencyResolver : System.Web.Http.Dependencies.IDependencyResolver
    {

        #region [Fields]

        /// <summary>
        /// The windsor container.
        /// </summary>
        private readonly IKernel _container;

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="WindsorDependencyResolver" /> class.
        /// </summary>
        /// <param name="container">The windsor container.</param>
        public WindsorDependencyResolver(IKernel container)
        {
            _container = container;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Starts scope for resolving dependency.
        /// </summary>
        /// <returns></returns>
        public IDependencyScope BeginScope()
        {
            return new WindsorDependencyScope(_container);
        }

        /// <summary>
        /// Gets a specified dependency by type.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <returns>A specified dependency as boxing object.</returns>
        public object GetService(Type serviceType)
        {
            return _container.HasComponent(serviceType) ? _container.Resolve(serviceType) : null;
        }

        /// <summary>
        /// Gets all dependencies which belong to specfied type.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <returns>A collection of specified dependencies as boxing object.</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.ResolveAll(serviceType).Cast<object>();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Do not need to dispose any resource.
        }

        #endregion

    }
}
