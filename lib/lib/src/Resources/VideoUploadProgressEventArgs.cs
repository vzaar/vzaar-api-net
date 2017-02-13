using System;

namespace VzaarApi
{
	public class VideoUploadProgressEventArgs : EventArgs
	{
		public long totalParts; //amount of chunks to upload
		public long uploadedChunk; //uploaded chunk number
	}
}

