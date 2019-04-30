using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;

namespace VzaarApi
{
	public abstract class BaseResourceCollection<TResource, TResourceCollection>
		where TResource : BaseResource
		where TResourceCollection : BaseResourceCollection<TResource, TResourceCollection>
	{
		internal RecordsList records;

		public List<TResource> Page { get; internal set; }

		protected BaseResourceCollection(string resourceEndpoint, Client client)
		{
			records = new RecordsList(resourceEndpoint, client);
			Page = new List<TResource>();
		}

		public Client GetClient()
		{
			return records.RecordClient;
		}

		//this method creates PagedItem objects from Records
		internal void Initialize()
		{
			Page.Clear();

			var flags = BindingFlags.NonPublic | BindingFlags.Instance;
			var culture = CultureInfo.InvariantCulture;
			var resourceType = typeof(TResource);

			foreach (var item in records.List)
			{
				var args = new object[] {item};
				var resource = (TResource)Activator.CreateInstance(resourceType, flags, null, args, culture);
				Page.Add(resource);
			}
		}

		//get list
		public static IEnumerable<TResource> EachItem()
		{
			return EachItem(new Dictionary<string, string>(), Client.GetClient());
		}

		public static IEnumerable<TResource> EachItem(Client client)
		{
			return EachItem(new Dictionary<string, string>(), client);
		}

		public static IEnumerable<TResource> EachItem(Dictionary<string, string> query)
		{
			return EachItem(query, Client.GetClient());
		}

		public static IEnumerable<TResource> EachItem(Dictionary<string, string> query, Client client)
		{
			return EachItemAsync(query, client).Result;
		}

		public static Task<IEnumerable<TResource>> EachItemAsync()
		{
			return EachItemAsync(new Dictionary<string, string>(), Client.GetClient());
		}

		public static Task<IEnumerable<TResource>> EachItemAsync(Client client)
		{
			return EachItemAsync(new Dictionary<string, string>(), client);
		}

		public static Task<IEnumerable<TResource>> EachEachItemAsyncItem(Dictionary<string, string> query)
		{
			return EachItemAsync(query, Client.GetClient());
		}

		public static async Task<IEnumerable<TResource>> EachItemAsync(Dictionary<string, string> query, Client client)
		{
			var resourceCollection = (TResourceCollection)Activator.CreateInstance(typeof(TResourceCollection), client);

			await resourceCollection.records.Read(query).ConfigureAwait(false);

			var resources = new List<TResource>();

			do
			{
				resourceCollection.Initialize();

				resources.AddRange(resourceCollection.Page);

			} while (await resourceCollection.records.Next().ConfigureAwait(false));

			return resources;
		}

		//paginate
		public static TResourceCollection Paginate()
		{
			return Paginate(new Dictionary<string, string>());
		}

		public static TResourceCollection Paginate(Client client)
		{
			return Paginate(new Dictionary<string, string>(), client);
		}

		public static TResourceCollection Paginate(Dictionary<string, string> query)
		{
			return Paginate(query, Client.GetClient());
		}

		public static TResourceCollection Paginate(Dictionary<string, string> query, Client client)
		{
			return PaginateAsync(query, client).Result;
		}

		public static Task<TResourceCollection> PaginateAsync()
		{
			return PaginateAsync(new Dictionary<string, string>());
		}

		public static Task<TResourceCollection> PaginateAsync(Client client)
		{
			return PaginateAsync(new Dictionary<string, string>(), client);
		}

		public static Task<TResourceCollection> PaginateAsync(Dictionary<string, string> query)
		{
			return PaginateAsync(query, Client.GetClient());
		}

		public static async Task<TResourceCollection> PaginateAsync(Dictionary<string, string> query, Client client)
		{
			var resourceCollection = (TResourceCollection)Activator.CreateInstance(typeof(TResourceCollection), client);

			await resourceCollection.records.Read(query).ConfigureAwait(false);
			resourceCollection.Initialize();

			return resourceCollection;
		}

		public virtual bool Next()
		{
			return NextAsync().Result;
		}

		public virtual bool Prevous()
		{
			return PreviousAsync().Result;
		}

		public virtual bool First()
		{
			return FirstAsync().Result;
		}

		public virtual bool Last()
		{
			return LastAsync().Result;
		}

		public virtual async Task<bool> NextAsync()
		{
			bool result = await records.Next().ConfigureAwait(false);

			if (result)
				Initialize();

			return result;
		}

		public virtual async Task<bool> PreviousAsync()
		{
			bool result = await records.Previous().ConfigureAwait(false);

			if (result)
				Initialize();

			return result;
		}

		public virtual async Task<bool> FirstAsync()
		{
			bool result = await records.First().ConfigureAwait(false);

			if (result)
				Initialize();

			return result;
		}

		public virtual async Task<bool> LastAsync()
		{
			bool result = await records.Last().ConfigureAwait(false);

			if (result)
				Initialize();

			return result;
		}
	}
}