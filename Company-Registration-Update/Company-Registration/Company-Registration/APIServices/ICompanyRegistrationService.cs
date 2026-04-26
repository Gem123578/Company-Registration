using Company_Registration.Common;
using Company_Registration_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Company_Registration.APIServices
{
    public interface ICompanyRegistrationService
    {
        Task<ResponseDto> GetAllCompanies();
        Task<ResponseDto> GetCompanyById(long id);
        Task<ResponseDto> UpdateCompany(long id, CompanyRegistrationDTO request);
        Task<ResponseDto> DeleteCompany(long id);
        Task<ResponseDto> SubmitRegistration(CompanyRegistrationDTO request);
        Task<string> UploadFile(HttpPostedFileBase file);
    }
}