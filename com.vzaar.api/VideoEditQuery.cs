using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.vzaar.api
{
    public class VideoEditQuery
    {
        public Int64 id = 0;
        public string title = String.Empty;
        public string description = String.Empty;
        public bool markAsPrivate = false;
        public string seoUrl = String.Empty;
    }
}
