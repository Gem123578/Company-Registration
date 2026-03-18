using Company_Registration.Common;
using Company_Registration.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Company_Registration.APIServices
{
    public interface ICompanyApplicantService
    {
        Task<ResponseDto> RegisterUser(ApplicantRegisterDTO request);
        Task<ResponseDto> LoginUser(ApplicantLoginDTO request);
        Task<ResponseDto> ConfirmEmail(string token, string email);

    }
}