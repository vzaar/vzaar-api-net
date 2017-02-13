using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VzaarApi;

namespace tests
{
	[TestFixture ()]
	public class VideoTest
	{
		[Test ()]
		public void NewVideo ()
		{
			var video = new Video ();

			Assert.AreEqual (video.record.RecordEndpoint, "videos");
			Assert.IsInstanceOf<Client> (video.record.RecordClient);

			var client = new ClientMock (MockResponse.Video);
			video = new Video (client);

			Assert.AreEqual (video.record.RecordEndpoint, "videos");
			Assert.IsInstanceOf<Record> (video.record);
			Assert.IsInstanceOf<Client> (video.record.RecordClient);
			Assert.That (video.record.RecordClient, Is.SameAs (client));

			var assignRecord = new Video (video.record);
			Assert.That (assignRecord.record, Is.SameAs (video.record));
		}

		[Test()]
		public void GetClient() {

			var video = new Video ();

			Assert.That (video.GetClient (), Is.SameAs (video.record.RecordClient));
		}

		[Test()]
		public void Find() {

			var video = Video.Find (1, new ClientMock(MockResponse.Video));

			Assert.AreEqual (0, video.record.Parameters.Count);
			Assert.IsFalse (video.record.Edited);
			Assert.AreEqual (0, video.record.cache.Count);

			Assert.IsNotNull (video["id"]);
			Assert.IsNotNull (video ["title"]);
		}

		[Test()]
		public void CreateFromTokens1() {

			var client = new S3ClientMock (MockResponse.Video);
			Assert.That (client.TestCreate_5mb, Throws.Nothing);

			Dictionary<string, object> tokens = new Dictionary<string, object> () {
				{ "title", "Example video" },
				{"filepath", "../../src/Fixture/movie-5mb.mp4"}
			};

			var task = Video.CreateAsync (tokens, new S3ClientMock(MockResponse.Video));
			task.Wait ();

			Video video = task.Result;

			Assert.AreEqual (0, video.record.Parameters.Count);
			Assert.IsFalse (video.record.Edited);
			Assert.AreEqual (0, video.record.cache.Count);

			Assert.IsNotNull (video["id"]);
			Assert.IsNotNull (video ["title"]);
		}

		[Test()]
		public void CreateFromTokens2() {

			var client = new S3ClientMock (MockResponse.Video);
			Assert.That (client.TestCreate_1mb, Throws.Nothing);

			Dictionary<string, object> tokens = new Dictionary<string, object> () {
				{ "title", "Example video" },
				{"filepath", "../../src/Fixture/movie-1mb.mp4"}
			};

			var task = Video.CreateAsync (tokens, new S3ClientMock(MockResponse.Video));
			task.Wait ();

			Video video = task.Result;

			Assert.AreEqual (0, video.record.Parameters.Count);
			Assert.IsFalse (video.record.Edited);
			Assert.AreEqual (0, video.record.cache.Count);

			Assert.IsNotNull (video["id"]);
			Assert.IsNotNull (video ["title"]);
		}

		[Test()]
		public void CreateFromTokens3() {

			Dictionary<string, object> tokens = new Dictionary<string, object> () {
				{ "title", "Example video" },
				{ "url", "https://example.com/video-unit.mp4"}
			};

			var task = Video.CreateAsync (tokens, new S3ClientMock(MockResponse.Video));
			task.Wait ();

			Video video = task.Result;

			Assert.IsNotNull (video["id"]);
			Assert.IsNotNull (video ["title"]);
		}

		[Test()]
		public void CreateFromTokens4() {

			Dictionary<string, object> tokens = new Dictionary<string, object> () {
				{ "title", "Example video" },
				{ "guid", "vz91e80db09a494467b265f0c327950825"}
			};

			var task = Video.CreateAsync (tokens, new S3ClientMock(MockResponse.Video));
			task.Wait ();

			Video video = task.Result;

			Assert.IsNotNull (video["id"]);
			Assert.IsNotNull (video ["title"]);
		}

