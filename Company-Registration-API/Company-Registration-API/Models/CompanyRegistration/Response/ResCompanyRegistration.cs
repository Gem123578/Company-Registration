using Company_Registration_API.Models.CompanyApplicant;
using Newtonsoft.Json;

namespace Company_Registration_API.Models.CompanyRegistration
{
    public class ResCompanyRegistration : ResultBase
    {
        [JsonProperty ("data")]
        public RegisteredCompany Data { get; set; }
    }
}