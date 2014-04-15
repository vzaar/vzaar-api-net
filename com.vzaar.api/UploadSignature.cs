using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Xml;

namespace com.vzaar.api
{
	public class UploadSignature
	{
		private string _signature;
		private string _toJson;

		public string guid;
		public string bucket;
		public DateTime expirationDate;
		public string signature;
		public string acl;
		public string key;
		public string accessKeyId;
		public string policy;
		public bool https;

		public UploadSignature()
		{
		}

		public UploadSignature(string response)
		{
			this._signature = response;

			//parse actual signature
			_toJson = JsonConvert.SerializeXmlNode(toXmlDocument().SelectSingleNode("//vzaar-api"), Newtonsoft.Json.Formatting.None, true);
			var jo = (JObject)JsonConvert.DeserializeObject(_toJson);
			guid = (string)jo["guid"];
			bucket = (string)jo["bucket"];
			expirationDate = DateTime.Parse((string)jo["expirationdate"].ToString());
            signature = (string)jo["signature"];
			acl = (string)jo["acl"];
			key = (string)jo["key"];
			accessKeyId = (string)jo["accesskeyid"];
			policy = (string)jo["policy"];
			https = (bool)jo["https"];
		}

		public string toXml()
		{
			return _signature;
		}

		public XmlDocument toXmlDocument()
		{
			var doc = new XmlDocument();
			doc.LoadXml(_signature);
			return doc;
		}

		public string toJson()
		{
			return _toJson;
		}
	}
}