		[Test()]
		public void Save() {

			Dictionary<string, object> tokens = new Dictionary<string, object> () {
				{ "title", "Example video" },
			};

			var video = Video.Find (1, new ClientMock(MockResponse.Video));

			video.Save (tokens);

			Assert.AreEqual (0, video.record.Parameters.Count);
			Assert.IsFalse (video.record.Edited);
			Assert.AreEqual (0, video.record.cache.Count);

			Assert.IsNotNull (video["id"]);
			Assert.IsNotNull (video ["title"]);

			video ["private"] = true;

			Assert.IsTrue (video.Edited);

			Assert.AreEqual(true, (bool)video["private"]);

			video ["private"] = null;

			Assert.IsFalse (video.Edited);

			video ["private"] = true;

			video.Save ();

			Assert.IsFalse (video.Edited);

			Assert.AreEqual (0, video.record.Parameters.Count);
			Assert.IsFalse (video.record.Edited);
			Assert.AreEqual (0, video.record.cache.Count);

			Assert.IsNotNull (video["id"]);
			Assert.IsNotNull (video ["title"]);
		}

		[Test()]
		public void Delete() {

			var video = Video.Find (1, new ClientMock(MockResponse.Video));

			video["title"] = "test name";

			Assert.IsNotNull (video["id"]);
			Assert.IsNotNull (video ["title"]);

			Assert.IsTrue (video.Edited);
			Assert.AreNotEqual (0, video.record.cache.Count);

			video.Delete ();

			Assert.IsNull (video ["title"]);

			Assert.IsFalse (video.Edited);
			Assert.AreEqual (0, video.record.cache.Count);

			var expected = "missing 'id'";
			try {

				var id = video["id"];

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
		public void ToTypeDef() {

			var video = Video.Find (1, new ClientMock(MockResponse.Video));

			var data = (VideoType)video.ToTypeDef (typeof(VideoType));

			Assert.AreEqual (data.id,(long)video["id"]);
			Assert.AreEqual (data.title,(string)video["title"]);

		}

		[Test()]
		public void CreateException1() {

			var expected = "StatusCode: ";
			try {

				Dictionary<string, object> tokens = new Dictionary<string, object> () {
					{ "title", "Example video" },
					{ "filepath", "../../src/Fixture/movie-1mb.mp4" }
				};

				var task = Video.CreateAsync (tokens, new S3ClientMock (MockResponse.UploadFailed));
				task.Wait ();

				//if the exception is not thrown, the below assert fails
				bool assert = true;
				Assert.IsFalse(assert);
			
			} catch (Exception e) {

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
		public void CreateException2() {

			var expected = "File Upload: not valid 'part_size_in_bytes'";
			try {

				Dictionary<string, object> tokens = new Dictionary<string, object> () {
					{ "title", "Example video" },
					{"filepath", "../../src/Fixture/movie-5mb.mp4"}
				};

				var task = Video.CreateAsync (tokens, new S3ClientMock(MockResponse.SignatureFailed));
				task.Wait ();

				//if the exception is not thrown, the below assert fails
				bool assert = true;
				Assert.IsFalse(assert);

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
		public void CreateException4() {

			var expected = "guid or url or filepath expected";
			try {

				Dictionary<string, object> tokens = new Dictionary<string, object> () {
					{ "title", "Example video" },
					{ "filepath", "../../src/Fixture/movie-5mb.mp4"},
					{ "guid", "vz91e80db09a494467b265f0c327950825"},
					{ "url", "https://example.com/video-unit.mp4"}
				};

				var task = Video.CreateAsync (tokens, new S3ClientMock(MockResponse.Video));
				task.Wait ();

				//if the exception is not thrown, the below assert fails
				bool assert = true;
				Assert.IsFalse(assert);

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

	}
}

