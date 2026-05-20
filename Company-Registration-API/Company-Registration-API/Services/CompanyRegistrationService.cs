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
        private readonly CompanyRegistrationDao _dao;
        private readonly ILog _logger;
        public CompanyRegistrationService()
        { 
            _dao = new CompanyRegistrationDao();
            _logger = LogManager.GetLogger(typeof(CompanyApplicants));
        }
        public ResCompanyRegistration SubmitCompanyRegistration(CompanyRegistrationDTO dto)
        {
            var response = new ResCompanyRegistration();


            var companyId = _dao.CreateCompanyRegistration(dto);
            response.Result = CreateResult(Constants.ACK_Result, string.Format(CommonMessages.MSG_COMREG_SUCCES));
            return response;

        }
        // GET ALL
        public ResGetAll GetAllCompanies(long userId)
        {
            ResGetAll response = new ResGetAll();
            try
            {
                response.Data = _dao.GetAll(userId);
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
        public ResCompanyId GetCompanyById(long id)
        {
            ResCompanyId response = new ResCompanyId();
            try
            {
                var company = _dao.GetById(id);

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
        public ResCompanyRegistration UpdateCompany(long id, CompanyRegistrationDTO dto)
        {
            ResCompanyRegistration response = new ResCompanyRegistration();
            try
            {
                var company = _dao.UpdateCompany(id, dto);

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
            ResDeleteCompany response = new ResDeleteCompany();
            try
            {
                _dao.DeleteCompany(id);
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