using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace VzaarApi
{
	public class Signature
	{
		internal Record record;

		//constructor
		public Signature ()
		{
			record = new Record ("signature");

		}

		public Signature (Client client)
		{
			record = new Record ("signature", client);
		}

		public Client GetClient() {
			return record.RecordClient;
		}

		public object this[string index]{

			get { return record [index];}

		}

		public object ToTypeDef(Type type){

			return record.ToTypeDef (type);

		}

		internal async Task CreateSingleAsync(Dictionary<string,object> tokens) {

			if (tokens.ContainsKey ("uploader") == false) {
				tokens.Add ("uploader", Client.UPLOADER + Client.VERSION);
			}

			string path = "/single/2";

			await record.CreateAsync (tokens, path).ConfigureAwait(false);

		}

		internal async Task CreateSingleAsync() {

			await CreateSingleAsync (new Dictionary<string, object> ()).ConfigureAwait(false);
		
		}

		internal async Task CreateMultipartAsync(Dictionary<string,object> tokens) {

			if (tokens.ContainsKey ("uploader") == false) {
				tokens.Add ("uploader", Client.UPLOADER + Client.VERSION);
			}

			string path = "/multipart/2";

			await record.CreateAsync (tokens, path).ConfigureAwait(false);
		}

		internal async Task CreateFromFileAsync(string filepath) {

			FileInfo file = new FileInfo (filepath);

			if(file.Exists == false)
				throw new VzaarApiException("File does not exist: "+filepath);

			Dictionary<string, object > tokens = new Dictionary<string, object> ();

			string filename = file.Name;
			long filesize = file.Length;

			tokens.Add ("filename", filename);
			tokens.Add ("filesize", filesize);

			if (filesize >= Client.MULTIPART_MIN_SIZE) {
				await CreateMultipartAsync (tokens).ConfigureAwait(false);	
			} else {
				await CreateSingleAsync (tokens).ConfigureAwait(false);
			}
		}
			
		//create from file async
		public async static Task<Signature> CreateAsync(string filepath) {

			var signature = new Signature ();

			await signature.CreateFromFileAsync (filepath).ConfigureAwait(false);

			return signature;
		}

		public async static Task<Signature> CreateAsync(string filepath, Client client) {

			var signature = new Signature (client);

			await signature.CreateFromFileAsync (filepath).ConfigureAwait(false);

			return signature;
		}

		public async static Task<Signature> SingleAsync(){

			var signature = new Signature ();

			await signature.CreateSingleAsync ().ConfigureAwait(false);

			return signature;
		}

		public async static Task<Signature> SingleAsync(Client client){

			var signature = new Signature (client);

			await signature.CreateSingleAsync ().ConfigureAwait(false);

			return signature;
		}

		public async static Task<Signature> SingleAsync(Dictionary<string,object> tokens){

			var signature = new Signature ();

			await signature.CreateSingleAsync (tokens).ConfigureAwait(false);

			return signature;
		}

		public async static Task<Signature> SingleAsync(Dictionary<string,object> tokens, Client client){

			var signature = new Signature (client);

			await signature.CreateSingleAsync (tokens).ConfigureAwait(false);

			return signature;
		}

		public async static Task<Signature> MultipartAsync(Dictionary<string,object> tokens){

			var signature = new Signature ();

			await signature.CreateMultipartAsync (tokens).ConfigureAwait(false);

			return signature;
		}

		public async static Task<Signature> MultipartAsync(Dictionary<string,object> tokens, Client client){

			var signature = new Signature (client);

			await signature.CreateMultipartAsync (tokens).ConfigureAwait(false);

			return signature;
		}

		//create from file sync
		public static Signature Create(string filepath) {

			var signature = new Signature ();

			var task = signature.CreateFromFileAsync (filepath);
			task.Wait ();

			return signature;
		}

		public static Signature Create(string filepath, Client client) {

			var signature = new Signature (client);

			var task = signature.CreateFromFileAsync (filepath);
			task.Wait ();

			return signature;
		}

		public static Signature Single(){

			var signature = new Signature ();

			var task = signature.CreateSingleAsync ();
			task.Wait ();

			return signature;
		}

		public static Signature Single(Client client){

			var signature = new Signature (client);

			var task = signature.CreateSingleAsync ();
			task.Wait ();

			return signature;
		}

		public static Signature Single(Dictionary<string,object> tokens){

			var signature = new Signature ();

			var task = signature.CreateSingleAsync (tokens);
			task.Wait ();

			return signature;
		}

		public static Signature Single(Dictionary<string,object> tokens, Client client){

			var signature = new Signature (client);

			var task = signature.CreateSingleAsync (tokens);
			task.Wait ();

			return signature;
		}

		public static Signature Multipart(Dictionary<string,object> tokens){

			var signature = new Signature ();

			var task = signature.CreateMultipartAsync (tokens);
			task.Wait ();

			return signature;
		}

		public static Signature Multipart(Dictionary<string,object> tokens, Client client){

			var signature = new Signature (client);

			var task = signature.CreateMultipartAsync (tokens);
			task.Wait ();

			return signature;
		}
	}
}

