using Company_Registration_API.Models.DTO;
using Newtonsoft.Json;

namespace Company_Registration_API.Models.CompanyApplicant
{
    public class RegisterResponse : ResultBase
    {
        [JsonProperty("data")]
        public CompanyApplicantDto Data { get; set; }
    }
}