using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VzaarApi;

namespace Examples
{
	public class Examples
	{
		public Examples ()
		{
		}

		public async static Task RunAsync(Dictionary<string, object> tokens){

			// -- set the values

			int presetId = (int)tokens["presetId"];
			int categoryId = (int)tokens["categoryId"];

			int videoId = (int)tokens["videoId"];

			string videopath = (string)tokens["videopath"];
			string imagepath = (string)tokens["imagepath"];
			string subtitlespath = (string)tokens["subtitlespath"];
			string videourl = (string)tokens["videourl"];

			Console.WriteLine("##ASYNC - EncodingPresets##");

			await EncodingPresetsAsync.ReadingEncodingPresetsListAsyncCA (Client.client_id, Client.auth_token );
			await EncodingPresetsAsync.UsingEncodingPresetAsync (Client.client_id, Client.auth_token, presetId);
		
			Console.WriteLine ();
			Console.WriteLine("##ASYNC - ImageFrame##");

			await ImageFrameAsync.UsingImageFrameAsync (Client.client_id, Client.auth_token, videoId, imagepath);	

			Console.WriteLine ();
			Console.WriteLine("##ASYNC - IngestRecipes##");

			await IngestRecipesAsync.ReadingIngestRecipesListAsync(Client.client_id, Client.auth_token);
			await IngestRecipesAsync.UsingIngestRecipeAsync (Client.client_id, Client.auth_token);

			Console.WriteLine ();
			Console.WriteLine("##ASYNC - LinkUploads##");

			await LinkUploadsAsync.UsingLinkUploadParametersAsync (Client.client_id, Client.auth_token, videourl);
			await LinkUploadsAsync.UsingLinkUploadUrlStringAsync (Client.client_id, Client.auth_token, videourl);

			Console.WriteLine ();
			Console.WriteLine("##ASYNC - Playlists##");

			await PlaylistsAsync.UsingPlaylistAsync (Client.client_id, Client.auth_token, categoryId);
			await PlaylistsAsync.ReadingPlaylistsListAsync (Client.client_id, Client.auth_token);

			Console.WriteLine ();
			Console.WriteLine("##ASYNC - Subtitles##");

			await SubtitlesAsync.UsingSubtitleAsync (Client.client_id, Client.auth_token, videoId, subtitlespath);	
			await SubtitlesAsync.ReadingSubtitlesListAsync (Client.client_id, Client.auth_token, videoId);

			Console.WriteLine ();
			Console.WriteLine("##ASYNC - UploadSignatures##");

			await UploadSignaturesAsync.ReadingSignatureAsync (Client.client_id, Client.auth_token, videopath);

			Console.WriteLine ();
			Console.WriteLine("##ASYNC - VideoCategories##");

			await VideoCategoriesAsync.ReadingCategoriesListAsync (Client.client_id, Client.auth_token);
			await VideoCategoriesAsync.ReadingSubtreeCategoriesListAsync (Client.client_id, Client.auth_token, categoryId);

			Console.WriteLine ();
			Console.WriteLine("##ASYNC - Videos##");

			await VideosAsync.ReadingVideosListAsync(Client.client_id, Client.auth_token);
		 	await VideosAsync.UsingVideoCreateGuidAsync(Client.client_id, Client.auth_token,videopath);
			await VideosAsync.UsingVideoCreateFromFileAsync(Client.client_id, Client.auth_token,videopath);
			await VideosAsync.UsingVideoCreateUrlAsync (Client.client_id, Client.auth_token, videourl);

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

				EncodingPresets.ReadingEncodingPresetsList (Client.client_id, Client.auth_token );
				EncodingPresets.UsingEncodingPreset (Client.client_id, Client.auth_token, presetId);

			Console.WriteLine ();
			Console.WriteLine("##IngestRecipes##");

				IngestRecipes.ReadingIngestRecipesList(Client.client_id, Client.auth_token);
				IngestRecipes.UsingIngestRecipe (Client.client_id, Client.auth_token);


			Console.WriteLine ();
			Console.WriteLine("##Videos##");

				Videos.ReadingVideosList(Client.client_id, Client.auth_token);
				Videos.UsingVideoCreateGuid(Client.client_id, Client.auth_token,videopath);
				Videos.UsingVideoCreateFromFile(Client.client_id, Client.auth_token,videopath);
				Videos.UsingVideoCreateUrl (Client.client_id, Client.auth_token, videourl);


			Console.WriteLine ();
			Console.WriteLine("##VideoCategories##");

				VideoCategories.ReadingCategoriesList (Client.client_id, Client.auth_token);
				VideoCategories.ReadingSubtreeCategoriesList (Client.client_id, Client.auth_token, categoryId);
				VideoCategories.UsingVideoCategory (Client.client_id, Client.auth_token);

			Console.WriteLine ();
			Console.WriteLine("##UploadSignatures##");

				UploadSignatures.ReadingSignature (Client.client_id, Client.auth_token, videopath);

			Console.WriteLine ();
			Console.WriteLine("##LinkUploads##");

				LinkUploads.UsingLinkUploadParameters (Client.client_id, Client.auth_token, videourl);
				LinkUploads.UsingLinkUploadUrlString (Client.client_id, Client.auth_token, videourl);

			Console.WriteLine ();
			Console.WriteLine("##Playlists##");

				Playlists.UsingPlaylist (Client.client_id, Client.auth_token, categoryId);
				Playlists.ReadingPlaylistsList (Client.client_id, Client.auth_token);

			Console.WriteLine ();
			Console.WriteLine("##Subtitles##");

				Subtitles.UsingSubtitle (Client.client_id, Client.auth_token, videoId, subtitlespath);	
				Subtitles.ReadingSubtitlesList (Client.client_id, Client.auth_token, videoId);

			Console.WriteLine ();
			Console.WriteLine("##ImageFrame##");

				ImageFrame.UsingImageFrame (Client.client_id, Client.auth_token, videoId, imagepath);	

		}
	}
}

