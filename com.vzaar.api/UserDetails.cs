using System;
using System.Collections.Generic;

namespace com.vzaar.api
{
	public class UserDetails
	{
		public string version;
		public string authorName;
		public int authorId;
		public string authorUrl;
		public int authorAccount;
		public string authorAccountTitle;
		public DateTime createdAt;
		public Int64 videoCount;
		public Int64 playCount;
		public Int64 bandwidthThisMonth;
		public List<UserBandwidthDetails> bandwidth = new List<UserBandwidthDetails>();
		public Int64 videosTotalSize;
		public Int64 maxFileSize;
	}

	public class UserBandwidthDetails
	{
		public int month;
		public int year;
		public Int64 bandwidth;
	}
}
