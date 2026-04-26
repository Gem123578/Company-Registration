using Company_Registration_API.DataAccess;
using Company_Registration_API.Models;
using Company_Registration_API.Models.CompanyRegistration;
using Company_Registration_API.Models.CompanyRegistration.Response;
using Company_Registration_API.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Services
{
    public class CompanyRegistrationService : ICompanyRegistrationService
    {
        private readonly CompanyRegistrationDao _dao;
        public CompanyRegistrationService()
        { 
            _dao = new CompanyRegistrationDao();
        }
        public ResCompanyRegistration SubmitCompanyRegistration(CompanyRegistrationDTO dto)
        {
            var response = new ResCompanyRegistration();

            if (dto == null)
            {
                response.IsSuccess = false;
                response.Message = "Registration data is null.";
                return response;
            }

            //if (dto.ApplicantId == 0)
            //{
            //    response.IsSuccess = false;
            //    response.Message = "ApplicantId missing.";
            //    return response;
            //}

            var companyId = _dao.CreateCompanyRegistration(dto);

            response.IsSuccess = true;
            response.Message = "Company registration submitted successfully.";
            return response;

        }
        // GET ALL
        public List<CompanyRegistrationDTO> GetAllCompanies()
        {
            try
            {
                return _dao.GetAll();
            }
            catch (Exception)
            {
                throw new ApiException("Failed to retrieve companies");
            }
        }

        //  GET BY ID
        public CompanyRegistrationDTO GetCompanyById(long id)
        {
            try
            {
                var company = _dao.GetById(id);

                if (company == null)
                {
                    throw new ApiException("Company not found");
                }

                return company;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ApiException("Failed to retrieve company");
            }
        }

        //  UPDATE
        public RegisteredCompany UpdateCompany(long id, CompanyRegistrationDTO dto)
        {
            try
            {
                return _dao.UpdateCompany(id, dto);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ApiException("Failed to update company");
            }
        }

        //  DELETE
        public bool DeleteCompany(long id)
        {
            try
            {
                _dao.DeleteCompany(id);
                return true;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ApiException("Failed to delete company");
            }
        }

        public UploadResponse UploadConstitution()
        {
            var response = new UploadResponse();

            try
            {
                var request = HttpContext.Current.Request;

                if (request.Files.Count == 0)
                {
                    response.IsSuccess = false;
                    response.Message = "No file uploaded";
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

                response.IsSuccess = true;
                response.Message = "File uploaded successfully";
                response.Path = dbPath;

                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }
    }
}