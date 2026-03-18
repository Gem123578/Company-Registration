using Company_Registration_API.Models;
using Company_Registration_API.Models.CompanyRegistration;
using Company_Registration_API.Models.CompanyRegistration.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Services
{
    public interface ICompanyRegistrationService
    {
        ResCompanyRegistration SubmitCompanyRegistration(CompanyRegistrationDTO dto);
        UploadResponse UploadConstitution();
    }
}