using System;

namespace com.vzaar.api
{
    public class VideoProcessQuery
    {
        public string guid = String.Empty;
        public string title = String.Empty;
        public string description = String.Empty;
        public string[] labels = new string[] { };
        public int profile = 5;
        public bool transcode = false;
        public string replaceId = String.Empty;
    }
}
