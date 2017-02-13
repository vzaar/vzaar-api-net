using System;
using VzaarApi;

namespace Examples
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Client.client_id="id";
			Client.auth_token = "token";
			//Client.urlAuth = true; 

			Examples.Run ();

		}
	}
}
