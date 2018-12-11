using System;
using System.Collections.Generic;
using VzaarApi;

namespace Examples
{
	class MainClass
	{
		public static void Main (string[] args)
		{

			Client.client_id="<client_id>";
			Client.auth_token = "<auth_token>";

			Client.url = "https://api.vzaar.com/api/";
			//Client.urlAuth = true; 

			//Helper parameters
			Dictionary<string, object> tokens = new Dictionary<string, object> () {
				{ "presetId",  <preset_id> },
				{ "categoryId", <category_id> },
				{ "videoId", <video_id> },
				{ "videopath", "<video filepath>" },
				{ "imagepath", "<image frame filepath>" },
				{ "subtitlespath", "<subtitle filepath>" },
				{ "videourl", "<video link url>" }
			};

			Examples.Run (tokens);

		}
	}
}
