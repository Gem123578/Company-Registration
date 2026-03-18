using Company_Registration_API.DataAccess;
using Company_Registration_API.Models;
using Company_Registration_API.Models.CompanyApplicant;
using Company_Registration_API.Models.CompanyApplicant.Request;
using Company_Registration_API.Models.DTO;
using Company_Registration_API.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Services
{
    public class CompanyApplicantService : ICompanyApplicantService
    {
        private readonly ApplicantRegistrationDao _dao;

        public CompanyApplicantService(ApplicantRegistrationDao dao)
        {
            _dao = dao;
        }

        public RegisterResponse Register(ApplicantRegisterDTO dto)
        {
            var response = new RegisterResponse();
            

            if (_dao.IsEmailExist(dto.EmailAddress))
            {
                response.Success = false;
                response.Message = "Email already exists";
                return response;
            }
            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new Exception("Password is required");
            var token = Guid.NewGuid().ToString(); // confirmation token

            var applicantDto = _dao.CreateApplicant(dto, token);
            // confirmation link
            string confirmLink = $"https://localhost:44378/api/companyapplicants/confirm-email?token={token}&email={dto.EmailAddress}";

            // send email
            EmailHelper.SendConfirmationEmail(dto.EmailAddress, confirmLink);


            response.Message = "Registration successful";
            response.Success = true;
            response.Data = applicantDto;

            return response;
        }

        public LoginResponse Login(ApplicantLoginDTO dto)
        {
            var response = new LoginResponse();
            var applicantDto = _dao.Login(dto);

            if (applicantDto == null)
            {
                response.Success = false;
                response.Message = "Invalid email or password";
                return response;
            }
            if (!applicantDto.EmailConfirmed)
            {
                response.Success = false;
                response.Message = "Please confirm your email before login";
                return response;
            }


            response.Success = true;
            response.Message = "Login successful";
            response.Data = applicantDto;

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