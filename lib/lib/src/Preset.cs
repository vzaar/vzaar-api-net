using System;

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

		//lookup
		public static Preset Find(long id) {

			var preset = new Preset ();

			preset.record.Read (id);

			return preset;
		}

		public static Preset Find(long id, Client client) {

			var preset = new Preset (client);

			preset.record.Read (id);

			return preset;
		}
	}
}

