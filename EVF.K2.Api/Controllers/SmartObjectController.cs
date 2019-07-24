using EVF.K2.Bll.Interfaces;
using EVF.K2.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace EVF.K2.Api.Controllers
{
    public class SmartObjectController : ApiController
    {
        #region [Fields]

        /// <summary>
        /// The workflow manager provides workflow functionality.
        /// </summary>
        private readonly ISmartObject _smartObject;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="SmartObjectController" /> class.
        /// </summary>
        /// <param name="smartObject"></param>
        public SmartObjectController(ISmartObject smartObject)
        {
            _smartObject = smartObject;
        }

        #endregion

        #region [Methods]

        [HttpPost]
        [Route("GetSmartObject")]
        public IHttpActionResult GetSmartObject(SmartObjectModel model)
        {
            return Ok(_smartObject.GetSmartObject(model));
        }

        #endregion
    }
}