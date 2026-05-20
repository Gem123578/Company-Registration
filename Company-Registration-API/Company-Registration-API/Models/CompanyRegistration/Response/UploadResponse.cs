using Company_Registration_API.Models.CompanyApplicant;

namespace Company_Registration_API.Models.CompanyRegistration.Response
{
    public class UploadResponse : ResultBase  
    {
        public string Path { get; set; }
    }
}