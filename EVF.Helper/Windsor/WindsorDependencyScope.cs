using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;

namespace EVF.Helper.Windsor
{
    public sealed class WindsorDependencyScope : IDependencyScope
    {

        #region [Fields]

        /// <summary>
        /// The windsor container.
        /// </summary>
        private readonly IKernel _container;
        /// <summary>
        /// The resolving scope.
        /// </summary>
        private readonly IDisposable _scope;

        #endregion


        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="WindsorDependencyScope" /> class.
        /// </summary>
        /// <param name="container">The windsor container.</param>
        public WindsorDependencyScope(IKernel container)
        {
            _container = container;
            _scope = container.BeginScope();
        }

        #endregion


        #region [Methods]

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
            if (_scope != null)
            {
                _scope.Dispose();
            }
        }

        #endregion

    }
}
