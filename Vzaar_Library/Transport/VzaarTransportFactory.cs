using VzaarAPI.Transport.HttpClient;

namespace VzaarAPI.Transport
{
    /// <summary>
    /// This class is responsible for creating IVzaarTransport objects.
    /// </summary>
    public abstract class VzaarTransportFactory
    {        
        #region Private Static Members         

        private static VzaarTransportFactory factory = new HttpClientTransportFactory();

        #endregion
        
        #region Public Static Methods 
     

        /// <summary>
        /// Get the default registered factory.
        /// </summary>
        /// <returns>The default factory</returns>
        public static VzaarTransportFactory GetDefaultFactory()
        {
            return factory;
        }


        /// <summary>
        /// Set the default factory.
        /// </summary>
        /// <param name="factory">The default factory </param>
        public static void SetDefaultFactory(VzaarTransportFactory factory)
        {
            VzaarTransportFactory.factory = factory;
        }


        /// <summary>
        /// Create a transport instance using the default transport factory.
        /// </summary>
        /// <returns>A new transport instance</returns>
        public static IVzaarTransport CreateDefaultTransport()
        {
            return factory.Create();
        }


        /// <summary>
        /// Create a transport instance.
        /// </summary>
        /// <returns>A new transport instance</returns>
        public abstract IVzaarTransport Create();

        #endregion
    }
}
