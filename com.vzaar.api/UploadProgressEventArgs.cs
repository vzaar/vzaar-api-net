using System;

namespace com.vzaar.api
{
	public class UploadProgressEventArgs : EventArgs
	{
		public Int64 bytesUploaded;
		public Int64 bytesTotal;
	}
}
