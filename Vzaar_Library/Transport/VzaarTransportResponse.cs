using System.IO;
using System.Net;

namespace VzaarAPI.Transport
{
    /// <summary>
    /// This class wraps the HTTP response, it contains the status code, status description
    /// and the response stream.
    /// </summary>
    public class VzaarTransportResponse
    {
        #region Private Members 
        
        private HttpStatusCode _statusCode;
        private string _statusLine;
        private Stream _response;

        #endregion

        #region Public Methods                

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="statusCode">The HTTP status code</param>
        /// <param name="statusLine">The HTTP status line</param>
        /// <param name="response">The response stream</param>
        public VzaarTransportResponse(
            HttpStatusCode statusCode, string statusLine, Stream response)
        {
            _statusCode = statusCode;
            _statusLine = statusLine;
            _response = response;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>The StatusCode</returns>
        public HttpStatusCode GetStatusCode()
        {
            return _statusCode;
        }


        /// <summary>
        /// Get the HTTP status line 
        /// </summary>
        /// <returns>The HTTP status line</returns>
        public string GetStatusLine()
        {
            return _statusLine;
        }


        /// <summary>
        /// Get the response input stream.
        /// </summary>
        /// <returns>The response input stream</returns>
        public Stream GetResponse()
        {
            return _response;
        }

        #endregion
    }
}
