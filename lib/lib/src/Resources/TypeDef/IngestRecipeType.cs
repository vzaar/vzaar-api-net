using System;
using System.Collections.Generic;

namespace VzaarApi
{
	public class IngestRecipeType
	{
		/*The class is defined to be used with ToTypeDef method*/

		public long? id;
		public string name;
		public string recipe_type;
		public string description;
		public long? account_id;
		public long? user_id;

		/*
		 * not possible to define the field variable
		 * 'default' is restricted key-word in C#
		 * access the property using indexer 
		 * 
		 * example: <object_name>["default"]
		 * 
		*/

		//public bool? default;

		public bool? multipass;
		public string frame_grab_time;

		public bool? generate_animated_thumb;
		public bool? generate_sprite;
		public bool? use_watermark;
		public bool? send_to_youtube;
		public bool? notify_by_email;
		public bool? notify_by_pingback;

		public List<EncodingPresetType> encoding_presets;

		public DateTime created_at;
		public DateTime updated_at;

	}
}

