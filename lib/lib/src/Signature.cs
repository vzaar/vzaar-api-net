using System.Collections.Generic;
using System.IO;

namespace VzaarApi
{
	public class Signature : BaseResource
	{
		//constructor
		public Signature()
			: this(Client.GetClient())
		{
		}

		public Signature(Client client)
			: base("signature", client)
		{
		}

		public object this[string index]
		{
			get => record[index];
		}

		//create from file
		public static Signature Create(string filepath)
		{
			return Create(filepath, Client.GetClient());
		}

		public static Signature Create(string filepath, Client client)
		{
			var signature = new Signature(client);

			signature.CreateFromFile(filepath);

			return signature;
		}

		public static Signature Single()
		{
			return Single(new Dictionary<string, object>(), Client.GetClient());
		}

		public static Signature Single(Client client)
		{
			return Single(new Dictionary<string, object>(), client);
		}

		public static Signature Single(Dictionary<string, object> tokens)
		{
			return Single(tokens, Client.GetClient());
		}

		public static Signature Single(Dictionary<string, object> tokens, Client client)
		{
			var signature = new Signature(client);

			signature.CreateSingle(tokens);

			return signature;
		}

		public static Signature Multipart(Dictionary<string, object> tokens)
		{
			return Multipart(tokens, Client.GetClient());
		}

		public static Signature Multipart(Dictionary<string, object> tokens, Client client)
		{
			var signature = new Signature(client);

			signature.CreateMultipart(tokens);

			return signature;
		}

		private void CreateSingle(Dictionary<string, object> tokens)
		{
			if (tokens.ContainsKey("uploader") == false)
				tokens.Add("uploader", Client.UPLOADER + Client.VERSION);

			string path = "/single/2";

			record.Create(tokens, path);
		}

		private void CreateMultipart(Dictionary<string, object> tokens)
		{
			if (tokens.ContainsKey("uploader") == false)
				tokens.Add("uploader", Client.UPLOADER + Client.VERSION);

			string path = "/multipart/2";

			record.Create(tokens, path);
		}

		private void CreateFromFile(string filepath)
		{
			FileInfo file = new FileInfo(filepath);

			if (file.Exists == false)
				throw new VzaarApiException("File does not exist: " + filepath);

			Dictionary<string, object> tokens = new Dictionary<string, object>();

			string filename = file.Name;
			long filesize = file.Length;

			tokens.Add("filename", filename);
			tokens.Add("filesize", filesize);

			if (filesize >= Client.MULTIPART_MIN_SIZE)
				CreateMultipart(tokens);
			else
				CreateSingle(tokens);
		}
	}
}

