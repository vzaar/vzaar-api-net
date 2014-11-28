using System;
using com.vzaar.api;

namespace VzaarApi {
	class TestSuite {
    private Vzaar api;
    private int total = 0;
    private int failures = 0;

    private readonly Random _rng = new Random();
    private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public int videoId;
    public string username;
    public string token;
    public string apiUrl;
   
    public TestSuite(string username, string token, string apiUrl, int videoId) {
      this.username = username;
      this.token = token;
      this.apiUrl = apiUrl;
      this.videoId = videoId;

      this.api = new Vzaar(this.username, this.token);
      this.api.apiUrl = apiUrl;
    }

    public void whoAmITest() {
      this.assertEqual(this.api.whoAmI(), this.username, "whoAmI");
    }

    public void userDetailsTest() {
      var user = this.api.getUserDetails(this.username);
      this.assertEqual(user.authorName, this.username, "UserDetails");
    }

    public void accountDetailsTest() {
      var account = this.api.getAccountDetails(34);
      this.assertEqual(account.title, "Staff", "AccountDetails");
    }

    public void videoDetailsTest() {
      var vid = this.api.getVideoDetails(this.videoId);
      this.assertEqual(vid.type, "video", "videoDetails");
    }

    public void videoListTest() {
      var query = new VideoListQuery
      {
       count = 10,
       labels = new string[]{"api%2Capi2"}
      };
      
      var col = this.api.getVideoList(query);
      this.assertEqual(col.Count, 1, "videoList");
    }

    public void deleteVideoTest() {
       var guid = this.api.uploadVideo("./examples/video.mp4");
       var query = new VideoProcessQuery {
        guid = guid,
        title = "for deletion",
        description = ".net api test",
        profile = VideoProfile.ORIGINAL
      };
      var vidId = this.api.processVideo(query);
      var res = this.api.deleteVideo(vidId);
      this.assertEqual(res, true, "videoDelete");
    }

//    public void editVideoTest() {
//      var query = new VideoEditQuery {
//        title = this.RandomString(7),
//        id = this.videoId
//      };
//      var res = this.api.editVideo(query);
//      this.assertEqual(res, true, "editVideo");
//    }


    public void videoUploadTest() {
      var guid = this.api.uploadVideo("./examples/video.mp4");
      var title = String.Concat("api-net-", this.RandomString(5));
      
      var query = new VideoProcessQuery {
        guid = guid,
        title = title,
        description = ".net api test",
        profile = VideoProfile.ORIGINAL
      };
      var vidId = this.api.processVideo(query);
      var vid = this.api.getVideoDetails(vidId);

      this.assertEqual(vid.videoStatus.id, 11, "videoUpload");
      
      // clean up
      this.api.deleteVideo(vidId);
    }


    public void linkUploadTest() {
      var url = "http://samples.mplayerhq.hu/MPEG-4/turn-on-off.mp4";
      var title = String.Concat("api-net-lu-", this.RandomString(5));
      
      var query = new UploadLinkQuery {
        title = title,
        url = url,
        description = ".net api test"
      };
      
      var videoId = this.api.uploadLink(query);
      var vid = this.api.getVideoDetails(videoId);
      this.assertEqual(vid.videoStatus.id, 11, "linkUpload");
    }


    public void uploadSubtitleTest() {
      var body = "srt";
      
      var query = new SubtitleQuery {
        body = body,
        videoId = this.videoId,
        language = "en"
      };
      
      var res = this.api.uploadSubtitle(query);
      this.assertEqual(res, true, "uploadSubtitle");
    }

    public void generateThumbnailTest() {
      var status = this.api.generateThumbnail(this.videoId, 2);
      this.assertEqual(status, "Accepted", "generateThumbnail");
    }


//    public void uploadThumbnailTest() {
//      var path = "./examples/pic.jpg";
//      
//      var status = this.api.uploadThumbnail(this.videoId, path);
//      this.assertEqual(status, "Accepted", "uploadThumbnail");
//    }








    private string RandomString(int size) {
      char[] buffer = new char[size];

      for (int i = 0; i < size; i++) {
        buffer[i] = _chars[_rng.Next(_chars.Length)];
      }
      return new string(buffer);
    }



    public void assertEqual(object a, object b, string mName) {
      this.total += 1;
      if (a.Equals(b)) {
        System.Console.Write(".");
      }
      else
      {
        this.failures =+ 1;
        System.Console.Write("F(" + mName + ")");
      }
    }

    public void summarize() {
      var str = String.Concat("\n", this.total, " examples ", this.failures, " failures");
      System.Console.WriteLine(str);
    }
  }

  class Program {
		static void Main(string[] args)
		{
      var username = "vz-account1";
      var token = "cyCFbqQ4YTkrQhjFu7OZ2yoO3ol2avg79jRqWhKCpo";
      var url = "http://app.vzaar.localhost";
      var videoId = 1465464;
      
      if (args.Length > 0) {
        username = args[1];
        token = args[3];
        url = args[5];
        videoId = Int32.Parse(args[7]);
      }

      var ts = new TestSuite(username, token, url, videoId);
      ts.whoAmITest();
      ts.videoDetailsTest();
      ts.videoListTest();
      ts.videoUploadTest();
      ts.linkUploadTest();
      ts.uploadSubtitleTest();
//      ts.uploadThumbnailTest();
      ts.generateThumbnailTest();
      ts.userDetailsTest();
      ts.accountDetailsTest();
//      ts.editVideoTest();
      ts.deleteVideoTest();
      ts.summarize();

		}
	}
}