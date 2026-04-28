using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Company_Registration.Common
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }

        [JsonProperty("result")]
        public ApiResult Result { get; set; }
    }
}