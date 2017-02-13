using NUnit.Framework;
using System;
using System.Collections.Generic;
using VzaarApi;

namespace tests
{
	[TestFixture ()]
	public class PresetsListTest
	{
		[Test ()]
		public void NewPresetsList ()
		{
			var list = new PresetsList ();

			Assert.AreEqual (list.records.RecordEndpoint, "encoding_presets");
			Assert.IsInstanceOf<List<Record>> (list.records.List);
			Assert.IsInstanceOf<List<Preset>> (list.Page);

			var client = new ClientMock (MockResponse.PresetsList);
			list = new PresetsList (client);

			Assert.AreEqual (list.records.RecordEndpoint, "encoding_presets");
			Assert.IsInstanceOf<List<Record>> (list.records.List);
			Assert.IsInstanceOf<List<Preset>> (list.Page);

			Assert.IsInstanceOf<Client> (list.records.RecordClient);
		}

		[Test ()]
		public void Paginate ()
		{
			var list = PresetsList.Paginate (new ClientMock(MockResponse.PresetsList));

			var listClient = list.records.RecordClient;

			Assert.AreEqual (list.records.RecordEndpoint, "encoding_presets");

			foreach (var item in list.Page) {

				Assert.AreEqual (item.record.RecordEndpoint, "encoding_presets");
			}

			var per_page = "1";
			var query = new Dictionary<string,string>() {
				{"per_page",per_page}
			};

			var list2 = PresetsList.Paginate (query, new ClientMock(MockResponse.PresetsList));

			var countItem = 0;
			foreach (var item in list2.Page) {
				countItem++;
			}

			Assert.AreEqual (countItem.ToString(), per_page);

			list = PresetsList.Paginate (new ClientMock (MockResponse.RecordPaginate));

			Assert.IsTrue (list.Next ());
			Assert.IsTrue (list.Prevous ());
			Assert.IsTrue (list.First ());
			Assert.IsTrue (list.Last ());

			list = PresetsList.Paginate (new ClientMock (MockResponse.RecordPaginateEmpty));

			Assert.IsFalse (list.Next ());
			Assert.IsFalse (list.Prevous ());
			Assert.IsFalse (list.First ());
			Assert.IsFalse (list.Last ());

		}

		[Test()]
		public void EachItem ()
		{
			Client listClient = new ClientMock (MockResponse.PresetsList);

			foreach (Preset item in PresetsList.EachItem (listClient)) {

				Assert.That (listClient, Is.SameAs (item.record.RecordClient));
				Assert.AreEqual (item.record.RecordEndpoint, "encoding_presets");

			}

			var per_page = "1";
			var query = new Dictionary<string,string>() {
				{"per_page",per_page}
			};

			int countItem = 0;
			foreach (Preset item in PresetsList.EachItem (query,listClient)) {

				countItem++;

			}

			Assert.AreEqual (countItem.ToString(), per_page);

		} 
	}
}

