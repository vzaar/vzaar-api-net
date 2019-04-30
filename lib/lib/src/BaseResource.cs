using System;

namespace VzaarApi
{
	public abstract class BaseResource
	{
		internal Record record;

		//constructor
		protected BaseResource(string resourceEndpoint, Client client)
			: this(new Record(resourceEndpoint, client))
		{
		}

		internal BaseResource(Record item)
		{
			record = item;
		}

		public Client GetClient()
		{
			return record.RecordClient;
		}

		public object ToTypeDef(Type type)
		{
			return record.ToTypeDef(type);
		}
	}
}