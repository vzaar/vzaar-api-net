using System.Net;

/**
 * FormWebResponse
 * @author Maxim Fridberg
 * @updated November 29, 2011
 **/

namespace VzaarAPI.Transport
{
    /// <summary>
    /// This class wraps the HTTP response, it contains the status code, status description
    /// and the response stream.
    /// </summary>
    public class FormWebResponse
    {
       

        #region Public Methods                

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="statusCode">The HTTP status code</param>
        /// <param name="statusLine">The HTTP status line</param>
        /// <param name="response">The response stream</param>
        public FormWebResponse(HttpStatusCode statusCode, string statusLine, string response)
        {
            StatusCode = statusCode;
            StatusLine = statusLine;
            Response = response;
        }


        /// <summary>
        /// The StatusCode
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }


        /// <summary>
        /// Get the HTTP status line 
        /// </summary>
        public string StatusLine{get; private set;}


        /// <summary>
        /// Get the response input stream.
        /// </summary>
        public string Response { get; set; }

        #endregion
    }
}
