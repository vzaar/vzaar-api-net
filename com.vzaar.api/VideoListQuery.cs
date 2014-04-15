using System;

namespace com.vzaar.api
{
	public class VideoListQuery
	{
		public string username = String.Empty;
		public bool includePrivateVideos = false;
		public int count = 20;
		public int page = 1;
		public VideoListSorting sort = VideoListSorting.DESCENDING;
		public string[] labels = new string[] { };
		public string status = String.Empty;
		public string title = String.Empty;
	}

	public enum VideoListSorting
	{
		ASCENDING = 0,
		DESCENDING = 1
	}
}
