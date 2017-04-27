using System;
using VzaarApi;

namespace Examples
{
	public class Examples
	{
		public Examples ()
		{
		}

		public static void Run() {

			// -- set the values

			int presetId = 10;
			int categoryId = 3193;

			string filepath ="<filepath>";
			string url = "http://techslides.com/demos/sample-videos/small.mp4";


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
				//Videos.UsingVideoCreateGuid(Client.client_id, Client.auth_token,filepath);
				//Videos.UsingVideoCreateFromFile(Client.client_id, Client.auth_token,filepath);
				//Videos.UsingVideoCreateUrl (Client.client_id, Client.auth_token, url);


			Console.WriteLine ();
			Console.WriteLine("##VideoCategories##");

				//VideoCategories.ReadingCategoriesList (Client.client_id, Client.auth_token);
				//VideoCategories.ReadingSubtreeCategoriesList (Client.client_id, Client.auth_token, categoryId);
				//VideoCategories.UsingVideoCategory (Client.client_id, Client.auth_token);

			Console.WriteLine ();
			Console.WriteLine("##UploadSignatures##");

				//UploadSignatures.ReadingSignature (Client.client_id, Client.auth_token, filepath);

			Console.WriteLine ();
			Console.WriteLine("##LinkUploads##");

				//LinkUploads.UsingLinkUploadParameters (Client.client_id, Client.auth_token, url);
				//LinkUploads.UsingLinkUploadUrlString (Client.client_id, Client.auth_token, url);

			Console.WriteLine ();
			Console.WriteLine("##Playlists##");

				//Playlists.UsingPlaylist (Client.client_id, Client.auth_token, categoryId);
				//Playlists.ReadingPlaylistsList (Client.client_id, Client.auth_token);
		}
	}
}

