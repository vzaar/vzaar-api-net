using NUnit.Framework;
using System;
using System.Collections.Generic;
using VzaarApi;

namespace tests
{
	[TestFixture ()]
	public class PlaylistsListTest
	{
		[Test ()]
		public void NewPlaylistsList ()
		{
			var list = new PlaylistsList ();

			Assert.AreEqual (list.records.RecordEndpoint, "feeds/playlists");
			Assert.IsInstanceOf<List<Record>> (list.records.List);
			Assert.IsInstanceOf<List<Playlist>> (list.Page);

			var client = new ClientMock (MockResponse.PlaylistsList);
			list = new PlaylistsList (client);

			Assert.AreEqual (list.records.RecordEndpoint, "feeds/playlists");
			Assert.IsInstanceOf<List<Record>> (list.records.List);
			Assert.IsInstanceOf<List<Playlist>> (list.Page);

			Assert.IsInstanceOf<Client> (list.records.RecordClient);
		}

		[Test ()]
		public void Paginate ()
		{
			var list = PlaylistsList.Paginate (new ClientMock(MockResponse.PlaylistsList));

			var listClient = list.records.RecordClient;

			Assert.AreEqual (list.records.RecordEndpoint, "feeds/playlists");

			foreach (var item in list.Page) {

				Assert.AreEqual (item.record.RecordEndpoint, "feeds/playlists");
			}

			var per_page = "1";
			var query = new Dictionary<string,string>() {
				{"per_page",per_page}
			};

			var list2 = PlaylistsList.Paginate (query, new ClientMock(MockResponse.PlaylistsList));

			var countItem = 0;
			foreach (var item in list2.Page) {
				countItem++;
			}

			Assert.AreEqual (countItem.ToString(), per_page);

			list = PlaylistsList.Paginate (new ClientMock (MockResponse.RecordPaginate));

			Assert.IsTrue (list.Next ());
			Assert.IsTrue (list.Prevous ());
			Assert.IsTrue (list.First ());
			Assert.IsTrue (list.Last ());

			list = PlaylistsList.Paginate (new ClientMock (MockResponse.RecordPaginateEmpty));

			Assert.IsFalse (list.Next ());
			Assert.IsFalse (list.Prevous ());
			Assert.IsFalse (list.First ());
			Assert.IsFalse (list.Last ());

		}

		[Test()]
		public void EachItem ()
		{
			Client listClient = new ClientMock (MockResponse.PlaylistsList);

			foreach (Playlist item in PlaylistsList.EachItem (listClient)) {

				Assert.That (listClient, Is.SameAs (item.record.RecordClient));
				Assert.AreEqual (item.record.RecordEndpoint, "feeds/playlists");

			}

			var per_page = "1";
			var query = new Dictionary<string,string>() {
				{"per_page",per_page}
			};

			int countItem = 0;
			foreach (Playlist item in PlaylistsList.EachItem (query,listClient)) {

				countItem++;

			}

			Assert.AreEqual (countItem.ToString(), per_page);

		}// end 
	}
}

