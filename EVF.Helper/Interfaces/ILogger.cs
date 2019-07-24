using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EVF.Helper.Interfaces
{
    /// <summary>
    /// Interface ILogger provides logging to track all activities by using NLog library.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Creates a new session for logging.
        /// </summary>
        void CreateNewSession(HttpContext context);

        /// <summary>
        /// Logs the debug, the message will be logged when debug mode is on.
        /// </summary>
        /// <param name="message">The message.</param>
        void Debug(string message);

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="message">The additional message.</param>
        void Error(Exception ex, string message = null);

        /// <summary>
        /// Logs the information such as start and end.
        /// </summary>
        /// <param name="message">The message.</param>
        void Info(string message);
    }
}
