﻿using System;
using System.Collections.Specialized;
using System.Net;
using System.IO;

/**
 * Form
 * @author Maxim Fridberg
 * @updated November 29, 2011
 **/

namespace VzaarAPI.Transport
{
	public static class Form
	{
		public static event EventHandler ProgressEvent = delegate
		{
		};


		// NameValueCollection nvc = new NameValueCollection();
		// nvc.Add("id", "TTR");
		// nvc.Add("btn-submit-photo", "Upload");
		// HttpUploadFile("http://your.server.com/upload", @"C:\test\test.jpg", "file", "image/jpeg", nvc);


		public static FormWebResponse HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
		{
			var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
			var boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

			var wr = (HttpWebRequest)WebRequest.Create(url);
			wr.ContentType = "multipart/form-data; boundary=" + boundary;
			wr.Method = "POST";
			wr.KeepAlive = true;
			wr.Credentials = CredentialCache.DefaultCredentials;

			//todo find out if necessary
			wr.Headers.Add("Accept-Language", "en-gb,en;q=0.5");
			wr.Headers.Add("Accept-Encoding", "gzip,deflate");
			wr.Headers.Add("Accept-Charset: ISO-8859-1,utf-8;q=0.7,*;q=0.7");
			wr.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
			wr.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-GB; rv: 1.9.1.1) Gecko/20090715 Firefox/3.5.1 (.NET CLR 3.5.30729)";
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
			wr.AllowWriteStreamBuffering = false;
			wr.SendChunked = false;
			wr.Timeout = 1000000000;
			//request.ContentType = contentType;

			const string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
			var header = string.Format(headerTemplate, paramName, file, contentType);
			var headerbytes = System.Text.Encoding.UTF8.GetBytes(header);

			var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);

			const string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";

			var nvcByteCount = 0;
			foreach (string key in nvc.Keys)
			{
				var formitem = string.Format(formdataTemplate, key, nvc[key]);
				var formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
				nvcByteCount += formitembytes.Length;
			}
			var trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");

			wr.ContentLength = fileStream.Length + headerbytes.Length + ((nvc.Count + 1) * boundarybytes.Length) + nvcByteCount + trailer.Length;


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


			var buffer = new byte[4096];
			int bytesRead;
			while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
			{
				rs.Write(buffer, 0, bytesRead);

				ProgressEvent(null, new EventArgs());
			}
			fileStream.Close();


			rs.Write(trailer, 0, trailer.Length);
			rs.Close();

			HttpWebResponse wresp = null;
			FormWebResponse fresp = null;

			try
			{
				wresp = (HttpWebResponse)wr.GetResponse();
				var stream2 = wresp.GetResponseStream();
				var reader2 = new StreamReader(stream2);
				var respstr = reader2.ReadToEnd();
				fresp = new FormWebResponse(wresp.StatusCode, wresp.StatusDescription, respstr);

			}
			catch (Exception)
			{
				if (wresp != null)
				{
					wresp.Close();
				}
			}

			return fresp;
		}

	}
}