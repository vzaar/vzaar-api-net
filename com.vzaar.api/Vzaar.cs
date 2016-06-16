using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
        
        public const String uploader = "net-1.2.1";
        public const String version = "1.2.1";
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
        public UserDetails getUserDetails(string username)
        {
            var url = apiUrl + "/api/users/" + username + ".json";
            var response = executeRequest(url);
            var jo = (JObject)JsonConvert.DeserializeObject(response);

            var details = new UserDetails
            {
                videoCount = String.IsNullOrEmpty(jo["video_count"].ToString()) ? 0 : Int64.Parse(jo["video_count"].ToString()),
                videosTotalSize = String.IsNullOrEmpty(jo["video_total_size"].ToString()) ? 0 : Int64.Parse(jo["video_total_size"].ToString()),
                maxFileSize = String.IsNullOrEmpty(jo["max_file_size"].ToString()) ? 0 : Int64.Parse(jo["max_file_size"].ToString()),
                playCount = String.IsNullOrEmpty(jo["play_count"].ToString()) ? 0 : Int64.Parse(jo["play_count"].ToString()),
                version = (string)jo["version"],
                authorId = String.IsNullOrEmpty(jo["author_id"].ToString()) ? 0 : int.Parse(jo["author_id"].ToString()),
                authorAccountTitle = (string)jo["author_account_title"],
                authorName = (string)jo["author_name"],
                authorAccount = String.IsNullOrEmpty(jo["author_account"].ToString()) ? 0 : int.Parse(jo["author_account"].ToString()),
                authorUrl = (string)jo["author_url"],
                createdAt = DateTime.Parse((string)jo["created_at"]),
                bandwidthThisMonth = String.IsNullOrEmpty(jo["bandwidth_this_month"].ToString()) ? 0 : Int64.Parse(jo["bandwidth_this_month"].ToString())
            };

            var bandwidth = new List<UserBandwidthDetails>();
            JsonConvert.PopulateObject(jo["bandwidth"].ToString(), bandwidth);
            details.bandwidth = bandwidth;

            return details;
        }

        /// <summary>
        /// This API call returns the details and rights for each vzaar subscription account type along with it's relevant metadata. This will show the details of the packages available here: http://vzaar.com/pricing
        /// </summary>
        /// <param name="accountId">is the vzaar account type. This is an integer.</param>
        /// <returns></returns>
        public AccountDetails getAccountDetails(int accountId)
        {
            var url = apiUrl + "/api/accounts/" + accountId + ".json";

            var response = executeRequest(url);
            var jo = (JObject)JsonConvert.DeserializeObject(response);

            var details = new AccountDetails
            {
                accountId = String.IsNullOrEmpty(jo["account_id"].ToString()) ? 0 : int.Parse(jo["account_id"].ToString()),
                title = (string)jo["title"],
                bandwidth = String.IsNullOrEmpty(jo["bandwidth"].ToString()) ? 0 : Int64.Parse(jo["bandwidth"].ToString()),
                cost = new AccountCostDetails
                {
                    monthly = String.IsNullOrEmpty(jo["cost"]["monthly"].ToString()) ? 0 : int.Parse(jo["cost"]["monthly"].ToString()),
                    currency = (string)jo["cost"]["currency"]
                },
                rights = new AccountRightsDetails
                {
                    borderless = String.IsNullOrEmpty(jo["rights"]["borderless"].ToString()) ? false : bool.Parse(jo["rights"]["borderless"].ToString()),
                    searchEnhancer = String.IsNullOrEmpty(jo["rights"]["searchEnhancer"].ToString()) ? false : bool.Parse(jo["rights"]["searchEnhancer"].ToString()),
                }
            };
            return details;
        }

        /// <summary>
        /// vzaar uses the oEmbed open standard for allowing 3rd parties to integrated with the vzaar.
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        public VideoDetails getVideoDetails(Int64 videoId)
        {
            var url = apiUrl + "/api/videos/" + videoId + ".json";

            var response = executeRequest(url);
            var jo = (JObject)JsonConvert.DeserializeObject(response);
            var result = new VideoDetails();

            try
            {
                result = new VideoDetails
                {
                    duration = String.IsNullOrEmpty(jo["duration"].ToString()) ? 0 : decimal.Parse(jo["duration"].ToString()),
                    type = (string)jo["type"],
                    height = String.IsNullOrEmpty(jo["height"].ToString()) ? 0 : int.Parse(jo["height"].ToString()),
                    width = String.IsNullOrEmpty(jo["width"].ToString()) ? 0 : int.Parse(jo["width"].ToString()),
                    url = (string)jo["video_url"],
                    provider = new VideoDetailsProvider
                    {
                        name = (string)jo["provider_name"],
                        url = (string)jo["provider_url"]
                    },
                    playCount = String.IsNullOrEmpty(jo["play_count"].ToString()) ? 0 : Int64.Parse(jo["play_count"].ToString()),
                    videoStatus = new VideoDetailsVideoStatus
                    {
                        id = String.IsNullOrEmpty(jo["video_status_id"].ToString()) ? 0 : int.Parse(jo["video_status_id"].ToString()),
                        description = (string)jo["video_status_description"]
                    },
                    thumbnail = new VideoDetailsThumbnail
                    {
                        height = String.IsNullOrEmpty(jo["thumbnail_height"].ToString()) ? 0 : int.Parse(jo["thumbnail_height"].ToString()),
                        width = String.IsNullOrEmpty(jo["thumbnail_width"].ToString()) ? 0 : int.Parse(jo["thumbnail_width"].ToString()),
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
                        height = String.IsNullOrEmpty(jo["framegrab_height"].ToString()) ? 0 : int.Parse(jo["framegrab_height"].ToString()),
                        width = String.IsNullOrEmpty(jo["framegrab_width"].ToString()) ? 0 : int.Parse(jo["framegrab_width"].ToString()),
                        url = (string)jo["framegrab_url"]
                    },
                    totalSize = String.IsNullOrEmpty(jo["total_size"].ToString()) ? 0 : Int64.Parse(jo["total_size"].ToString()),
                    title = (string)jo["title"],
                    description = (string)jo["description"]
                };

                var renditions = new List<VideoRendition>();
                JsonConvert.PopulateObject(jo["renditions"].ToString(), renditions);
                result.renditions = renditions;
            }
            catch
            {
                result = new VideoDetails
                {
                    videoStatus = new VideoDetailsVideoStatus
                    {
                        id =
                            String.IsNullOrEmpty(jo["vzaar-api"]["video"]["video_status_id"].ToString())
                                ? 0
                                : int.Parse(jo["vzaar-api"]["video"]["video_status_id"].ToString()),
                        description = (string)jo["vzaar-api"]["video"]["state"]
                    }
                };
            }

            return result;
        }

        /// <summary>
        /// This API call returns a list of the user's active videos along with it's relevant metadata. 20 videos are returned by default but this is customisable.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<Video> getVideoList(VideoListQuery query)
        {
            var url = apiUrl + "/api/" + username + "/videos.json?count=" + query.count.ToString();

            if (query.labels.Length > 0)
            {
                var outh = new OAuthBase();
                url += "&labels=" + outh.UrlEncode(String.Join(",", query.labels));
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
                url += "&title=" + HttpUtility.UrlEncode(query.title);
            }

            url += "&page=" + ((query.page > 1) ? query.page : 1);

            var response = executeRequest(url);
            var jo = (JArray)JsonConvert.DeserializeObject(response);

            var result = new List<Video>();

            foreach (var o in jo)
            {
                var video = new Video
                {
                    status = (string)o["status"],
                    statusId = String.IsNullOrEmpty(o["status_id"].ToString()) ? 0 : int.Parse(o["status_id"].ToString()),
                    duration = String.IsNullOrEmpty(o["duration"].ToString()) ? 0 : decimal.Parse(o["duration"].ToString()),
                    description = (string)o["description"],
                    height = string.IsNullOrEmpty(o["height"].ToString()) ? 0 : int.Parse(o["height"].ToString()),
                    createdAt = DateTime.Parse((string)o["created_at"].ToString()),
                    width = string.IsNullOrEmpty(o["width"].ToString()) ? 0 : int.Parse(o["width"].ToString()),
                    playCount = String.IsNullOrEmpty(o["play_count"].ToString()) ? 0 : Int64.Parse(o["play_count"].ToString()),
                    version = (string)o["version"],
                    thumbnail = (string)o["thumbnail"],
                    url = (string)o["url"],
                    id = String.IsNullOrEmpty(o["id"].ToString()) ? 0 : Int64.Parse(o["id"].ToString()),
                    title = (string)o["title"],
                    user = new VideoAuthor
                    {
                        videoCount = String.IsNullOrEmpty(o["user"]["video_count"].ToString()) ? 0 : Int64.Parse(o["user"]["video_count"].ToString()),
                        name = (string)o["user"]["author_name"],
                        account = String.IsNullOrEmpty(o["user"]["author_account"].ToString()) ? 0 : int.Parse(o["user"]["author_account"].ToString()),
                        url = (string)o["user"]["author_url"]
                    }
                };

                result.Add(video);
            }

            return result;
        }
        public UploadSignature getUploadSignature(UploadSignatureQuery query)
        {
            var url = apiUrl + "/api/v1.1/videos/signature";
            var oAuth = new OAuthBase();
            if (String.IsNullOrEmpty(query.path) && String.IsNullOrEmpty(query.url))
            {
                throw new ArgumentException("path or url must be provided");
            }

            if (!String.IsNullOrEmpty(query.path))
            {
                url += "?path=" + oAuth.UrlEncode(query.path);
                if (!String.IsNullOrEmpty(query.filename))
                {
                    url += "&filename=" + oAuth.UrlEncode(query.filename);
                }
                if (query.fileSize > 0)
                {
                    url += "&filesize=" + query.fileSize;
                }
            }
            else
            {
                url += "?url=" + oAuth.UrlEncode(query.url);
            }

            if (enableFlashSupport)
            {
                url += "&flash_request=true";
            }
            if (!String.IsNullOrEmpty(query.redirectUrl))
            {
                url += "&success_action_redirect=" + oAuth.UrlEncode(query.redirectUrl);
            }

            if (query.multipart)
            {
                url += "&multipart=true";
                url += "&uploader=" + uploader;
            }

            UploadSignature signature = null;

            var response = executeRequest(url);
            signature = new UploadSignature(response);

            return signature;
        }

        /// <summary>
        /// Upload video from local drive to vzaar upload host
        /// </summary>
        /// <param name="path">Path of the video file to be uploaded</param>
        /// <returns>GUID of the file uploaded</returns>
        public string uploadVideo(string path)
        {
            var query = new UploadSignatureQuery();
            query.path = path;
            var fileInfo = new FileInfo(path);
            query.fileSize = fileInfo.Length;
            query.filename = fileInfo.Name;
            query.multipart = true;
            var signature = new UploadSignature();

            signature = getUploadSignature(query);
            if (String.IsNullOrEmpty(signature.chunkSize))
            {
                return simpleUpload(path, signature);
            }

            return multipartUpload(path, signature);  
        }
        private string simpleUpload(string path, UploadSignature signature)
        {
            var parameters = new NameValueCollection
                                 {
                                     {"AWSAccessKeyId", signature.accessKeyId},
                                     {"signature", signature.signature},
                                     {"acl", signature.acl},
                                     {"bucket", signature.bucket},
                                     {"policy", signature.policy},
                                     {"success_action_status", "201"},
                                     {"key", signature.key},
                                     {"chunks", "0"},
                                     {"chunk", "0"},
                                     {"x-amz-meta-uploader", uploader}
                                 };

            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            var guid = HttpUploadFile(signature.uplodHostname, parameters, fileStream, path);
            fileStream.Close();
            return guid;
        }

        /// <summary>
        /// Upload video from local drive in multiple chunks
        /// </summary>
        /// <param name="path">Path of the video file to be uploaded</param>
        /// <param name="signature">Object of type <see cref="UploadSignature"/></param>
        /// <returns> GUID string of thhe file uploaded</returns>
        private string multipartUpload(string path, UploadSignature signature)
        {
            string guid = null;
            var fileInfo = new FileInfo(path);
            long filesize = fileInfo.Length;
            int chunks = (int)Math.Ceiling((double)filesize / chunkSyzeInBytes(signature.chunkSize));
            var buffer = new byte[chunkSyzeInBytes("5mb")];
            int bytesRead;
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            int chunk = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                MemoryStream ms = new MemoryStream(buffer, 0, bytesRead);

                string key = signature.key.Replace("${filename}", fileInfo.Name) + "." + chunk;
                var parameters = new NameValueCollection
                                 {
                                     {"AWSAccessKeyId", signature.accessKeyId},
                                     {"signature", signature.signature},
                                     {"acl", signature.acl},
                                     {"bucket", signature.bucket},
                                     {"policy", signature.policy},
                                     {"success_action_status", "201"},
                                     {"key", key},
                                     {"chunks", chunks.ToString()},
                                     {"chunk", chunk.ToString()},
                                     {"x-amz-meta-uploader", uploader}
                                 };

                guid = HttpUploadFile(signature.uplodHostname, parameters, ms, path);
                chunk++;
                ms.Close();
                ms.Dispose();
            }
            fileStream.Close();

            return guid;
        }
        private string HttpUploadFile(string url, NameValueCollection nvc, Stream stream, string file)
        {
            var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            var boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            var wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = CredentialCache.DefaultCredentials;

            wr.Headers.Add("Accept-Language", "en-gb,en;q=0.5");
            wr.Headers.Add("Accept-Encoding", "gzip,deflate");
            wr.Headers.Add("Accept-Charset: ISO-8859-1,utf-8;q=0.7,*;q=0.7");
            wr.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            wr.AllowWriteStreamBuffering = false;
            wr.SendChunked = false;
            //wr.Timeout = 1000000000;

            const string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            var header = string.Format(headerTemplate, "file", file, "application/octet-stream");
            var headerbytes = System.Text.Encoding.UTF8.GetBytes(header);

            const string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";

            var nvcByteCount = 0;
            foreach (string key in nvc.Keys)
            {
                var formitem = string.Format(formdataTemplate, key, nvc[key]);
                var formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                nvcByteCount += formitembytes.Length;
            }

            var trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");

            wr.ContentLength = stream.Length + headerbytes.Length + ((nvc.Count + 1) * boundarybytes.Length) + nvcByteCount + trailer.Length;

            var rs = wr.GetRequestStream();


            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                var formitem = string.Format(formdataTemplate, key, nvc[key]);
                var formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);
            rs.Write(headerbytes, 0, headerbytes.Length);

            var buffer = new byte[bufferSize];
            int bytesRead;
            var bytesUploaded = 0;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);

                //raise event with upload progress
                bytesUploaded += bytesRead;
                var eventArgs = new UploadProgressEventArgs
                {
                    bytesUploaded = bytesUploaded,
                    bytesTotal = stream.Length
                };

                ProgressEvent(this, eventArgs);

            }

            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            HttpWebResponse wresp = null;
            var rawResponse = "";

            try
            {
                wresp = (HttpWebResponse)wr.GetResponse();
                var stream2 = wresp.GetResponseStream();
                var reader2 = new StreamReader(stream2);
                rawResponse = reader2.ReadToEnd();

            }
            catch (Exception ex)
            {
                if (wresp != null)
                {
                    wresp.Close();
                }
            }

            var guid = "";
            if (rawResponse.Length > 0)
            {
                var doc = new XmlDocument();
                doc.LoadXml(rawResponse);
                string[] keyParts = doc.SelectSingleNode("//Key").InnerText.Split('/');
                guid = keyParts[keyParts.Length - 2];
            }
            return guid;
        }
        public Int64 processVideo ( VideoProcessQuery query )
        {
            var url = apiUrl + "/api/v1.1/videos";
            var data = "<vzaar-api><video>";
            if (query.replaceId != "")
                data += "<replace_id>" + query.replaceId + "</replace_id>";
            data += "<guid>" + query.guid + "</guid><title>" + HttpUtility.HtmlEncode(query.title) + "</title><description>" + HttpUtility.HtmlEncode(query.description) + "</description><labels>";
            data += HttpUtility.HtmlEncode(String.Join( ",", query.labels )) + "</labels><profile>" + (int)query.profile + "</profile>";
            if (query.chunks > 0)
            {
                data += "<chunks>" + query.chunks + "</chunks>";
            }
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

            var url = apiUrl + "/api/videos/" + query.id.ToString() + ".xml";

            var data = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><vzaar-api><_method>put</_method><video><title>" + HttpUtility.HtmlEncode(query.title) + "</title><description>" + HttpUtility.HtmlEncode(query.description) + "</description>";
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
		               "</language><video_id>" + query.videoId.ToString() + "</video_id><body>" + HttpUtility.HtmlEncode(query.body)+
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
				var reader = new StreamReader(response.GetResponseStream());
				rawResponse = reader.ReadToEnd();

			}
			catch (Exception ex)
			{
			    result = null;
			}
			if (rawResponse.Length > 0)
			{
				JObject resp = JObject.Parse(rawResponse);
				result = resp["vzaar-api"]["status"].ToString();
			}

			return result;
		}

        public Int64 uploadLink(UploadLinkQuery query)
        {
            var url = apiUrl + "/api/upload/link.xml";

            var signatureQuery = new UploadSignatureQuery();
            signatureQuery.url = query.url;
            signatureQuery.multipart = true;

            var signature = new UploadSignature();

            signature = getUploadSignature(signatureQuery);

            var data = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                       "<vzaar-api>" +
                            "<link_upload>" +
                                "<key>" + signature.key + "</key>" +
                                "<guid>" + signature.guid + "</guid>" +
                                "<url>" + HttpUtility.HtmlEncode(query.url) + "</url>" +
                                "<encoding_params>" +
                                    "<title>" + HttpUtility.HtmlEncode(query.title) + "</title>" +
                                    "<description>" + HttpUtility.HtmlEncode(query.description) + "</description>" +
                                    "<size_id>" + query.size_id + "</size_id>" +
                                    "<bitrate>" + query.bitrate + "</bitrate>" +
                                    "<width>" + query.width + "</width>" +
                                    "<transcoding>" + query.transcoding.ToString().ToLower() + "</transcoding>" +
                                "</encoding_params>" +
                            "</link_upload>" +
                       "</vzaar-api>";

            var response = executeRequest(url, "POST", data);

            var doc = new XmlDocument();
            doc.LoadXml(response);
            var videoId = Int64.Parse(doc.SelectSingleNode("//id").InnerText);

            return videoId;
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
                var reader = new StreamReader( response.GetResponseStream() );
                rawResponse = reader.ReadToEnd();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rawResponse;
        }
        private long chunkSyzeInBytes(String str)
        {
            int index = str.ToLower().IndexOf("mb");
            int number;
            int.TryParse(str.Substring(0, index), out number);

            return (long)(number * Math.Pow(1024, 2));
        }
    }
}
