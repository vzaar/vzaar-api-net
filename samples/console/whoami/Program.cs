using System;
using com.vzaar.api;

namespace whoami
{
	class Program
	{
		static void Main(string[] args)
		{
			var api = new Vzaar("username", "token");
			Console.WriteLine(api.whoAmI());
		}
	}
}
