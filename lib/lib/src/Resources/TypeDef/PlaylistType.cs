using System;

namespace VzaarApi
{
	/*The class is defined to be used with ToTypeDef method*/

	public class PlaylistType
	{
		public long? id;
		public long? category_id;
		public string title;
		public string sort_order;
		public string sort_by;
		public long? max_vids;
		public string position;

		/*
		 * not possible to define the field variable
		 * 'private' is restricted key-word in C#
		 * access the property using indexer 
		 * 
		 * example: <object_name>["private"]
		 * 
		*/

		//public bool? private;

		public string dimensions;
		public bool? autoplay;
		public bool? continuous_play;
		public string embed_code;
		public DateTime created_at;
		public DateTime updated_at;

	}
}