using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace EVF.Helper
{
    public class ApiGlobalPrefixRouteProvider : DefaultDirectRouteProvider
    {

        #region [Fields]

        /// <summary>
        /// The global prefix route.
        /// </summary>
        private readonly string _globalPrefix;

        #endregion


        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiGlobalPrefixRouteProvider" /> class.
        /// </summary>
        /// <param name="globalPrefix">The global prefix. It can be null if you dont want.</param>
        public ApiGlobalPrefixRouteProvider(string globalPrefix = null)
        {
            _globalPrefix = globalPrefix;
        }

        #endregion


        #region [Methods]

        /// <summary>
        /// Gets the route prefix from the provided controller.
        /// </summary>
        /// <param name="controllerDescriptor">The controller descriptor.</param>
        /// <returns>The route prefix or null.</returns>
        protected override string GetRoutePrefix(HttpControllerDescriptor controllerDescriptor)
        {
            // Checks route prefix of each controller if any.
            var existingPrefix = base.GetRoutePrefix(controllerDescriptor);

            // Plus route prefix of each controller next to global route.
            if (string.IsNullOrEmpty(_globalPrefix))
            {
                return string.Format("{0}", existingPrefix ?? controllerDescriptor.ControllerName);
            }

            return string.Format("{0}/{1}", _globalPrefix, existingPrefix ?? controllerDescriptor.ControllerName);
        }

        #endregion

    }
}
