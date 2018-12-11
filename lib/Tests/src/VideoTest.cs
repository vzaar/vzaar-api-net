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
		public void SetImageFrame() {

			var video = Video.Find (7574853, new ClientMock(MockResponse.Video));

			Dictionary<string, object> tokens = new Dictionary<string, object> () {
				{ "time", 12.12 }
			};

			video.SetImageFrame(tokens);

			Assert.IsNotNull (video["poster_url"]);

		}

		[Test()]
		public void SetImageFrameFromFile() {

			var video = Video.Find (7574853, new ClientMock(MockResponse.Video));

			Dictionary<string, object> tokens = new Dictionary<string, object> () {
				{ "image", "../../src/Fixture/test.jpg"}
			};

			video.SetImageFrame(tokens);

			Assert.IsNotNull (video["poster_url"]);

		}

		[Test()]
		public void SetImageFrameFromFileException1() {

			var expected = "File does not exsist:";
			try {

				Video video = Video.Find (7574853, new ClientMock(MockResponse.Video));

				Dictionary<string, object> tokens = new Dictionary<string, object> () {
					{ "image", "unknown.jpg" }
				};

				video.SetImageFrame(tokens);

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
		public void SubtitleCreate() {

				var video = Video.Find (7574853, new ClientMock(MockResponse.Video));

				Dictionary<string, object> tokens = new Dictionary<string, object> () {
					{ "code", "en" },
					{ "content", "1\n00:02:17,440 --> 00:02:20,375\nSenator, we're making\nour final approach into Coruscant." }
				};

				Subtitle subtitle = video.SubtitleCreate(tokens);

				Assert.That (video.record.RecordClient, Is.SameAs (subtitle.record.RecordClient));

				Assert.AreEqual (subtitle.record.RecordEndpoint, "videos/7574853/subtitles");

				Assert.IsNotNull (subtitle["id"]);
				Assert.IsNotNull (subtitle["language"]);

		}

		[Test()]
		public void SubtitleCreateFromFile() {

				var video = Video.Find (7574853, new ClientMock(MockResponse.Video));

				Dictionary<string, object> tokens = new Dictionary<string, object> () {
					{ "code", "en" },
					{ "file", "../../src/Fixture/test.srt" }
				};

				Subtitle subtitle = video.SubtitleCreate(tokens);

				Assert.IsNotNull (subtitle["id"]);
				Assert.IsNotNull (subtitle["language"]);

		}

		[Test()]
		public void SubtitleUpdate() {

				Video video = Video.Find (7574853, new ClientMock(MockResponse.Video));

				Dictionary<string, object> tokens = new Dictionary<string, object> () {
					{ "code", "en" },
					{ "content", "1\n00:02:17,440 --> 00:02:20,375\nSenator, we're making\nour final approach into Coruscant." }
				};

				var subtitleId = 26548;

				var updatedAt = (((List<Dictionary<string,object>>)video["subtitles"])[0]["updated_at"]).ToString();

				Subtitle subtitle = video.SubtitleUpdate(subtitleId, tokens);

				Assert.That (video.record.RecordClient, Is.SameAs (subtitle.record.RecordClient));

				Assert.AreEqual (subtitle.record.RecordEndpoint, "videos/7574853/subtitles");
				Assert.AreEqual ((long)subtitle.record["id"], (long)subtitleId);

				var new_updatedAt = subtitle["updated_at"].ToString();
				Assert.AreNotEqual (updatedAt, new_updatedAt);

		}

		[Test()]
		public void SubtitleUpdateFromFile() {

				Video video = Video.Find (7574853, new ClientMock(MockResponse.Video));

				Dictionary<string, object> tokens = new Dictionary<string, object> () {
					{ "code", "en" },
					{ "file", "../../src/Fixture/test.srt" }
				};

				var subtitleId = 26548;

				var updatedAt = (((List<Dictionary<string,object>>)video["subtitles"])[0]["updated_at"]).ToString();

				Subtitle subtitle = video.SubtitleUpdate(subtitleId, tokens);

				var new_updatedAt = subtitle["updated_at"].ToString();
				Assert.AreNotEqual (updatedAt, new_updatedAt);

		}

		[Test()]
		public void SubtitleDelete() {

			Video video = Video.Find (7574853, new ClientMock(MockResponse.Video));

			var subtitleId = 26548;

			Subtitle subtitle = video.SubtitleDelete(subtitleId);

			var expected = "missing 'id'";
			try {

				var id = subtitle["id"];

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
		public void SubtitleCreateFromFileException1() {

			var expected = "File does not exsist:";
			try {

				Video video = Video.Find (7574853, new ClientMock(MockResponse.Video));

				Dictionary<string, object> tokens = new Dictionary<string, object> () {
					{ "code", "en" },
					{ "file", "unknown.srt" }
				};

				Subtitle subtitle = video.SubtitleCreate(tokens);

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
		public void SubtitleUpdateFromFileException1() {

			var expected = "File does not exsist:";
			try {

				Video video = Video.Find (7574853, new ClientMock(MockResponse.Video));

				Dictionary<string, object> tokens = new Dictionary<string, object> () {
					{ "code", "en" },
					{ "file", "unknown.srt" }
				};

				var subtitleId = 26548;

				Subtitle subtitle = video.SubtitleUpdate(subtitleId, tokens);

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
		
		[Test ()]
		public void SubtitlesPaginate ()
		{
			Video video = Video.Find (7574853, new ClientMock(MockResponse.Video));

			SubtitlesList list = video.Subtitles();

			Assert.AreEqual (list.records.RecordEndpoint, "videos/7574853/subtitles");

			foreach (var item in list.Page) {

				Assert.That (video.record.RecordClient, Is.SameAs (item.record.RecordClient));
				Assert.AreEqual (item.record.RecordEndpoint, "videos/7574853/subtitles");
			}

			var page = list.Paginate ();

			Assert.IsFalse (list.Next ());
			Assert.IsFalse (list.Prevous ());
			Assert.IsTrue (list.First ());
			Assert.IsTrue (list.Last ());

		}

		[Test()]
		public void SubtitlesEachItem ()
		{
			Video video = Video.Find (7574853, new ClientMock(MockResponse.Video));

			SubtitlesList list = video.Subtitles();

			foreach (Subtitle item in list.EachItem()) {

				Assert.That (video.record.RecordClient, Is.SameAs (item.record.RecordClient));
				Assert.AreEqual (item.record.RecordEndpoint, "videos/7574853/subtitles");
			}
		} 

		[Test()]
		public void SubtitlesToTypeDef() {

			var video = Video.Find (7574853, new ClientMock(MockResponse.Video));

			Dictionary<string, object> tokens = new Dictionary<string, object> () {
				{ "code", "en" },
				{ "file", "../../src/Fixture/test.srt" }
			};

			Subtitle subtitle = video.SubtitleCreate(tokens);

			var data = (SubtitleType)subtitle.ToTypeDef(typeof(SubtitleType));

			Assert.AreEqual (data.id,(long)subtitle["id"]);
			Assert.AreEqual (data.title,(string)subtitle["title"]);

		}

		[Test()]
		public void ToTypeDef() {

			var video = Video.Find (1, new ClientMock(MockResponse.Video));

			var data = (VideoType)video.ToTypeDef (typeof(VideoType));

			var count = data.subtitles.Count;

			Assert.AreNotEqual (count,0);
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

