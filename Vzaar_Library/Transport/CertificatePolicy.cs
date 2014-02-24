using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace VzaarAPI.Certificates
{
    public class CertificatePolicy : ICertificatePolicy
    {
        public bool CheckValidationResult(ServicePoint srvPoint, X509Certificate certificate, WebRequest request, int certificateProblem)
        {
            //Return True to force the certificate to be accepted.
            return true;

        } // end CheckValidationResult
    }
}