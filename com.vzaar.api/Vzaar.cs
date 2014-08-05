using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Web;
using System.Text;
using System.Collections.Specialized;
using System.Xml;
using com.vzaar.api.OAuth;

namespace com.vzaar.api
{
    public class Vzaar
    {
        public delegate void UploadProgressHandler ( object sender, UploadProgressEventArgs e );
        public event UploadProgressHandler ProgressEvent = delegate { };

        public string username;
        public string token;

        public long bufferSize = 131072; //128Kb

        public bool enableFlashSupport = false;

        public string apiUrl = "https://vzaar.com";

        public Vzaar ()
        {
        }

        public Vzaar ( string username, string token )
        {
            this.username = username;
            this.token = token;
        }

        public string whoAmI ()
        {
            var url = apiUrl + "/api/test/whoami.json";

            var response = executeRequest( url );
            var jo = (JObject)JsonConvert.DeserializeObject( response );
            var username = (String)jo["vzaar_api"]["test"]["login"];
            return username;
        }

        /// <summary>
        /// This API call returns the user's public details along with it's relevant metadata.
        /// </summary>
        /// <param name="username">is the vzaar login name for the user. Note: This must be the actual username and not the email address</param>
        /// <returns></returns>
        public UserDetails getUserDetails ( string username )
        {
            var url = apiUrl + "/users/" + username + ".json";
            var response = executeRequest( url );
            var jo = (JObject)JsonConvert.DeserializeObject( response );

            var details = new UserDetails
            {
                videoCount = (Int64)jo["video_count"],
                videosTotalSize = (Int64)jo["video_total_size"],
                maxFileSize = (Int64)jo["max_file_size"],
                playCount = (Int64)jo["play_count"],
                version = (string)jo["version"],
                authorId = (int)jo["author_id"],
                authorAccountTitle = (string)jo["author_account_title"],
                authorName = (string)jo["author_name"],
                authorAccount = (int)jo["author_account"],
                authorUrl = (string)jo["author_url"],
                createdAt = DateTime.Parse( (string)jo["created_at"] ),
                bandwidthThisMonth = (Int64)jo["bandwidth_this_month"]
            };

            var bandwidth = new List<UserBandwidthDetails>();
            JsonConvert.PopulateObject( jo["bandwidth"].ToString(), bandwidth );
            details.bandwidth = bandwidth;

            return details;
        }

        /// <summary>
        /// This API call returns the details and rights for each vzaar subscription account type along with it's relevant metadata. This will show the details of the packages available here: http://vzaar.com/pricing
        /// </summary>
        /// <param name="accountId">is the vzaar account type. This is an integer.</param>
        /// <returns></returns>
        public AccountDetails getAccountDetails ( int accountId )
        {
            var url = apiUrl + "/api/accounts/" + accountId + ".json";

            var response = executeRequest( url );
            var jo = (JObject)JsonConvert.DeserializeObject( response );

            var details = new AccountDetails
            {
                accountId = (int)jo["account_id"],
                title = (string)jo["title"],
                bandwidth = (Int64)jo["bandwidth"],
                cost = new AccountCostDetails
                {
                    monthly = (int)jo["cost"]["monthly"],
                    currency = (string)jo["cost"]["currency"]
                },
                rights = new AccountRightsDetails
                {
                    borderless = (bool)jo["rights"]["borderless"],
                    searchEnhancer = (bool)jo["rights"]["searchEnhancer"]
                }
            };
            return details;
        }

