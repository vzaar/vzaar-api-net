using System;
using System.Collections.Generic;
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

		internal void CreateSingle(Dictionary<string,object> tokens) {

			if (tokens.ContainsKey ("uploader") == false) {
				tokens.Add ("uploader", Client.UPLOADER + Client.VERSION);
			}

			string path = "/single";

			record.Create (tokens, path);

		}

		internal void CreateSingle() {

			CreateSingle (new Dictionary<string, object> ());
		
		}

		internal void CreateMultipart(Dictionary<string,object> tokens) {

			if (tokens.ContainsKey ("uploader") == false) {
				tokens.Add ("uploader", Client.UPLOADER + Client.VERSION);
			}

			string path = "/multipart";

			record.Create (tokens, path);
		}

		internal void CreateFromFile(string filepath) {

			FileInfo file = new FileInfo (filepath);

			if(file.Exists == false)
				throw new VzaarApiException("File does not exist: "+filepath);

			Dictionary<string, object > tokens = new Dictionary<string, object> ();

			string filename = file.Name;
			long filesize = file.Length;

			tokens.Add ("filename", filename);
			tokens.Add ("filesize", filesize);

			if (filesize >= Client.MULTIPART_MIN_SIZE) {
				CreateMultipart (tokens);	
			} else {
				CreateSingle (tokens);
			}
		}

		//create from file
		public static Signature Create(string filepath) {

			var signature = new Signature ();

			signature.CreateFromFile (filepath);

			return signature;
		}

		public static Signature Create(string filepath, Client client) {

			var signature = new Signature (client);

			signature.CreateFromFile (filepath);

			return signature;
		}

		public static Signature Single(){

			var signature = new Signature ();

			signature.CreateSingle ();

			return signature;
		}

		public static Signature Single(Client client){

			var signature = new Signature (client);

			signature.CreateSingle ();

			return signature;
		}

		public static Signature Single(Dictionary<string,object> tokens){

			var signature = new Signature ();

			signature.CreateSingle (tokens);

			return signature;
		}

		public static Signature Single(Dictionary<string,object> tokens, Client client){
			
			var signature = new Signature (client);

			signature.CreateSingle (tokens);

			return signature;
		}

		public static Signature Multipart(Dictionary<string,object> tokens){

			var signature = new Signature ();

			signature.CreateMultipart (tokens);

			return signature;
		}

		public static Signature Multipart(Dictionary<string,object> tokens, Client client){

			var signature = new Signature (client);

			signature.CreateMultipart (tokens);

			return signature;
		}
	}
}

