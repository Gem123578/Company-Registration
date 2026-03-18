using Company_Registration.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace Company_Registration.Common
{
    public class RequestDto
    {
        public string RequestUrl { get; set; }

        public eHTTPRequestType RequestType { get; set; }

        public object Data { get; set; }

        public string AccessToken { get; set; }

        public List<Header> Headers { get; set; }
    }
}