        /// <summary>
        /// vzaar uses the oEmbed open standard for allowing 3rd parties to integrated with the vzaar. 
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        public VideoDetails getVideoDetails ( Int64 videoId )
        {
            var url = apiUrl + "/api/videos/" + videoId + ".json";

            var response = executeRequest( url );
            var jo = (JObject)JsonConvert.DeserializeObject( response );

            var result = new VideoDetails
            {
                duration = (decimal)jo["duration"],
                type = (string)jo["type"],
                height = (int)jo["height"],
                width = (int)jo["width"],
                url = (string)jo["video_url"],
                provider = new VideoDetailsProvider
                {
                    name = (string)jo["provider_name"],
                    url = (string)jo["provider_url"]
                },
                playCount = (Int64)jo["play_count"],
                videoStatus = new VideoDetailsVideoStatus
                {
                    id = (int)jo["video_status_id"],
                    description = (string)jo["video_status_description"]
                },
                thumbnail = new VideoDetailsThumbnail
                {
                    height = (int)jo["thumbnail_height"],
                    width = (int)jo["thumbnail_width"],
                    url = (string)jo["thumbnail_url"]
                },
                author = new VideoDetailsAuthor
                {
                    name = (string)jo["author_name"],
                    url = (string)jo["author_url"]
                },
                poster = "http://view.vzaar.com/" + videoId + "/image",
                html = (string)jo["html"],
                framegrab = new VideoDetailsFramegrab
                {
                    height = (int)jo["framegrab_height"],
                    width = (int)jo["framegrab_width"],
                    url = (string)jo["framegrab_url"]
                },
                totalSize = (Int64)jo["total_size"],
                title = (string)jo["title"],
                description = (string)jo["description"]
            };
            return result;
        }

        /// <summary>
        /// This API call returns a list of the user's active videos along with it's relevant metadata. 20 videos are returned by default but this is customisable.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<Video> getVideoList ( VideoListQuery query )
        {
            var url = apiUrl + "/api/" + username + "/videos.json?count=" + query.count.ToString();

            if (query.labels.Length > 0)
            {
                url += "&labels=" + String.Join( ",", query.labels ); //should we have it URL Encoded?
            }

            if (query.status != String.Empty)
            {
                url += "&status=" + query.status;
            }

            if (query.sort == VideoListSorting.ASCENDING)
            {
                url += "&sort=" + "asc";
            }
            else
            {
                url += "&sort=" + "desc";
            }

            if (query.title != String.Empty)
            {
                url += "&title=" + HttpUtility.UrlEncode( query.title );
            }

            var response = executeRequest( url );
            var jo = (JArray)JsonConvert.DeserializeObject( response );

            var result = new List<Video>();

            foreach (var o in jo)
            {
                var video = new Video
                {
                    status = (string)o["status"],
                    statusId = (int)o["status_id"],
                    duration = (decimal)o["duration"],
                    description = (string)o["description"],
                    height = string.IsNullOrEmpty( o["height"].ToString() ) ? 0 : int.Parse( o["height"].ToString() ),
                    createdAt = DateTime.Parse( (string)o["created_at"].ToString() ),
                    width = string.IsNullOrEmpty( o["width"].ToString() ) ? 0 : int.Parse( o["width"].ToString() ),
                    playCount = (Int64)o["play_count"],
                    version = (string)o["version"],
                    thumbnail = (string)o["thumbnail"],
                    url = (string)o["url"],
                    id = (Int64)o["id"],
                    title = (string)o["title"],
                    user = new VideoAuthor
                    {
                        videoCount = (Int64)o["user"]["video_count"],
                        name = (string)o["user"]["author_name"],
                        account = (int)o["user"]["author_account"],
                        url = (string)o["user"]["author_url"]
                    }
                };

                result.Add( video );
            }

            return result;
        }

        public UploadSignature getUploadSignature ()
        {
            var url = apiUrl + "/api/videos/signature";

            if (enableFlashSupport)
            {
                apiUrl += "?=flash_request=true";
            }

            var response = executeRequest( url );


            var signature = new UploadSignature( response );
            return signature;
        }

