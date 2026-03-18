using Company_Registration_API.Models;
using Company_Registration_API.Models.CompanyApplicant;
using Company_Registration_API.Models.CompanyApplicant.Request;
using Company_Registration_API.Models.CompanyRegistration;

namespace Company_Registration_API.Services
{
    public interface ICompanyApplicantService 
    {
        RegisterResponse Register(ApplicantRegisterDTO dto);
        LoginResponse Login(ApplicantLoginDTO dto);
        BaseResponse ConfirmEmail(string token, string email);
        


        /* ResGetAllApplicants GetAllApplicants()*/
    }
}