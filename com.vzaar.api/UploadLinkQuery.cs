using System;

namespace com.vzaar.api
{
    public class UploadLinkQuery
    {
        public string url = String.Empty;
        public string title = String.Empty;
        public string description = String.Empty;
        public int width = 0;
        public int size_id = 0;
        public int bitrate = 0;
        public bool transcoding = false;
    }
}
