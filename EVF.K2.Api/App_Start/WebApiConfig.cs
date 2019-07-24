using EVF.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace EVF.K2.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Implements dynamic url routing for any controller.
            config.MapHttpAttributeRoutes(new ApiGlobalPrefixRouteProvider("api"));
        }
    }
}
