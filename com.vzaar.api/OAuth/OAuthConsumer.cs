using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace com.vzaar.api.OAuth
{
	public class OAuthConsumer
	{
		#region Private Members

		public string consumerKey;
		public string consumerSecret;
		public string token;
		public string tokenSecret;
		public OAuthBase.SignatureTypes signatureMethod;
		public OAuthBase oAuth;
		public string timestamp;
		public string nonce;

		#endregion

		#region Public Method

		/// <summary>
		/// A Parameterless constructor. Defaults the consumer key and secret to string.Empty
		/// and the encryption method to HMAC-SHA1.
		/// </summary>
		public OAuthConsumer()
		{
			consumerKey = string.Empty;
			consumerSecret = string.Empty;
			signatureMethod = OAuthBase.SignatureTypes.HMACSHA1;
			oAuth = new OAuthBase();
		}

		/// <summary>
		/// A constructor that only takes the encryption method. The consumer key
		/// and secret are defaulted to string.Empty.
		/// </summary>
		/// <param name="signatureMethod">The signature encryption method</param>
		public OAuthConsumer(OAuthBase.SignatureTypes signatureMethod)
		{
			consumerKey = string.Empty;
			consumerSecret = string.Empty;
			this.signatureMethod = signatureMethod;
			oAuth = new OAuthBase();
		}


		/// <summary>
		/// A constructor that take the consumer key, consumer secret and signature method.
		/// </summary>
		/// <param name="consumerKey">Consumer key</param>
		/// <param name="consumerSecret">Consumer secret</param>
		/// <param name="signatureMethod">Signature encryption method</param>
		public OAuthConsumer(string consumerKey, string consumerSecret, OAuthBase.SignatureTypes signatureMethod)
		{
			this.consumerKey = consumerKey;
			this.consumerSecret = consumerSecret;
			this.signatureMethod = signatureMethod;
			oAuth = new OAuthBase();
		}

		/// <summary>
		/// Takes a HttpWebRequest object, signs an oAuth header and attaches it
		/// to the request object.
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public HttpWebRequest Sign(HttpWebRequest request)
		{
			// Create the nonce and timestamp
			timestamp = oAuth.GenerateTimeStamp();
			nonce = oAuth.GenerateNonce();

			// Build the oAuth parameter header
			var oauthParams = BuildOAuthParameterMap();
			string urlOut, paramsOut;

			// Generate a signature with the passed parameters
			string signature = oAuth.GenerateSignature(request.RequestUri, consumerKey, consumerSecret, token, tokenSecret, request.Method, timestamp, nonce, out urlOut, out paramsOut);

			// Add the oAuth authorisation header to the HttpWebRequest            
			request.Headers.Add(OAuthBase.HTTP_AUTHORIZATION_HEADER, BuildOAuthHeader(oauthParams, signature));

			return request;
		}

		/// <summary>
		/// Assign the object the oAuth token and secret
		/// </summary>
		/// <param name="token">The oAuth token</param>
		/// <param name="tokenSecret">The oAuth secret</param>
		public void SetTokenWithSecret(string token, string tokenSecret)
		{
			this.token = token;
			this.tokenSecret = tokenSecret;
		}
		#endregion

		#region Private Methods

		/// <summary>
		/// Compile the oAuth header with the supplied paramteres
		/// </summary>
		/// <returns></returns>
		private Dictionary<string, string> BuildOAuthParameterMap()
		{
			var map = new Dictionary<string, string>();

			// Apply the appropriate textual description, dependent on what
			// signature type was supplied.
			string signatureDescription;
			switch (signatureMethod)
			{
				case OAuthBase.SignatureTypes.HMACSHA1:
					signatureDescription = "HMAC-SHA1";
					break;
				case OAuthBase.SignatureTypes.PLAINTEXT:
					signatureDescription = "PLAINTEXT";
					break;
				case OAuthBase.SignatureTypes.RSASHA1:
					signatureDescription = "RSA-SHA1";
					break;
				default:
					signatureDescription = "PLAINTEXT";
					break;
			}

			// Add the oAuth parameters
			map.Add(OAuthBase.OAuthConsumerKeyKey, consumerKey);
			map.Add(OAuthBase.OAuthSignatureMethodKey, signatureDescription);
			map.Add(OAuthBase.OAuthTimestampKey, timestamp);
			map.Add(OAuthBase.OAuthNonceKey, nonce);
			map.Add(OAuthBase.OAuthVersionKey, OAuthBase.OAuthVersion);
			map.Add(OAuthBase.OAuthTokenKey, token);

			return map;
		}

		/// <summary>
		/// Build the oAuth header with the supplied parameters and signature.
		/// </summary>
		/// <param name="oauthParams">Set of oAuth parameters</param>
		/// <param name="signature">The signature of the request</param>
		/// <returns></returns>
		private string BuildOAuthHeader(Dictionary<string, string> oauthParams, string signature)
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("OAuth ");

			foreach (string key in oauthParams.Keys)
			{
				string value = oauthParams[key];
				sb.Append(OauthHeaderElement(key, value));
				sb.Append(", ");
			}

			sb.Append(OauthHeaderElement(OAuthBase.OAuthSignatureKey, signature));

			return sb.ToString();
		}

		/// <summary>
		/// Compile header element.
		/// </summary>
		/// <param name="name">The parameter name</param>
		/// <param name="value">The parameter value</param>
		/// <returns></returns>
		private string OauthHeaderElement(string name, string value)
		{
			return HttpUtility.UrlEncode(name) + "=\"" + HttpUtility.UrlEncode(value) + "\"";
		}

		#endregion
	}
}
