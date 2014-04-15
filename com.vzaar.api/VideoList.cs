using System;

namespace com.vzaar.api
{
	public class Video
	{
		public string version;
		public Int64 id;
		public string title;
		public string description;
		public DateTime createdAt;
		public decimal duration;

		public string status;
		public int statusId;

		public string url;
		public string thumbnail;

		public int height;
		public int width;

		public Int64 playCount;
		public VideoAuthor user;
	}

	public class VideoAuthor
	{
		public string name;
		public string url;
		public int account;
		public Int64 videoCount;
	}
}
