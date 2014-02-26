using System;
using System.Collections.Generic;

namespace VzaarAPI
{
    /// <summary>
    /// An assisting class to for Vzaar API requests.
    /// </summary>
    public class VzaarRequest
    {        
        #region Protected Members         

        /// <summary>
        /// Set of parameters for the request
        /// </summary>
        protected Dictionary<string, object> _parameters;

        #endregion
        
        #region Public Methods         
        
        /// <summary>
        /// Retrieve a dictionary of parameters
        /// </summary>        
        /// <returns>Return the dictionary of parameters</returns>
        public Dictionary<string, object> GetParameters()
        {
            return _parameters;
        }


        /// <summary>
        /// Add a parameter to the list of parameters. If the value is null
        /// then the parameter key is removed from the list.
        /// </summary>        
        /// <param name="name">The name/key of the parameter</param>
        /// <param name="value">The value of the parameter</param>
        public void PutParameter(string name, Object value)
        {

            if (value == null)
            {
                _parameters.Remove(name);
            }
            else
            {
                _parameters.Add(name, value.ToString());
            }
        }


        /// <summary>
        /// Remove a parameter from the parameter list.
        /// </summary>        
        /// <param name="name">The name/key of the parameter</param>
        public void RemoveParameter(string name)
        {
            _parameters.Remove(name);
        }


        /// <summary>
        /// Remove a parameter from the parameter list.
        /// </summary>        
        /// <param name="name">The name/key of the parameter</param>
        /// <returns></returns>
        public string GetParameter(string name)
        {
            string value;

            try
            {
                value = (string)_parameters[name];
            }
            catch (Exception)
            {
                value = null;
            }

            return value;
        }

        #endregion
        
        #region Protected Methods 

        /// <summary>
        /// A parameterless constructor
        /// </summary>                
        protected VzaarRequest()
        {
            _parameters = new Dictionary<string, object>();
        }


        /// <summary>
        /// Initialised parameter constructor.
        /// </summary>        
        /// <param name="parameters"></param>
        protected VzaarRequest(Dictionary<string, object> parameters)
        {
            _parameters = parameters;
        }


        /// <summary>
        /// Get the value as an integer or null if it doesn't exist.
        /// </summary>        
        /// <param name="name">The name of the parameter</param>
        /// <returns>The value of the named parameter, or null if not set</returns>
        protected int GetInteger(string name)
        {
            string value = GetParameter(name);

            if (value != null)
            {
                try
                {
                    return Convert.ToInt32(value);
                }
                catch (Exception)
                { }
            }
            return -1;
        }


        /// <summary>
        /// Get the value as an boolean or null if it doesn't exist.
        /// </summary>        
        /// <param name="name">The name of the parameter</param>
        /// <returns>The value of the named parameter, or null if not set</returns>
        protected bool? GetBoolean(string name)
        {
            var value = GetParameter(name);

            if (value != null)
            {
                try
                {
                    return Convert.ToBoolean(value);
                }
                catch (Exception) { }
            }
            return null;
        }

        #endregion        
    }
}
