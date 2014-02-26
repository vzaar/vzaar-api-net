namespace VzaarAPI.Transport.HttpClient
{
    /// <summary>
    /// The HttpClientTransportFactory is responsible for creating an appropriate
    /// HTTP transport client.
    /// </summary>
    public class HttpClientTransportFactory : VzaarTransportFactory 
    {
	    /// <summary>
        /// Create a transport instance.
	    /// </summary>
        /// <returns>A new transport instance</returns>	
	    public override IVzaarTransport Create() 
        {
		    return new HttpClientTransport();
	    }
    }
}
