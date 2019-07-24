using Newtonsoft.Json;
using System.Net;
using System.Web.Mvc;

namespace EVF.K2.Api.Controllers
{
    public class ErrorHandlerController : Controller
    {
        /// <summary>
        /// Return http status code not found.
        /// </summary>
        /// <returns></returns>
        public ActionResult NotFound()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            Response.TrySkipIisCustomErrors = true;

            return new EmptyResult();
        }

        /// <summary>
        /// Return http status code internal server error.
        /// </summary>
        /// <returns></returns>
        [Route(Name = "ExceptionHandler")]
        public ActionResult Exception(int statusCode = 500, string message = null)
        {
            if (message == null)
            {
                return NotFound();
            }

            Response.StatusCode = statusCode;
            Response.TrySkipIisCustomErrors = true;

            return Content(JsonConvert.SerializeObject(new { Message = message }));
        }

    }
}