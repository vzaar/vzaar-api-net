using System;

namespace com.vzaar.api
{
    public class UploadSignatureQuery
    {
        public String redirectUrl;
        public bool multipart = false;
        public String path;
        public String url;
        public String filename;
        public long fileSize = 0;
    }
}
