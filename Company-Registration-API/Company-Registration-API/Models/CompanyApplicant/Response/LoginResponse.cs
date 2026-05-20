using Company_Registration_API.Models.DTO;
using Newtonsoft.Json;

namespace Company_Registration_API.Models.CompanyApplicant
{
    public class LoginResponse : ResultBase
    {
        [JsonProperty("data")]
        public LoginUserDto Data { get; set; }
    }
}