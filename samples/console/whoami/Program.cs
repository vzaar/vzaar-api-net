using System;
using com.vzaar.api;

namespace whoami
{
	class Program
	{
		static void Main(string[] args)
		{
			var api = new Vzaar("skitsanos", "zfGnd3DVI71jQ7dTtz9mHA953XeIQeodmZvSE6AbTX8");
			Console.WriteLine(api.whoAmI());
		}
	}
}
