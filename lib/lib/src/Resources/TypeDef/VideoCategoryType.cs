using System;

namespace VzaarApi
{
	/*The class is defined to be used with ToTypeDef method*/

	public class VideoCategoryType
	{
		public long? id;
		public long? account_id;
		public long? user_id;
		public string name;
		public string description;
		public long? parent_id;
		public long? depth;
		public long? node_children_count;
		public long? tree_children_count;
		public long? node_video_count;
		public long? tree_video_count;
		public DateTime created_at;
		public DateTime updated_at;

	}
}

