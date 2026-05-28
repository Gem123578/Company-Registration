using Company_Registration_API.DataAccess;
using Company_Registration_API.Models;
using Company_Registration_API.Models.CompanyApplicant;
using Company_Registration_API.Models.DTO;
using Company_Registration_API.Utils;
using log4net;
using System;
using System.Configuration;

namespace Company_Registration_API.Services
{
    public class CompanyApplicantService : BaseServices ,ICompanyApplicantService 
    {
        private readonly ApplicantRegistrationDao _applicantDao;
        private readonly string baseUrl;
        private readonly ILog _logger;

        public CompanyApplicantService()
        {
            _applicantDao = new ApplicantRegistrationDao();
            baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            _logger = LogManager.GetLogger(typeof(CompanyApplicants));
        }

        public RegisterResponse Register(ApplicantRegisterDTO dto)
        {
            var response = new RegisterResponse();
            try
            {
                // Check existing applicant
                var existingApplicant =
                    _applicantDao.IsEmailExist(dto.EmailAddress);

                // Validate NRC & Phone only for new applicant
                if (existingApplicant == null)
                {
                    _applicantDao.ValidateIdentityNumber(dto.IdentityNumber);

                    _applicantDao.ValidatePhoneNumber(dto.PhoneNumber);
                }

                CompanyApplicantDto applicant;
                string tokenString;

                // Existing but unconfirmed applicant
                if (existingApplicant != null)
                {
                    var updatedResult =_applicantDao.UpdateUnconfirmedApplicant(existingApplicant,dto);

                    applicant = updatedResult.applicant;
                    tokenString = updatedResult.token;
                }
                else
                {
                    // New applicant
                    var result = _applicantDao.CreateApplicant(dto);

                    applicant = result.applicant;
                    tokenString = result.token;
                }

                // confirmation link
                string confirmLink = $"{baseUrl}/Companyapplicant/ConfirmEmail?token={tokenString}&email={dto.EmailAddress}";

                // send email
                EmailHelper.SendConfirmationEmail(dto.EmailAddress, confirmLink);
                response.Result = CreateResult(Constants.ACK_Result);
                response.Result.Message = CommonMessages.MSG_NEED_EMAILCONFIRM;
                response.Data = applicant;
                
            }
            catch (Exception ex)
            {
                response.Result = CreateResult(Constants.NACK_Result, ex.Message);
                _logger.Error(null, ex);

            }
            return response;
        }
        
        public ResultBase ConfirmEmail(string token, string email)
        {
            var response = new ResultBase();
            try
            {
                ModalValidator.ValidateToken(token);
                ModalValidator.ValidateEmail(email);
                var result = _applicantDao.ConfirmEmail(token, email);

                response.Result = CreateResult(Constants.ACK_Result , CommonMessages.MSG_MAIL_CONFIRM);
               
            }
            catch (Exception ex)
            {
                response.Result = CreateResult(Constants.NACK_Result, ex.Message);
                _logger.Error(null, ex);

            }
            return response;
        }
        public ResultBase ResendConfirmationEmail(string email)
        {
            var response = new ResultBase();

            try
            {
                ModalValidator.ValidateEmail(email);

                string token = _applicantDao.ResendConfirmationEmail(email);

                string confirmLink =
                    $"{baseUrl}/Companyapplicant/ConfirmEmail?token={token}&email={email}";

                EmailHelper.SendConfirmationEmail(email, confirmLink);

                response.Result = CreateResult(
                    Constants.ACK_Result,
                    "Confirmation email resend successfully.");
            }
            catch (Exception ex)
            {
                response.Result = CreateResult(Constants.NACK_Result,ex.Message);

                _logger.Error(null, ex);
            }

            return response;
        }
    }
}