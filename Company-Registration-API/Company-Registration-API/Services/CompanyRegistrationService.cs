using Company_Registration_API.DataAccess;
using Company_Registration_API.Models;
using Company_Registration_API.Models.CompanyRegistration;
using Company_Registration_API.Models.CompanyRegistration.Response;
using Company_Registration_API.Utils;
using log4net;
using System;
using System.IO;
using System.Web;

namespace Company_Registration_API.Services
{
    public class CompanyRegistrationService : BaseServices, ICompanyRegistrationService
    {
        private readonly CompanyRegistrationDao _CompanyRegistrationdao;
        private readonly ILog _logger;
        public CompanyRegistrationService()
        {
            _CompanyRegistrationdao = new CompanyRegistrationDao();
            _logger = LogManager.GetLogger(typeof(CompanyApplicants));
        }
        public ResCompanyRegistration SubmitCompanyRegistration(CompanyRegistrationDTO dto)
        {
            var response = new ResCompanyRegistration();
            ModalValidator.ValidateCompanyRegistration(dto);

            var companyId = _CompanyRegistrationdao.CreateCompanyRegistration(dto);

            response.Result = CreateResult(Constants.ACK_Result, string.Format(CommonMessages.MSG_COMREG_SUCCES));
            return response;

        }
        // GET ALL
        public ResGetAll GetAllCompanies(long userId)
        {
            ModalValidator.ValidateUserId(userId);
            ResGetAll response = new ResGetAll();
            try
            {
                response.Data = _CompanyRegistrationdao.GetAll(userId);
                response.Result = CreateResult(Constants.ACK_Result);
            }
            catch (Exception ex)
            {
                _logger.Error(null, ex);
                response.Result = CreateResult(Constants.NACK_Result, ex.Message);
            }

            return response;
        }

        //  GET BY ID
        public ResCompanyId GetCompanyById(long userid)
        {
            ResCompanyId response = new ResCompanyId();
            try
            {
                ModalValidator.ValidateUserId(userid);
                var company = _CompanyRegistrationdao.GetById(userid);

                if (company == null)
                {
                    throw new ApiException("Company not found");
                }

                response.Data = company;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ApiException("Failed to retrieve company");
            }
            return response;
        }

        //  UPDATE
        public ResCompanyRegistration UpdateCompany(long userid, CompanyRegistrationDTO dto)
        {
            ResCompanyRegistration response = new ResCompanyRegistration();
            try
            {
                ModalValidator.ValidateUserId(userid);
                ModalValidator.ValidateCompanyRegistration(dto);
                var company = _CompanyRegistrationdao.UpdateCompany(userid, dto);

                response.Data = company;
                response.Result = CreateResult(Constants.ACK_Result, "Company updated successfully");
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ApiException("Failed to update company");
            }
            return response;
        }

        //  DELETE
        public ResDeleteCompany DeleteCompany(long id)
        {
            ModalValidator.ValidateUserId(id);
            ResDeleteCompany response = new ResDeleteCompany();
            try
            {
                _CompanyRegistrationdao.DeleteCompany(id);
                response.Result = CreateResult(Constants.ACK_Result, string.Format(CommonMessages.MSG_DELETE));
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ApiException("Failed to delete company");
            }
            return response;
        }

        public UploadResponse UploadConstitution()
        {
            var response = new UploadResponse();

            try
            {
                var request = HttpContext.Current.Request;

                if (request.Files.Count == 0)
                {
                    
                    response.Result.Message = "No file uploaded";
                    return response;
                }

                var file = request.Files[0];

                string folder = HttpContext.Current.Server.MapPath("~/Uploads/");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                string fullPath = Path.Combine(folder, fileName);

                file.SaveAs(fullPath);

                string dbPath = "/Uploads/" + fileName;
                response.Path = dbPath;
                response.Result = CreateResult(Constants.ACK_Result, "File uploaded successfully");
                

                return response;
            }
            catch (Exception ex)
            {
                response.Result.Code = Constants.NACK_Result;
                response.Result.Message = ex.Message;
                return response;
            }
        }
    }
}