using System;
using com.vzaar.api;

namespace upload
{
	class Program
	{
		static void Main(string[] args)
		{
			var api = new Vzaar("username", "token");

			if (args.Length != 1)
			{
				Console.WriteLine("Invalid number of arguments. File path is required");
			}
			else
			{
				var filePath = args[0];
				var guid = api.uploadVideo(filePath);
				Console.WriteLine("-- Uploaded. vzaar GUID: " + guid);
			}
		}
	}
}
