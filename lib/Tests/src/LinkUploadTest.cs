using NUnit.Framework;
using System;
using System.Collections.Generic;
using VzaarApi;

namespace tests
{
	[TestFixture ()]
	public class LinkUploadTest
	{
		[Test ()]
		public void NewLinkUpload ()
		{
			var linkup = new LinkUpload ();

			Assert.AreEqual (linkup.record.RecordEndpoint, "link_uploads");
			Assert.IsInstanceOf<Client> (linkup.record.RecordClient);

			var client = new ClientMock (MockResponse.Video);
			linkup = new LinkUpload (client);

			Assert.AreEqual (linkup.record.RecordEndpoint, "link_uploads");
			Assert.IsInstanceOf<Record> (linkup.record);
			Assert.IsInstanceOf<Client> (linkup.record.RecordClient);
			Assert.That (linkup.record.RecordClient, Is.SameAs (client));

		}

		[Test()]
		public void Create() {

			var link = "exampl.com/sample.mp4";

			var client = new ClientMock (MockResponse.Video);
			Video video = LinkUpload.Create (link, client);

			Assert.That (video.record.RecordClient, Is.SameAs (client));

			var tokens = new Dictionary<string,object> () {
				{"url", link},
				{"uploader", "unit link uploader"}
			};

			var video2 = LinkUpload.Create (tokens, client);
			Assert.IsInstanceOf<Video> (video2);
		}
	}
}

