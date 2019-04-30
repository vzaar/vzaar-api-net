using NUnit.Framework;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using VzaarApi;

namespace tests
{
	[TestFixture ()]
	public class PlaylistTest
	{
		[Test ()]
		public void NewPlaylist ()
		{
			var playlist = new Playlist ();

			Assert.AreEqual (playlist.record.RecordEndpoint, "feeds/playlists");
			Assert.IsInstanceOf<Client> (playlist.record.RecordClient);

			var client = new ClientMock (MockResponse.Playlist);
			playlist = new Playlist (client);

			Assert.AreEqual (playlist.record.RecordEndpoint, "feeds/playlists");
			Assert.IsInstanceOf<Record> (playlist.record);
			Assert.IsInstanceOf<Client> (playlist.record.RecordClient);
			Assert.That (playlist.record.RecordClient, Is.SameAs (client));
		}

		[Test()]
		public void GetClient() {

			var playlist = new Playlist ();

			Assert.That (playlist.GetClient (), Is.SameAs (playlist.record.RecordClient));
		}

		[Test()]
		public void RateLimits() {

			var playlist = Playlist.Find (1, new ClientMock (MockResponse.Playlist));

			var client = playlist.GetClient ();

			Assert.AreEqual ("222",client.RateLimit);
			Assert.AreEqual ("122",client.RateRemaining);
			Assert.AreEqual ("33333",client.RateReset);
		}

		[Test()]
		public void Find() {

			var playlist = Playlist.Find (1, new ClientMock(MockResponse.Playlist));

			Assert.AreEqual (0, playlist.record.Parameters.Count);
			Assert.IsFalse (playlist.record.Edited);
			Assert.AreEqual (0, playlist.record.cache.Count);

			Assert.IsNotNull (playlist["id"]);
			Assert.IsNotNull (playlist ["title"]);
			Assert.IsNotNull (playlist ["category_id"]);
		}

		[Test()]
		public void Create() {

			Dictionary<string, object> tokens = new Dictionary<string, object> () {
				{ "title", "Example Playlist" },
				{ "category_id", 42}
			};

			var playlist = Playlist.Create (tokens, new ClientMock(MockResponse.Playlist));

			Assert.AreEqual (0, playlist.record.Parameters.Count);
			Assert.IsFalse (playlist.record.Edited);
			Assert.AreEqual (0, playlist.record.cache.Count);

			Assert.IsNotNull (playlist["id"]);
			Assert.IsNotNull (playlist ["title"]);
			Assert.IsNotNull (playlist ["category_id"]);
		}

		[Test()]
		public void Save() {

			Dictionary<string, object> tokens = new Dictionary<string, object> () {
				{ "title", "Example Playlist" },
			};

			var playlist = Playlist.Find (1, new ClientMock(MockResponse.Playlist));

			playlist.Save (tokens);

			Assert.AreEqual (0, playlist.record.Parameters.Count);
			Assert.IsFalse (playlist.record.Edited);
			Assert.AreEqual (0, playlist.record.cache.Count);

			Assert.IsNotNull (playlist["id"]);
			Assert.IsNotNull (playlist ["title"]);
			Assert.IsNotNull (playlist ["category_id"]);

			playlist ["autoplay"] = true;

			Assert.IsTrue (playlist.Edited);

			Assert.AreEqual(true, (bool)playlist["autoplay"]);

			playlist ["autoplay"] = null;

			Assert.IsFalse (playlist.Edited);

			playlist ["autoplay"] = true;

			playlist.Save ();

			Assert.IsFalse (playlist.Edited);

			Assert.AreEqual (0, playlist.record.Parameters.Count);
			Assert.IsFalse (playlist.record.Edited);
			Assert.AreEqual (0, playlist.record.cache.Count);

			Assert.IsNotNull (playlist["id"]);
			Assert.IsNotNull (playlist ["title"]);
		}

		[Test()]
		public void Delete() {

			var playlist = Playlist.Find (1, new ClientMock(MockResponse.Playlist));

			playlist["title"] = "Test Playlist Title";

			Assert.IsNotNull (playlist["id"]);
			Assert.IsNotNull (playlist ["title"]);

			Assert.IsTrue (playlist.Edited);
			Assert.AreNotEqual (0, playlist.record.cache.Count);

			playlist.Delete ();

			Assert.IsNull (playlist ["title"]);

			Assert.IsFalse (playlist.Edited);
			Assert.AreEqual (0, playlist.record.cache.Count);

			var expected = "missing 'id'";
			try {

				var id = playlist["id"];

				//if the exception is not thrown, the below assert fails
				bool assert = true;
				Assert.IsFalse(assert);

			} catch(VzaarApiException ve) {

				StringAssert.Contains (expected,ve.Message);

			} catch (Exception e){

				if (e is AggregateException) {
					AggregateException ae = (AggregateException)e;

					var flatten = ae.Flatten ();

					foreach (var fe in flatten.InnerExceptions) {
						if (fe is VzaarApiException)
							StringAssert.Contains (expected, fe.Message);
					}
				}
			}


		}

		[Test()]
		public void IndexerGet() {

			var playlist = Playlist.Find (1, new ClientMock (MockResponse.Playlist));

			var id = (long)playlist ["id"];
			var title = (string)playlist ["title"];

			Assert.AreEqual (id,(long)playlist.record.Data["data"]["id"]);
			Assert.AreEqual (title,(string)playlist.record.Data["data"]["title"]);

		}

		[Test()]
		public void IndexerSet() {

			var playlist = Playlist.Find (1, new ClientMock (MockResponse.Playlist));

			object value;
			Assert.AreEqual (0, playlist.record.Parameters.Count);
			Assert.IsFalse(playlist.record.Parameters.TryGetValue("autoplay",out value));

			playlist ["autoplay"] = true;

			JToken token;
			Assert.IsFalse(playlist.record.Data.TryGetValue("autoplay",out token));
			Assert.IsTrue ((bool)playlist["autoplay"]);

			Assert.AreEqual (1, playlist.record.Parameters.Count);
			Assert.IsTrue(playlist.record.Parameters.TryGetValue("autoplay",out value));
			Assert.AreEqual (true, (bool)value);

			playlist ["autoplay"] = false;

			Assert.IsFalse(playlist.record.Data.TryGetValue("autoplay",out token));
			Assert.IsFalse ((bool)playlist["autoplay"]);

			Assert.AreEqual (1, playlist.record.Parameters.Count);
			Assert.IsTrue(playlist.record.Parameters.TryGetValue("autoplay",out value));
			Assert.AreEqual (false, (bool)value);

			Assert.IsTrue (playlist.Edited);

			playlist ["autoplay"] = null;

			Assert.IsFalse(playlist.record.Data.TryGetValue("autoplay",out token));
			Assert.IsFalse ((bool)playlist["autoplay"]);

			Assert.AreEqual (0, playlist.record.Parameters.Count);
			Assert.IsFalse(playlist.record.Parameters.TryGetValue("autoplay",out value));

			Assert.IsFalse (playlist.Edited);
		}

		[Test()]
		public void ToTypeDef() {

			var playlist = Playlist.Find (1, new ClientMock(MockResponse.Playlist));

			var data = (PlaylistType)playlist.ToTypeDef (typeof(PlaylistType));

			Assert.AreEqual (data.id,(long)playlist.record.Data["data"]["id"]);
			Assert.AreEqual (data.title,(string)playlist.record.Data["data"]["title"]);

		}
			

	}//end class
}//end namespace

