using Company_Registration.Common;
using Company_Registration.Models.DTO;
using Company_Registration.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Company_Registration.APIServices
{
    public class CompanyApplicantService : ICompanyApplicantService
    {
        private readonly ApiHelpers _apiHelper;

        public CompanyApplicantService(ApiHelpers apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<ResponseDto> RegisterUser(ApplicantRegisterDTO request)
        {
            var reqDto = ModelConverter.CreateRequestDto(request, ApiHelpers.BaseUrl, "api/CompanyApplicants/Register", eHTTPRequestType.POST);
            return await _apiHelper.SendRequestAsync(reqDto);
        }

        public async Task<ResponseDto> LoginUser(ApplicantLoginDTO request)
        {
            var reqDto = ModelConverter.CreateRequestDto(request, ApiHelpers.BaseUrl, "api/CompanyApplicants/Login", eHTTPRequestType.POST);
            return await _apiHelper.SendRequestAsync(reqDto);
        }
        public async Task<ResponseDto> ConfirmEmail(string token, string email)
        {
            var url = $"api/CompanyApplicants/confirm-email?token={token}&email={email}";

            var reqDto = ModelConverter.CreateRequestDto(
                null,
                ApiHelpers.BaseUrl,
                url,
                eHTTPRequestType.GET
            );

            return await _apiHelper.SendRequestAsync(reqDto);
        }
    }
}
