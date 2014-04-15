using System;

namespace com.vzaar.api
{
	public class VideoDetails
	{
		public string version;
		public string type;

		public string title;
		public string description;
		public bool borderless;

		public decimal duration;

		public int height;
		public int width;

		public string url;
		public string poster;
		public string html;

		public Int64 playCount;
		public Int64 totalSize;

		public VideoDetailsAuthor author;
		public VideoDetailsProvider provider;
		public VideoDetailsThumbnail thumbnail;
		public VideoDetailsFramegrab framegrab;
		public VideoDetailsVideoStatus videoStatus;

	}

	public class VideoDetailsVideoStatus
	{
		public int id;
		public string description;
	}

	public class VideoDetailsAuthor
	{
		public string name;
		public string url;
	}

	public class VideoDetailsProvider
	{
		public string name;
		public string url;
	}

	public class VideoDetailsThumbnail
	{
		public int width;
		public int height;
		public string url;
	}

	public class VideoDetailsFramegrab
	{
		public int width;
		public int height;
		public string url;
	}
}
