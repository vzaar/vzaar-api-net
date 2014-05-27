using System;

namespace com.vzaar.api
{
	public class AccountDetails
	{
		public string version;
		public int accountId;
		public Int64 bandwidth;
		public string title;
		public AccountRightsDetails rights;
		public AccountCostDetails cost;
	}

	public class AccountRightsDetails
	{
		public bool borderless;
		public bool searchEnhancer;
	}

	public class AccountCostDetails
	{
		public string currency;
		public int monthly;
	}
}
