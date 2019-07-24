using EVF.Helper.Interfaces;
using EVF.Helper.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Authentication;
using System.Web.Http;

namespace EVF.Helper
{
    public class ApiErrorHandler : IApiErrorHandler
    {

        #region [Fields]

        /// <summary>
        /// The mapping table between exception and HTTP status code.
        /// </summary>
        private readonly IReadOnlyDictionary<Type, HttpStatusCode> _httpStatusCodeExceptionMapper =
            new Dictionary<Type, HttpStatusCode>
            {
                { typeof (ArgumentException), HttpStatusCode.BadRequest },
                { typeof (AuthenticationException), HttpStatusCode.Unauthorized },
                { typeof (UnauthorizedAccessException), HttpStatusCode.Forbidden },
                { typeof (ObjectNotFoundException), HttpStatusCode.NotFound }
                // Other exceptions go HttpStatusCode.InternalServerError
            };

        #endregion


        #region [Methods]

        /// <summary>
        /// Gets the HTTP response message with an API error model.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns>A HTTP response message.</returns>
        public HttpResponseMessage GetHttpResponseMessage(Exception ex)
        {
            // Creates error model from exception.
            var statusCode = GetHttpStatusCode(ex);

            // Hides real reason when HTTP status code is 500.
            var apiErrorModel = new ApiErrorModel { Message = ex.Message };

            // Creates response message with API error model.
            var result = new HttpResponseMessage(statusCode)
            {
                Content = new ObjectContent<ApiErrorModel>(
                apiErrorModel, new JsonMediaTypeFormatter(), "application/json")
            };

            return result;
        }

        /// <summary>
        /// Gets the HTTP status code from exception by using mapper.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns>A HTTP status code.</returns>
        public HttpStatusCode GetHttpStatusCode(Exception ex)
        {
            // Sets default value of returning value.
            var result = HttpStatusCode.InternalServerError;

            // Gets HTTP status code by using mapping table.
            KeyValuePair<Type, HttpStatusCode> mapper =
                _httpStatusCodeExceptionMapper.FirstOrDefault(m => m.Key.IsAssignableFrom(ex.GetType()));
            if (!mapper.Equals(default(KeyValuePair<Type, HttpStatusCode>)))
            {
                result = mapper.Value;
            }

            return result;
        }

        /// <summary>
        /// Throws the HTTP response message by an API error model.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns>A HTTP response exception.</returns>
        public HttpResponseException ThrowErrorModel(Exception ex)
        {
            return new HttpResponseException(GetHttpResponseMessage(ex));
        }

        #endregion

    }
}
