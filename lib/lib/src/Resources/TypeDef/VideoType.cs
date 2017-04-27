using System;
using System.Collections.Generic;

namespace VzaarApi
{
	/*The class is defined to be used with ToTypeDef method*/
	
	public class VideoType
	{

		public long? id;
		public string title;
		public long? user_id;
		public long? account_id;
		public string description;
		public double? duration;

		//public bool? private; 
		/*
		 * not possible to define the field variable
		 * 'private' is restricted key-word in C#
		 * access the property using indexer 
		 * 
		 * example: <object_name>["private"]
		 * 
		*/

		//public bool private;

		public string seo_url;
		public string url;
		public string state;
		public string thumbnail_url;
		public string embed_code;

		public DateTime created_at;
		public DateTime updated_at;

		public List<VideoCategoryType> categories;
		public List<RenditionType> renditions;
		public List<LegacyRenditionType> legacy_renditions;

	}

	public class RenditionType {
		
		public long? id;
		public long? width;
		public long? height;
		public long? bitrate;
		public string framerate;
		public string status;
		public long? size_in_bytes;
	}

	public class LegacyRenditionType {

		public long? id;
		public long? width;
		public long? height;
		public long? bitrate;
		public string status;
		public string type;

	}
}