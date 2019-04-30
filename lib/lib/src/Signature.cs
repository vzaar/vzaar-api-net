using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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
			return CreateAsync(filepath, client).Result;
		}

		public static Task<Signature> CreateAsync(string filepath)
		{
			return CreateAsync(filepath, Client.GetClient());
		}

		public static async Task<Signature> CreateAsync(string filepath, Client client)
		{
			var signature = new Signature(client);

			await signature.CreateFromFile(filepath).ConfigureAwait(false);

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
			return SingleAsync(tokens, client).Result;
		}

		public static Task<Signature> SingleAsync()
		{
			return SingleAsync(new Dictionary<string, object>(), Client.GetClient());
		}

		public static Task<Signature> SingleAsync(Client client)
		{
			return SingleAsync(new Dictionary<string, object>(), client);
		}

		public static Task<Signature> SingleAsync(Dictionary<string, object> tokens)
		{
			return SingleAsync(tokens, Client.GetClient());
		}

		public static async Task<Signature> SingleAsync(Dictionary<string, object> tokens, Client client)
		{
			var signature = new Signature(client);

			await signature.CreateSingleAsync(tokens).ConfigureAwait(false);

			return signature;
		}

		public static Signature Multipart(Dictionary<string, object> tokens)
		{
			return Multipart(tokens, Client.GetClient());
		}

		public static Signature Multipart(Dictionary<string, object> tokens, Client client)
		{
			return MultipartAsync(tokens, client).Result;
		}

		public static Task<Signature> MultipartAsync(Dictionary<string, object> tokens)
		{
			return MultipartAsync(tokens, Client.GetClient());
		}

		public static async Task<Signature> MultipartAsync(Dictionary<string, object> tokens, Client client)
		{
			var signature = new Signature(client);

			await signature.CreateMultipartAsync(tokens).ConfigureAwait(false);

			return signature;
		}

		private async Task CreateSingleAsync(Dictionary<string, object> tokens)
		{
			if (tokens.ContainsKey("uploader") == false)
				tokens.Add("uploader", Client.UPLOADER + Client.VERSION);

			string path = "/single/2";

			await record.Create(tokens, path).ConfigureAwait(false);
		}

		private async Task CreateMultipartAsync(Dictionary<string, object> tokens)
		{
			if (tokens.ContainsKey("uploader") == false)
				tokens.Add("uploader", Client.UPLOADER + Client.VERSION);

			string path = "/multipart/2";

			await record.Create(tokens, path).ConfigureAwait(false);
		}

		private async Task CreateFromFile(string filepath)
		{
			var file = new FileInfo(filepath);
			if (!file.Exists)
				throw new VzaarApiException("File does not exist: " + filepath);

			var tokens = new Dictionary<string, object>();

			string filename = file.Name;
			long filesize = file.Length;

			tokens.Add("filename", filename);
			tokens.Add("filesize", filesize);

			if (filesize >= Client.MULTIPART_MIN_SIZE)
				await CreateMultipartAsync(tokens).ConfigureAwait(false);
			else
				await CreateSingleAsync(tokens).ConfigureAwait(false);
		}
	}
}

