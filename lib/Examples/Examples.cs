using System;
using System.Collections.Generic;
using VzaarApi;

namespace Examples
{
	public class Examples
	{
		public Examples ()
		{
		}

		public static void Run(Dictionary<string, object> tokens) {

			// -- set the values

			int presetId = (int)tokens["presetId"];
			int categoryId = (int)tokens["categoryId"];

			int videoId = (int)tokens["videoId"];

			string videopath = (string)tokens["videopath"];
			string imagepath = (string)tokens["imagepath"];
			string subtitlespath = (string)tokens["subtitlespath"];
			string videourl = (string)tokens["videourl"];


			Console.WriteLine("##EncodingPresets##");

				//EncodingPresets.ReadingEncodingPresetsList (Client.client_id, Client.auth_token );
				//EncodingPresets.UsingEncodingPreset (Client.client_id, Client.auth_token, presetId);


			Console.WriteLine ();
			Console.WriteLine("##IngestRecipes##");

				//IngestRecipes.ReadingIngestRecipesList(Client.client_id, Client.auth_token);
				//IngestRecipes.UsingIngestRecipe (Client.client_id, Client.auth_token);


			Console.WriteLine ();
			Console.WriteLine("##Videos##");

				//Videos.ReadingVideosList(Client.client_id, Client.auth_token);
				//Videos.UsingVideoCreateGuid(Client.client_id, Client.auth_token,videopath);
				//Videos.UsingVideoCreateFromFile(Client.client_id, Client.auth_token,videopath);
				//Videos.UsingVideoCreateUrl (Client.client_id, Client.auth_token, videourl);


			Console.WriteLine ();
			Console.WriteLine("##VideoCategories##");

				//VideoCategories.ReadingCategoriesList (Client.client_id, Client.auth_token);
				//VideoCategories.ReadingSubtreeCategoriesList (Client.client_id, Client.auth_token, categoryId);
				//VideoCategories.UsingVideoCategory (Client.client_id, Client.auth_token);

			Console.WriteLine ();
			Console.WriteLine("##UploadSignatures##");

				//UploadSignatures.ReadingSignature (Client.client_id, Client.auth_token, videopath);

			Console.WriteLine ();
			Console.WriteLine("##LinkUploads##");

				//LinkUploads.UsingLinkUploadParameters (Client.client_id, Client.auth_token, videourl);
				//LinkUploads.UsingLinkUploadUrlString (Client.client_id, Client.auth_token, videourl);

			Console.WriteLine ();
			Console.WriteLine("##Playlists##");

				//Playlists.UsingPlaylist (Client.client_id, Client.auth_token, categoryId);
				//Playlists.ReadingPlaylistsList (Client.client_id, Client.auth_token);

			Console.WriteLine ();
			Console.WriteLine("##Subtitles##");

				//Subtitles.UsingSubtitle (Client.client_id, Client.auth_token, videoId, subtitlespath);	
				//Subtitles.ReadingSubtitlesList (Client.client_id, Client.auth_token, videoId);

			Console.WriteLine ();
			Console.WriteLine("##ImageFrame##");

				//ImageFrame.UsingImageFrame (Client.client_id, Client.auth_token, videoId, imagepath);	

		}
	}
}

