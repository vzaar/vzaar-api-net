using System.Threading.Tasks;

namespace VzaarApi
{
	public class Preset : BaseResource
	{
		//constructor
		public Preset()
			: this(Client.GetClient())
		{
		}

		public Preset(Client client)
			: base("encoding_presets", client)
		{
		}

		/// <summary>
		/// Do not remove. This is required for use in BaseResourceCollection
		/// </summary>
		internal Preset(Record item)
			: base(item)
		{
			record = item;
		}

		public object this[string index]
		{
			get => record[index];
		}

		//lookup
		public static Preset Find(long id)
		{
			return Find(id, Client.GetClient());
		}

		public static Preset Find(long id, Client client)
		{
			return FindAsync(id, client).Result;
		}

		public static Task<Preset> FindAsync(long id)
		{
			return FindAsync(id, Client.GetClient());
		}

		public static async Task<Preset> FindAsync(long id, Client client)
		{
			var resource = new Preset(client);

			await resource.record.Read(id).ConfigureAwait(false);

			return resource;
		}
	}
}

