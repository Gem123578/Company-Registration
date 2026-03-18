using Newtonsoft.Json;

namespace Company_Registration.Common
{
    public class ApiResult
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}