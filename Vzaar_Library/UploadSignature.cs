using System;

namespace VzaarAPI
{
    /// <summary>
    /// This class holds all the information returned from the Signature API call
    /// as show here (http://developer.vzaar.com/docs/version_1.0/uploading/sign).
    /// This call takes care of producing a signature that is used to then upload
    /// the file to Amazon's Web Services S3 server cloud.
    /// </summary>
    public class UploadSignature
    {
        #region Private Members

        private string _guid;

        public string guid
        {
            get
            {
                return _guid;
            }
            set
            {
                _guid = value;
            }
        }
        private string _key;

        public string key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }
        private bool _https;

        public bool https
        {
            get
            {
                return _https;
            }
            set
            {
                _https = value;
            }
        }
        private string _acl;

        public string acl
        {
            get
            {
                return _acl;
            }
            set
            {
                _acl = value;
            }
        }
        private string _bucket;

        public string bucket
        {
            get
            {
                return _bucket;
            }
            set
            {
                _bucket = value;
            }
        }
        private string _policy;

        public string policy
        {
            get
            {
                return _policy;
            }
            set
            {
                _policy = value;
            }
        }
        private string _expirationDate;

        public string expirationDate
        {
            get
            {
                return _expirationDate;
            }
            set
            {
                _expirationDate = value;
            }
        }
        private string _accessKeyId;

        public string accessKeyId
        {
            get
            {
                return _accessKeyId;
            }
            set
            {
                _accessKeyId = value;
            }
        }
        private string _signature;

        public string signature
        {
            get
            {
                return _signature;
            }
            set
            {
                _signature = value;
            }
        }

        #endregion

        #region Public and Package Protected Methods

        /// <summary>
        /// Signature Constructor
        /// </summary>
        /// <param name="guid">The vzaar global unique identifier</param>
        /// <param name="key">A name for the S3 object that will store the uploaded file's data</param>
        /// <param name="https">Whether https was used</param>
        /// <param name="acl">The access control policy to apply to the uploaded file</param>
        /// <param name="bucket">The vzaar bucket that has been allocated for this file</param>
        /// <param name="policy">a Base64-encoded policy document that applies rules to 
        /// file uploads sent by the S3 POST form. This document is used to authorise 
        /// the form, and to impose conditions on the files that can be uploaded.</param>
        /// <param name="expirationDate">Greenwich Mean Time (GMT) timestamp that 
        /// specifies when the policy document will expire. Once a policy document 
        /// has expired, the upload will fail</param>
        /// <param name="accessKeyId">The vzaar AWS Access Key Identifier credential</param>
        /// <param name="signature">A signature value that authorises the form and proves 
        /// that only vzaar could have created it. This value is calculated by signing 
        ///	the policy document</param>
        public UploadSignature(string guid, string key, bool https, string acl, string bucket, string policy, string expirationDate, string accessKeyId, string signature)
        {
            _guid = guid;
            _key = key;
            _https = https;
            _acl = acl;
            _bucket = bucket;
            _policy = policy;
            _expirationDate = expirationDate;
            _accessKeyId = accessKeyId;
            _signature = signature;
        }

        /// <summary>
        /// The vzaar global unique identifier.
        /// </summary>
        /// <returns>The vzaar global unique identifier</returns>
        public string GetGuid()
        {
            return _guid;
        }

        /// <summary>
        /// A name for the S3 object that will store the uploaded file's data
        /// </summary>
        /// <returns>A name for the S3 object that will store the uploaded file's data</returns>
        public string GetKey()
        {
            return _key;
        }

        /// <summary>
        /// Whether the request uses https
        /// </summary>
        /// <returns>The https indicator</returns>
        public bool IsHttps()
        {
            return _https;
        }


        /// <summary>
        /// The access control policy to apply to the uploaded file.
        /// </summary>
        /// <returns>The access control policy to apply to the uploaded file</returns>
        public string GetAcl()
        {
            return _acl;
        }

        /// <summary>
        /// The vzaar bucket that has been allocated for this file
        /// </summary>
        /// <returns>The vzaar bucket that has been allocated for this file</returns>
        public string GetBucket()
        {
            return _bucket;
        }

        /// <summary>
        /// A Base64-encoded policy document that applies rules to file uploads 
        /// sent by the S3 POST form. This document is used to authorise the form, 
        /// and to impose conditions on the files that can be uploaded.
        /// </summary>
        /// <returns>a Base64-encoded policy document that applies rules to file 
        /// uploads sent by the S3 POST form.</returns>
        public string GetPolicy()
        {
            return _policy;
        }

        /// <summary>
        /// A Greenwich Mean Time (GMT) timestamp that specifies when the policy 
        /// document will expire. Once a policy document has expired, the upload 
        /// will fail.
        /// </summary>
        /// <returns>A Greenwich Mean Time (GMT) timestamp that specifies when the 
        /// policy document will expire. </returns>
        public string GetExpirationDate()
        {
            return _expirationDate;
        }

        /// <summary>
        /// The vzaar AWS Access Key Identifier credential.
        /// </summary>
        /// <returns>The vzaar AWS Access Key Identifier credential</returns>
        public string GetAccessKeyId()
        {
            return _accessKeyId;
        }

        /// <summary>
        /// A signature value that authorises the form and proves that only vzaar 
        /// could have created it. This value is calculated by signing the policy 
        /// document.
        /// </summary>
        /// <returns>a signature value that authorizes the form and proves that 
        ///	only vzaar could have created it</returns>
        public string GetSignature()
        {
            return _signature;
        }


        /// <summary>
        /// String representation of the upload document.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("guid={0}, key={1}, http={2}, acl={3}, bucket={4}, policy={5}, expirationdate={6}, accesskeyid={7}, signature={8}",
                _guid, _key, _https, _acl, _bucket, _policy, _expirationDate, _accessKeyId, _signature);
        }

        #endregion
    }
}
