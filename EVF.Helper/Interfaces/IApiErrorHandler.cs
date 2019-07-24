using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace EVF.Helper.Interfaces
{
    /// <summary>
    /// Interface IApiErrorHandler handles all error in api.
    /// </summary>
    public interface IApiErrorHandler
    {
        /// <summary>
        /// Throws the HTTP response message by an API error model.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns>A HTTP response exception.</returns>
        HttpResponseException ThrowErrorModel(Exception ex);

        /// <summary>
        /// Gets the HTTP response message with an API error model.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns>A HTTP response message.</returns>
        HttpResponseMessage GetHttpResponseMessage(Exception ex);

        /// <summary>
        /// Gets the HTTP status code from exception by using mapper.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns>A HTTP status code.</returns>
        HttpStatusCode GetHttpStatusCode(Exception ex);
    }
}