        public UploadSignature getUploadSignature ( string redirectUrl )
        {
            var url = apiUrl + "/api/videos/signature";

            var oAuth = new OAuthBase();
            redirectUrl = oAuth.UrlEncode( redirectUrl );

            if (enableFlashSupport)
            {
                apiUrl += "?flash_request=true";
            }

            if (redirectUrl != String.Empty)
            {
                if (!enableFlashSupport)
                {
                    url += "?success_action_redirect=" + redirectUrl;
                }
                else
                {
                    url += "&success_action_redirect=" + redirectUrl;
                }
            }

            var response = executeRequest( url );

            var signature = new UploadSignature( response );
            return signature;
        }

        public string uploadVideo ( string path )
        {
            var signature = new UploadSignature();

            signature = getUploadSignature();

            var url = "https://" + signature.bucket + ".s3.amazonaws.com/";

            var parameters = new NameValueCollection
                                 {
                                     {"key", signature.key},
                                     {"AWSAccessKeyId", signature.accessKeyId},
                                     {"acl", signature.acl},
                                     {"policy", signature.policy},
                                     {"signature", signature.signature},
                                     {"success_action_status", "201"}
                                 };

            var response = HttpUploadFile( url, path, "file", "application/octet-stream", parameters );

            return signature.guid;
        }

        private string HttpUploadFile ( string url, string file, string paramName, string contentType, NameValueCollection nvc )
        {
            var boundary = "---------------------------" + DateTime.Now.Ticks.ToString( "x" );
            var boundarybytes = System.Text.Encoding.ASCII.GetBytes( "\r\n--" + boundary + "\r\n" );

            var wr = (HttpWebRequest)WebRequest.Create( url );
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = CredentialCache.DefaultCredentials;

            //todo find out if necessary
            wr.Headers.Add( "Accept-Language", "en-gb,en;q=0.5" );
            wr.Headers.Add( "Accept-Encoding", "gzip,deflate" );
            wr.Headers.Add( "Accept-Charset: ISO-8859-1,utf-8;q=0.7,*;q=0.7" );
            wr.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            wr.AllowWriteStreamBuffering = false;
            wr.SendChunked = false;
            //wr.Timeout = 1000000000;
            //request.ContentType = contentType;

            const string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            var header = string.Format( headerTemplate, paramName, file, contentType );
            var headerbytes = System.Text.Encoding.UTF8.GetBytes( header );

            var fileStream = new FileStream( file, FileMode.Open, FileAccess.Read );

            const string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";

            var nvcByteCount = 0;
            foreach (string key in nvc.Keys)
            {
                var formitem = string.Format( formdataTemplate, key, nvc[key] );
                var formitembytes = System.Text.Encoding.UTF8.GetBytes( formitem );
                nvcByteCount += formitembytes.Length;
            }
            var trailer = System.Text.Encoding.ASCII.GetBytes( "\r\n--" + boundary + "--\r\n" );

            wr.ContentLength = fileStream.Length + headerbytes.Length + ((nvc.Count + 1) * boundarybytes.Length) + nvcByteCount + trailer.Length;


            var rs = wr.GetRequestStream();


            foreach (string key in nvc.Keys)
            {
                rs.Write( boundarybytes, 0, boundarybytes.Length );
                var formitem = string.Format( formdataTemplate, key, nvc[key] );
                var formitembytes = System.Text.Encoding.UTF8.GetBytes( formitem );
                rs.Write( formitembytes, 0, formitembytes.Length );
            }
            rs.Write( boundarybytes, 0, boundarybytes.Length );

            rs.Write( headerbytes, 0, headerbytes.Length );

            var buffer = new byte[bufferSize];
            int bytesRead;
            var bytesUploaded = 0;
            while ((bytesRead = fileStream.Read( buffer, 0, buffer.Length )) != 0)
            {
                rs.Write( buffer, 0, bytesRead );

                //raise event with upload progress
                bytesUploaded += bytesRead;
                var eventArgs = new UploadProgressEventArgs
                {
                    bytesUploaded = bytesUploaded,
                    bytesTotal = fileStream.Length
                };

                ProgressEvent( this, eventArgs );

            }
            fileStream.Close();

            rs.Write( trailer, 0, trailer.Length );
            rs.Close();

            HttpWebResponse wresp = null;
            var response = "";

            try
            {
                wresp = (HttpWebResponse)wr.GetResponse();
                var stream2 = wresp.GetResponseStream();
                var reader2 = new StreamReader( stream2 );
                response = reader2.ReadToEnd();

            }
            catch (Exception ex)
            {
                Console.Write( ex.ToString() );
                if (wresp != null)
                {
                    wresp.Close();
                }
            }

            return response;
        }

