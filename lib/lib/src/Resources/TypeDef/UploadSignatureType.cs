using System;

namespace VzaarApi
{
	/*The class is defined to be used with ToTypeDef method*/

	public class UploadSignatureType
	{
		// public string x-amz-signature;
		// public string x-amz-credential;
		// public string x-amz-algorithm;
		// public string x-amz-date;
		/*
		 * not possible to define the fields variables
		 * '-' is is not allowed in variable names in C#
		 * access the property using indexer 
		 * 
		 * example: <object_name>["x-amz-date"]
		 * 
		*/

		public string key;
		public string acl;
		public string policy;
		public string success_action_status;
		public string content_type;
		public string guid;
		public string bucket;
		public string upload_hostname;

		/*
		 * 
		 * For signature of type Single the fields:
		 * 
		 * 'part_size', 'part_size_in_bytes', 'parts'
		 * 
		 * are set to 'null'
		 * 
		 */

		public string part_size;
		public long? part_size_in_bytes;
		public long? parts;

	}
}

