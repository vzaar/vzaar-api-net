using NUnit.Framework;
using System;
using System.Collections.Generic;
using VzaarApi;

namespace tests
{
	[TestFixture ()]
	public class VideosListTest
	{
		[Test ()]
		public void NewVideosList ()
		{
			var list = new VideosList ();

			Assert.AreEqual (list.records.RecordEndpoint, "videos");
			Assert.IsInstanceOf<List<Record>> (list.records.List);
			Assert.IsInstanceOf<List<Video>> (list.Page);

			var client = new ClientMock (MockResponse.VideosList);
			list = new VideosList (client);

			Assert.AreEqual (list.records.RecordEndpoint, "videos");
			Assert.IsInstanceOf<List<Record>> (list.records.List);
			Assert.IsInstanceOf<List<Video>> (list.Page);

			Assert.IsInstanceOf<Client> (list.records.RecordClient);
		}

		[Test ()]
		public void Paginate ()
		{
			var list = VideosList.Paginate (new ClientMock(MockResponse.VideosList));

			var listClient = list.records.RecordClient;

			Assert.AreEqual (list.records.RecordEndpoint, "videos");

			foreach (var item in list.Page) {

				Assert.AreEqual (item.record.RecordEndpoint, "videos");
			}

			var per_page = "1";
			var query = new Dictionary<string,string>() {
				{"per_page",per_page}
			};

			var list2 = VideosList.Paginate (query, new ClientMock(MockResponse.VideosList));

			var countItem = 0;
			foreach (var item in list2.Page) {
				countItem++;
			}

			Assert.AreEqual (countItem.ToString(), per_page);

			list = VideosList.Paginate (new ClientMock (MockResponse.RecordPaginate));

			Assert.IsTrue (list.Next ());
			Assert.IsTrue (list.Prevous ());
			Assert.IsTrue (list.First ());
			Assert.IsTrue (list.Last ());

			list = VideosList.Paginate (new ClientMock (MockResponse.RecordPaginateEmpty));

			Assert.IsFalse (list.Next ());
			Assert.IsFalse (list.Prevous ());
			Assert.IsFalse (list.First ());
			Assert.IsFalse (list.Last ());

		}

		[Test()]
		public void EachItem ()
		{
			Client listClient = new ClientMock (MockResponse.VideosList);

			foreach (Video item in VideosList.EachItem (listClient)) {

				Assert.That (listClient, Is.SameAs (item.record.RecordClient));
				Assert.AreEqual (item.record.RecordEndpoint, "videos");

			}

			var per_page = "1";
			var query = new Dictionary<string,string>() {
				{"per_page",per_page}
			};

			int countItem = 0;
			foreach (Video item in VideosList.EachItem (query,listClient)) {

				countItem++;

			}

			Assert.AreEqual (countItem.ToString(), per_page);

		} 
	}
}

