using System;

namespace VzaarAPI
{
    /// <summary>
    /// A specific exception class to handle Vzaar API exceptions
    /// </summary>
    public class VzaarException : Exception
    {
        private string _errorMessage;     
        /// <summary>
        /// The error message raised by the exception
        /// </summary>   
        public string ErrorMessage
        {
            get { return _errorMessage; }
        }

        private Exception _exception;
        /// <summary>
        /// The original exception object
        /// </summary>
        public Exception ExceptionObject
        {
            get { return _exception; }
        }

        /// <summary>
        /// A single parameter constructor.
        /// </summary>
        /// <param name="errorMessage">The error message of the originating exception</param>
        public VzaarException(string errorMessage)
            : base(errorMessage)
        {
            _errorMessage = errorMessage;            
        }


        /// <summary>
        /// A parameter based construstor
        /// </summary>
        /// <param name="errorMessage">The error message of the originating exception</param>
        /// <param name="ex">The originating exception object</param>
        public VzaarException(string errorMessage, Exception ex)
            : base(errorMessage, ex)
        {
            _errorMessage = errorMessage;
            _exception = ex;
        }
    }
}
