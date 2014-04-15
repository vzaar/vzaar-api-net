using System;
using com.vzaar.api;
using utils;

namespace user_details
{
	class Program
	{
		static void Main(string[] args)
		{
			var api = new Vzaar("skitsanos", "abfDnd3DVI71jQ7dTtz9mHA953XeIQeodmZvSE6AbTX8");
			var details = api.getUserDetails("skitsanos");

			Console.WriteLine("Videos owned: " + details.videoCount);
			Console.WriteLine("Storage used: " + String.Format(new FileSizeFormatProvider(), "File size: {0:fs}", details.videosTotalSize));
			Console.WriteLine("Bandwidth this month: " + String.Format(new FileSizeFormatProvider(), "File size: {0:fs}", details.bandwidthThisMonth));
		}
	}
}
