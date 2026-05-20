using Company_Registration_API.Models.CompanyApplicant;
using Newtonsoft.Json;

namespace Company_Registration_API.Models.CompanyRegistration.Response
{
    public class ResCompanyId : ResultBase
    {
        [JsonProperty("data")]
        public CompanyRegistrationDTO Data { get; set; }
    }
}