        public Int64 processVideo ( VideoProcessQuery query )
        {
            var url = apiUrl + "/api/videos";
            var data = "<vzaar-api><video>";
            if (query.replaceId != "")
                data += "<replace_id>" + query.replaceId + "</replace_id>";
            data += "<guid>" + query.guid + "</guid><title>" + query.title + "</title><description>" + query.description + "</description><labels>";
            data += String.Join( ",", query.labels ) + "</labels><profile>" + query.profile + "</profile>";
            if (query.transcode)
                data += "<transcoding>true</transcoding>";
            data += "</video> </vzaar-api>";

            var response = executeRequest( url, "POST", data );

            var doc = new XmlDocument();
            doc.LoadXml( response );
            var videoId = Int64.Parse( doc.SelectSingleNode( "//video" ).InnerText );

            return videoId;
        }

        public bool deleteVideo ( Int64 videoId )
        {
            var url = apiUrl + "/api/videos/" + videoId.ToString() + ".xml";

            var data = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><vzaar-api><_method>delete</_method></vzaar-api>";

            var response = executeRequest( url, "DELETE", data );

            var doc = new XmlDocument();
            doc.LoadXml( response );
            var videoStatus = int.Parse( doc.SelectSingleNode( "//video_status_id" ).InnerText );


            return videoStatus == (int)VideoStatus.DELETED;
        }

        public bool editVideo ( VideoEditQuery query )
        {

            var url = apiUrl + "/api/videos/" + query.id.ToString() + ".json";

            var data = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><vzaar-api><_method>put</_method><video><title>" + query.title + "</title><description>" + query.description + "</description>";
            data += "<private>" + query.markAsPrivate + "</private>";

            if (query.seoUrl != "")
                data += "<seo_url>" + query.seoUrl + "</seo_url>";
            data += "</video></vzaar-api>";

            var response = executeRequest( url, "PUT", data );
            if (String.IsNullOrEmpty( response ))
                return false;

            return true;
        }
		public bool uploadSubtitle(SubtitleQuery query)
		{
			var url = apiUrl + "/api/subtitle/upload.xml";
			var data = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><vzaar-api><subtitle><language>" + query.language +
					   "</language><video_id>" + query.videoId.ToString() + "</video_id><body>" + query.body +
					   "</body></subtitle></vzaar-api>";

			var response = executeRequest(url, "POST", data);

			var doc = new XmlDocument();

			doc.LoadXml(response);
			var status = doc.SelectSingleNode("//status").InnerText;
			if (status.ToLower() != "accepted")
				return false;

			return true;
		}

		public string generateThumbnail(Int64 videoId, int thumbTime)
		{
			var url = apiUrl + "/api/videos/" + videoId + "/generate_thumb.xml";
			if (thumbTime < 0)
				thumbTime = 0;
			var data = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><vzaar-api><video><thumb_time>" +
					   +thumbTime + "</thumb_time></video></vzaar-api>";

			var response = executeRequest(url, "POST", data);

			if (response.Length > 0)
			{
				var doc = new XmlDocument();
				doc.LoadXml(response);
				var status = doc.SelectSingleNode("//status").InnerText;
				return status;
			}
			return null;
		}

