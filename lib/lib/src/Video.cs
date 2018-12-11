using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace VzaarApi
{
	public class Video
	{
		internal Record record;
		internal Subtitle subtitle;
		internal SubtitlesList subtitlesList;

		//constructor
		public Video ()
		{
			record = new Record ("videos");

		}

		public Video (Client client)
		{
			record = new Record ("videos", client);
		}

		internal Video (Record item)
		{
			record = item;
		}

		public Client GetClient() {
			return record.RecordClient;
		}

		public object this[string index]{

			get { return record [index];}

			set { record [index] = value; }
		}

		public object ToTypeDef(Type type){

			return record.ToTypeDef (type);

		}

		public bool Edited {
			get { return record.Edited; }
		}

		internal async Task<object> createDataAsync(Dictionary<string, object> tokens) {

			bool containsfile = tokens.ContainsKey ("filepath");
			bool containsguid = tokens.ContainsKey ("guid");
			bool containsurl = tokens.ContainsKey ("url");

			//to return Video created from LinkUpload, otherwise return null
			object result = null;

			if (((containsfile ^ containsguid) ^ containsurl) == true) {

				if (((containsfile & containsguid) & containsurl) == true) {
					throw new VzaarApiException ("Only one of the parameters: guid or url or filepath expected");
				}

				if (containsguid == true) {
					
					record.Create (tokens);

				} else if (containsurl == true) {
					
					result = LinkUpload.Create (tokens,record.RecordClient);

				} else {

					string filepath = (string)tokens ["filepath"];

					FileInfo file = new FileInfo (filepath);

					if(file.Exists == false)
						throw new VzaarApiException("File does not exist: "+ filepath);

					Signature signature = Signature.Create (filepath, record.RecordClient);

					await record.RecordClient.HttpPostS3Async (filepath, signature);

					tokens.Remove ("filepath");
					tokens.Add ("guid", (string)signature ["guid"]);

					await createDataAsync (tokens);
					
				}

			} else {

				throw new VzaarApiException ();
			}

			return result;
		}
			
		//create with additiobal parameters
		public async static Task<Video> CreateAsync(Dictionary<string,object> tokens) {

			var video = new Video ();

			object urlvideo = await video.createDataAsync (tokens);
			if (urlvideo != null)
				return (Video)urlvideo;

			return video;
		}

		public async static Task<Video> CreateAsync(Dictionary<string,object> tokens, Client client) {

			var video = new Video (client);

			object urlvideo = await video.createDataAsync (tokens);
			if (urlvideo != null)
				return (Video)urlvideo;

			return video;
		}

		//create from file
		public async static Task<Video> CreateAsync(string filepath){

			var video = new Video ();

			var file = new Dictionary<string, object> ();
			file.Add ("filepath",filepath);
			await video.createDataAsync(file);

			return video;
		}

		public async static Task<Video> CreateAsync(string filepath, Client client){

			var video = new Video (client);

			var file = new Dictionary<string, object> ();
			file.Add ("filepath",filepath);
			await video.createDataAsync(file);

			return video;
		}

		//lookup
		public static Video Find(long id) {

			var video = new Video ();

			video.record.Read (id);

			return video;
		}

		public static  Video Find(long id, Client client) {

			var video = new Video (client);

			video.record.Read (id);

			return video;
		}

		//update
		public virtual void Save() {

			record.Update ();

		}

		public virtual void Save(Dictionary<string,object> tokens) {

			record.Update (tokens);

		}

		//delete
		public virtual void Delete() {

			record.Delete ();

		}

		//SubtitlesList get
		public virtual SubtitlesList Subtitles(){

			subtitlesList = new SubtitlesList((long)record["id"], record.RecordClient);
			return subtitlesList;
		} 

		//Subtitle create
		public virtual Subtitle SubtitleCreate(Dictionary<string,object> tokens){

			subtitle = new Subtitle ((long)record["id"],record.RecordClient);
			subtitle.Create(tokens);
			
			//refresh video object
			record.Read ((long)record["id"]);

			return subtitle;

		}

		//Subtitle update
		public virtual Subtitle SubtitleUpdate(long subtitleId, Dictionary<string,object> tokens){

			subtitle = new Subtitle ((long)record["id"], record.RecordClient, subtitleId);
			subtitle.Save(tokens);

			//refresh video object
			record.Read ((long)record["id"]);

			return subtitle;
			
		} 

		//Subtitle delete
		public virtual Subtitle SubtitleDelete(long subtitleId){

			subtitle = new Subtitle ((long)record["id"], record.RecordClient, subtitleId);
			subtitle.Delete();

			//refresh video object
			record.Read ((long)record["id"]);

			return subtitle;
		}

		//Set Image Frame
		public virtual void SetImageFrame(Dictionary<string,object> tokens){

			var videoId = (long)record["id"];

			if(tokens.ContainsKey("image")) {

				var filepath = tokens["image"].ToString();
				record.Create (tokens, "/"+videoId.ToString() +"/image", filepath);

			} else {

				record.Update (tokens, "/image");

			}
		}
	}
}

