using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Company_Registration.Common
{
    public class ResponseDto
    { [JsonProperty("success")]
    public bool IsSuccess { get; set; }

    [JsonProperty("data")]
    public object Data { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

    public HttpStatusCode StatusCode { get; set; }
    }
}