		public string uploadThumbnail(Int64 videoId, string path)
		{
			var url = apiUrl + "/api/videos/" + videoId + "/upload_thumb.json";
			var result = String.Empty;
			var request = (HttpWebRequest)WebRequest.Create(url);
			request.Method = "POST";

			var consumer = new OAuth.OAuthConsumer();
			consumer.SetTokenWithSecret(username, token);
			request = consumer.Sign(request);

			var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
			request.ContentType = "multipart/form-data; boundary=" + boundary;
			boundary = "--" + boundary;

			var rs = request.GetRequestStream();

			var buff = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
			rs.Write(buff, 0, buff.Length);
			buff = Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"{2}", "vzaar-api[thumbnail]", path, Environment.NewLine));
			rs.Write(buff, 0, buff.Length);
			buff = Encoding.ASCII.GetBytes(string.Format("Content-Type: {0}{1}{1}", "image/jpeg", Environment.NewLine));
			rs.Write(buff, 0, buff.Length);

			var imgStream = File.Open(path, FileMode.Open);
			imgStream.CopyTo(rs);

			buff = Encoding.ASCII.GetBytes(Environment.NewLine);
			rs.Write(buff, 0, buff.Length);

			var boundaryBuffer = Encoding.ASCII.GetBytes(boundary + "--");
			rs.Write(boundaryBuffer, 0, boundaryBuffer.Length);

			rs.Close();

			WebResponse response = null;
			var rawResponse = String.Empty;

			try
			{
				response = request.GetResponse();
				Debug.WriteLine(((HttpWebResponse)response).StatusDescription);
				var reader = new StreamReader(response.GetResponseStream());
				rawResponse = reader.ReadToEnd();

			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
				throw;
			}
			if (rawResponse.Length > 0)
			{
				JObject resp = JObject.Parse(rawResponse);
				result = resp["vzaar-api"]["status"].ToString();
			}

			return result;
		}

        private string executeRequest ( string url )
        {
            return executeRequest( url, "GET", null );
        }

        private string executeRequest ( string url, string method, string data )
        {
            var request = (HttpWebRequest)WebRequest.Create( url );
            request.Method = method;

            //sign the request
            var consumer = new OAuth.OAuthConsumer();
            consumer.SetTokenWithSecret( username, token );
            request = consumer.Sign( request );


            switch (method.ToUpper())
            {
                case "GET":
                    break;

                case "POST":
                    request.ContentType = "application/xml";
                    request.ContentLength = Encoding.UTF8.GetBytes( data ).Length;

                    var requestStream = request.GetRequestStream();
                    requestStream.Write( Encoding.UTF8.GetBytes( data ), 0, Encoding.UTF8.GetBytes( data ).Length );
                    requestStream.Close();
                    break;

                case "PUT":
                    request.ContentType = "application/xml";
                    request.ContentLength = Encoding.UTF8.GetBytes(data).Length;

                    var reqS = request.GetRequestStream();
                    reqS.Write(Encoding.UTF8.GetBytes(data), 0, Encoding.UTF8.GetBytes(data).Length);
                    reqS.Close();
                    break;

                case "DELETE":
                    request.ContentType = "application/xml";
                    request.KeepAlive = false;
                    request.ContentLength = Encoding.UTF8.GetBytes( data ).Length;

                    var rs = request.GetRequestStream();
                    rs.Write( Encoding.UTF8.GetBytes( data ), 0, Encoding.UTF8.GetBytes( data ).Length );
                    rs.Close();
                    break;

                default:
                    throw new Exception( "HTTP Method " + method + " is not supported" );
            }

            WebResponse response = null;
            var rawResponse = String.Empty;

            try
            {
                response = request.GetResponse();
                Debug.WriteLine( ((HttpWebResponse)response).StatusDescription );
                var reader = new StreamReader( response.GetResponseStream() );
                rawResponse = reader.ReadToEnd();

            }
            catch (Exception ex)
            {
                Debug.WriteLine( ex.ToString() );
                throw;
            }

            return rawResponse;
        }
    }
}
