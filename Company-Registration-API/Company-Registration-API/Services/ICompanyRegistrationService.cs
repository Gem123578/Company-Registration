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
        //ResCompanyRegistration SubmitCompanyRegistration(CompanyRegistrationDTO dto);
        //ResGetAll GetAllCompanies();
        //ResCompanyId GetCompanyById(long id);
        ResCompanyRegistration UpdateCompany(long id, CompanyRegistrationDTO dto);
        ResDeleteCompany DeleteCompany(long id);
        UploadResponse UploadConstitution();
        ResCompanyRegistration SubmitCompanyRegistration(CompanyRegistrationDTO dto);

        ResGetAll GetAllCompanies(long userId);

        ResCompanyId GetCompanyById(long id);
    }
}