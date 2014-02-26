//
// Code adapted from Brian Grinstead's multipart/form post.
// http://www.briangrinstead.com/blog/multipart-form-post-in-c
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace VzaarAPI.Transport.S3Amazon
{
	/// <summary>
	/// This class is responsible for compiling a HTTP multipart/form POST
	/// request. This is to support uploading files to Amazon's AWS S3.
	/// </summary>
	public static class FormUpload
	{
		private static readonly Encoding encoding = Encoding.UTF8;

		/// <summary>
		/// Make a multipart/form post request.
		/// </summary>
		/// <param name="postUrl">The post url</param>
		/// <param name="postParameters">The set of parameters for the post form</param>
		/// <param name="keyId">The Amazon S3 AWS ID</param>
		/// <param name="signature">The Amazon S3 signature (received from the Vzaar sign api request)</param>
		/// <returns></returns>
		public static VzaarTransportResponse MultipartFormDataPost(string postUrl,Dictionary<string, object> postParameters,string keyId,string signature)
		{
			var formDataBoundary = "-----------------------------28947758029299";
			var contentType = "multipart/form-data; charset=UTF-8; boundary=" + formDataBoundary;

			var formData = GetMultipartFormData(postParameters, formDataBoundary);

			return PostForm(postUrl, contentType, formData);
		}

		/// <summary>
		/// Post the multipart/form with the compiled request.
		/// </summary>
		/// <param name="postUrl">The post url</param>
		/// <param name="contentType">The content type being uploaded</param>
		/// <param name="formData">The byte array that represents the file</param>
		/// <returns></returns>
		private static VzaarTransportResponse PostForm(string postUrl,string contentType,byte[] formData)
		{
			// Create the post request
			var request = (HttpWebRequest)WebRequest.Create(postUrl);
			VzaarTransportResponse vzaarResponse;

		    if(request == null)
			{
				throw new NullReferenceException("Request is not a http request");
			}

			// Set up the request properties
			request.Method = "POST";
			request.KeepAlive = true;
			request.ContentType = contentType;
			request.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-GB; rv: 1.9.1.1) Gecko/20090715 Firefox/3.5.1 (.NET CLR 3.5.30729)";
			request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
			request.Headers.Add("Accept-Language", "en-gb,en;q=0.5");
			request.Headers.Add("Accept-Encoding", "gzip,deflate");
			request.Headers.Add("Accept-Charset: ISO-8859-1,utf-8;q=0.7,*;q=0.7");
			request.ContentLength = formData.Length;  // We need to count how many bytes we're sending.             
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
			request.AllowWriteStreamBuffering = false;
			request.SendChunked = false;
			request.Timeout = 1000000000;

			using(var requestStream = request.GetRequestStream())
			{
				// Push it out there
				requestStream.Write(formData, 0, formData.Length);
				requestStream.Close();
			}

			try
			{
			    // Get the response
			    var response = (HttpWebResponse)request.GetResponse();
			    vzaarResponse = new VzaarTransportResponse(response.StatusCode, response.StatusDescription, response.GetResponseStream());
			}
			catch(WebException ex)
			{
				vzaarResponse = new VzaarTransportResponse((HttpStatusCode)ex.Status, ex.Message, ex.Response.GetResponseStream());
			}
			catch(Exception ex)
			{
				vzaarResponse = new VzaarTransportResponse(HttpStatusCode.InternalServerError, ex.Message, null);
			}

			return vzaarResponse;
		}

		/// <summary>
		/// Get the multipart/form data.
		/// </summary>
		/// <param name="postParameters">The set of post form parameters</param>
		/// <param name="boundary">The boundary text seperating objects</param>        
		/// <returns>A byte array representing the encoded request</returns>
		private static byte[] GetMultipartFormData(Dictionary<string, object>postParameters, string boundary)
		{
			Stream formDataStream = new MemoryStream();

			// For each post parameter, create the appropriate postback information
			foreach(var param in postParameters)
			{
				// If the parameter is the File, then handle this differently to text based information being posted back
				// in the multipart/form-data type.
				if(param.Value is FileParameter)
				{
					var fileToUpload = (FileParameter)param.Value;

					// Add just the first part of this param, since we will write the file data directly to the Stream
					var header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\";\r\nContent-Type: {3}\r\nContent-Transfer-Encoding: binary\r\n\r\n",
						boundary,
						param.Key,
						fileToUpload.FileName ?? param.Key,
						fileToUpload.ContentType ?? "application/octet-stream");

					formDataStream.Write(encoding.GetBytes(header), 0, header.Length);

					// Write the file data directly to the Stream, rather than serializing it to a string.
					formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
				}
				else
				{
					var postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}\r\n",
						boundary,
						param.Key,
						param.Value);
					formDataStream.Write(encoding.GetBytes(postData), 0, postData.Length);
				}
			}

			// Add the end of the request
			var footer = "\r\n--" + boundary + "--\r\n";
			formDataStream.Write(encoding.GetBytes(footer), 0, footer.Length);

			// Dump the Stream into a byte[]
			formDataStream.Position = 0;
			var formData = new byte[formDataStream.Length];
			formDataStream.Read(formData, 0, formData.Length);
			formDataStream.Close();

			return formData;
		}

		/// <summary>
		/// A file parameter object that represents a multipart/form parameter. 
		/// </summary>
		public class FileParameter
		{
			/// <summary>
			/// The file in bytes
			/// </summary>
			public byte[] File
			{
				get;
				set;
			}
			/// <summary>
			/// The name of the file
			/// </summary>
			public string FileName
			{
				get;
				set;
			}
			/// <summary>
			/// The content type being uploaded (e.g. application/octet-stream, binary)
			/// </summary>
			public string ContentType
			{
				get;
				set;
			}

			/// <summary>
			/// Constructor 
			/// </summary>
			/// <param name="file">File in bytes</param>
			public FileParameter(byte[] file)
				: this(file, null)
			{
			}

			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="file">File in bytes</param>
			/// <param name="filename">Name of the uploaded file</param>
			public FileParameter(byte[] file, string filename)
				: this(file, filename, null)
			{
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="file">File in bytes</param>
			/// <param name="filename">Name of the uploaded file</param>
			/// <param name="contenttype">The content type being uploaded (e.g. application/octet-stream, binary)</param>
			public FileParameter(byte[] file, string filename, string contenttype)
			{
				File = file;
				FileName = filename;
				ContentType = contenttype;
			}
		}
	}
}