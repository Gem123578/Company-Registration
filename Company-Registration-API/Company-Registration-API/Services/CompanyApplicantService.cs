using Company_Registration_API.DataAccess;
using Company_Registration_API.Models;
using Company_Registration_API.Models.CompanyApplicant;
using Company_Registration_API.Models.DTO;
using Company_Registration_API.Utils;
using log4net;
using QPSOS.Web.API.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Transactions;
using System.Web;

namespace Company_Registration_API.Services
{
    public class CompanyApplicantService : BaseServices ,ICompanyApplicantService
    {
        private readonly ApplicantRegistrationDao _dao;
        private readonly string baseUrl;
        private readonly ILog _logger;

        public CompanyApplicantService()
        {
            _dao = new ApplicantRegistrationDao();
            baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            _logger = LogManager.GetLogger(typeof(CompanyApplicants));
        }

        public RegisterResponse Register(ApplicantRegisterDTO dto)
        {
            var response = new RegisterResponse();
            try
            {
                _dao.IsEmailExist(dto.EmailAddress);
                _dao.ValidateIdentityNumber(dto.IdentityNumber);
                _dao.ValidatePhoneNumber(dto.PhoneNumber);
                //all check

                var applicant = _dao.CreateApplicant(dto);
                var tokenString = _dao.CreateEmailToken(applicant.Id);
                _dao.ValidateToken(tokenString);
                
                // confirmation link
                string confirmLink = $"{baseUrl}/api/companyapplicants/confirm-email?token={tokenString}&email={dto.EmailAddress}";

                // send email
                EmailHelper.SendConfirmationEmail(dto.EmailAddress, confirmLink);
                response.Result = CreateResult(Constants.ACK_Result);
                response.Data = applicant;

            }
            catch (Exception ex)
            {
                response.Result = CreateResult(Constants.NACK_Result, ex.Message);
                _logger.Error(null, ex);
                
            }
            return response;

        }
        public BaseResponse ConfirmEmail(string token, string email)
        {
            var response = new BaseResponse();

            var result = _dao.ConfirmEmail(token, email);

            if (!result)
            {
                response.IsSuccess = false;
                response.Message = "Invalid confirmation link";
                return response;
            }

            response.IsSuccess = true;
            response.Message = "Email confirmed successfully. You can login now.";

            return response;
        }

        //public ResGetAllApplicants GetAllApplicants()
        //{
        //    var response = new ResGetAllApplicants();

        //    response.Data = _dao.GetAllApplicants();

        //    return response;
        //}
    }
}