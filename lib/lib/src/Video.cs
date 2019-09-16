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
					
					await record.CreateAsync (tokens).ConfigureAwait(false);

				} else if (containsurl == true) {
					
					result = await LinkUpload.CreateAsync (tokens,record.RecordClient).ConfigureAwait(false);

				} else {

					string filepath = (string)tokens ["filepath"];

					FileInfo file = new FileInfo (filepath);

					if(file.Exists == false)
						throw new VzaarApiException("File does not exist: "+ filepath);

					Signature signature = await Signature.CreateAsync (filepath, record.RecordClient).ConfigureAwait(false);

					await record.RecordClient.HttpPostS3Async (filepath, signature).ConfigureAwait(false);

					tokens.Remove ("filepath");
					tokens.Add ("guid", (string)signature ["guid"]);

					await createDataAsync (tokens).ConfigureAwait(false);
					
				}

			} else {

				throw new VzaarApiException ();
			}

			return result;
		}

		//ASYNC METHODS
			
		//create with additiobal parameters
		public async static Task<Video> CreateAsync(Dictionary<string,object> tokens) {

			var video = new Video ();

			object urlvideo = await video.createDataAsync (tokens).ConfigureAwait(false);
			if (urlvideo != null)
				return (Video)urlvideo;

			return video;
		}

		public async static Task<Video> CreateAsync(Dictionary<string,object> tokens, Client client) {

			var video = new Video (client);

			object urlvideo = await video.createDataAsync (tokens).ConfigureAwait(false);
			if (urlvideo != null)
				return (Video)urlvideo;

			return video;
		}

		//create from file
		public async static Task<Video> CreateAsync(string filepath){

			var video = new Video ();

			var file = new Dictionary<string, object> ();
			file.Add ("filepath",filepath);
			await video.createDataAsync(file).ConfigureAwait(false);

			return video;
		}

		public async static Task<Video> CreateAsync(string filepath, Client client){

			var video = new Video (client);

			var file = new Dictionary<string, object> ();
			file.Add ("filepath",filepath);
			await video.createDataAsync(file).ConfigureAwait(false);

			return video;
		}

		//lookup
		public async static Task<Video> FindAsync(long id) {

			var video = new Video ();

			await video.record.ReadAsync (id).ConfigureAwait(false);

			return video;
		}

		public async static Task<Video> FindAsync(long id, Client client) {

			var video = new Video (client);

			await video.record.ReadAsync (id).ConfigureAwait(false);

			return video;
		}

		//update
		public async Task SaveAsync() {

			await record.UpdateAsync ().ConfigureAwait(false);

		}

		public async Task SaveAsync(Dictionary<string,object> tokens) {

			await record.UpdateAsync (tokens).ConfigureAwait(false);

		}

		//delete
		public async Task DeleteAsync() {

			await record.DeleteAsync ().ConfigureAwait(false);

		}

		//Subtitle create
		public async Task<Subtitle> SubtitleCreateAsync(Dictionary<string,object> tokens){

			subtitle = new Subtitle ((long)record["id"],record.RecordClient);
			await subtitle.CreateAsync(tokens).ConfigureAwait(false);

			//refresh video object
			await record.ReadAsync ((long)record["id"]).ConfigureAwait(false);

			return subtitle;

		}

		//Subtitle update
		public async Task<Subtitle> SubtitleUpdateAsync(long subtitleId, Dictionary<string,object> tokens){

			subtitle = new Subtitle ((long)record["id"], record.RecordClient, subtitleId);
			await subtitle.SaveAsync(tokens).ConfigureAwait(false);

			//refresh video object
			await record.ReadAsync ((long)record["id"]).ConfigureAwait(false);

			return subtitle;

		} 

		//Subtitle delete
		public async Task<Subtitle> SubtitleDeleteAsync(long subtitleId){

			subtitle = new Subtitle ((long)record["id"], record.RecordClient, subtitleId);
			await subtitle.DeleteAsync().ConfigureAwait(false);

			//refresh video object
			await record.ReadAsync ((long)record["id"]).ConfigureAwait(false);

			return subtitle;
		}

		//Set Image Frame
		public async Task SetImageFrameAsync(Dictionary<string,object> tokens){

			var videoId = (long)record["id"];

			if(tokens.ContainsKey("image")) {

				var filepath = tokens["image"].ToString();
				await record.CreateAsync (tokens, "/"+videoId.ToString() +"/image", filepath).ConfigureAwait(false);

			} else {

				await record.UpdateAsync (tokens, "/image").ConfigureAwait(false);

			}
		}

		//SYNC METHODS

		//lookup
		public static Video Find(long id) {
			return Video.FindAsync (id).Result;
		}

		public static  Video Find(long id, Client client) {
			return Video.FindAsync (id, client).Result;
		}

		//update
		public void Save() {
			SaveAsync().Wait();
		}

		public void Save(Dictionary<string,object> tokens) {
			SaveAsync (tokens).Wait();
		}

		//delete
		public void Delete() {
			DeleteAsync ().Wait();
		}

		//SubtitlesList get
		public virtual SubtitlesList Subtitles(){

			subtitlesList = new SubtitlesList((long)record["id"], record.RecordClient);
			return subtitlesList;
		} 

		//Subtitle create
		public virtual Subtitle SubtitleCreate(Dictionary<string,object> tokens){
			return SubtitleCreateAsync (tokens).Result;
		}

		//Subtitle update
		public Subtitle SubtitleUpdate(long subtitleId, Dictionary<string,object> tokens){
			return SubtitleUpdateAsync (subtitleId, tokens).Result;
		} 

		//Subtitle delete
		public Subtitle SubtitleDelete(long subtitleId){
			return SubtitleDeleteAsync (subtitleId).Result;
		}

		//Set Image Frame
		public void SetImageFrame(Dictionary<string,object> tokens){
			SetImageFrameAsync (tokens).Wait ();
		}
	}
}

