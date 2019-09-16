using System;
using System.Threading.Tasks;

namespace VzaarApi
{
	public class Preset
	{
		internal Record record;

		//constructor
		public Preset ()
		{
			record = new Record ("encoding_presets");

		}

		public Preset (Client client)
		{
			record = new Record ("encoding_presets", client);
		}

		internal Preset (Record item)
		{
			record = item;
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

		//ASYNC METHODS

		//lookup
		public async static Task<Preset> FindAsync(long id) {

			var preset = new Preset ();

			await preset.record.ReadAsync (id).ConfigureAwait(false);

			return preset;
		}

		public async static Task<Preset> FindAsync(long id, Client client) {

			var preset = new Preset (client);

			await preset.record.ReadAsync (id).ConfigureAwait(false);

			return preset;
		}

		//SYNC METHODS

		//lookup
		public static Preset Find(long id) {
			return Preset.FindAsync (id).Result;
		}

		public static Preset Find(long id, Client client) {
			return Preset.FindAsync (id, client).Result;
		}
	}
}

