using Company_Registration.Common;
using Company_Registration.Utils;
using Company_Registration_API.Models;
using log4net;
using Newtonsoft.Json;
using QSS.POS.Front.UI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Company_Registration.APIServices
{
    public class CompanyRegistrationService : ICompanyRegistrationService
    {
        private readonly ApiHelpers _apiHelper;
        private readonly ILog _logger;
        public CompanyRegistrationService()
        {
            _apiHelper = new ApiHelpers();
            _logger = LogManager.GetLogger(typeof(CompanyRegistrationService));
        }

        public async Task<ResponseDto> GetAllCompanies(long userId)
        {
            var response = new ResponseDto();

            try
            {
                var reqDto = ModelConverter.CreateRequestDto(
                    null,
                    ApiHelpers.BaseUrl,
                    "api/CompanyRegistration/GetCompany",
                    eHTTPRequestType.GET
                );

                response = await _apiHelper.SendRequestAsync(reqDto);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(null, ex);
                throw;
            }

            return response;
        }
        public async Task<ResponseDto> GetCompanyById(long id)
        {
            var response = new ResponseDto();

            try
            {
                var reqDto = ModelConverter.CreateRequestDto(
                    null,
                    ApiHelpers.BaseUrl,
                    $"api/CompanyRegistration/Get/{id}",
                    eHTTPRequestType.GET
                );

                response = await _apiHelper.SendRequestAsync(reqDto);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(null, ex);
                throw;
            }

            return response;
        }
        public async Task<ResponseDto> UpdateCompany(long id, CompanyRegistrationDTO dto)
        {
            var response = new ResponseDto();

            try
            {
                
                var reqDto = ModelConverter.CreateRequestDto(
                    dto,
                    ApiHelpers.BaseUrl,
                    $"api/CompanyRegistration/Update/{id}",
                    eHTTPRequestType.POST
                );

                response = await _apiHelper.SendRequestAsync(reqDto);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(null, ex);
                throw;
            }

            return response;
        }
        public async Task<ResponseDto> DeleteCompany(long id)
        {
            var response = new ResponseDto();

            try
            {
                var reqDto = ModelConverter.CreateRequestDto(
                    null,
                    ApiHelpers.BaseUrl,
                    $"api/CompanyRegistration/Delete/{id}",
                    eHTTPRequestType.POST
                );

                response = await _apiHelper.SendRequestAsync(reqDto);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(null, ex);
                throw;
            }

            return response;
        }
        // -----------------------------
        // Upload file to API
        // -----------------------------
        public async Task<string> UploadFile(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
                return null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = ApiHelpers.BaseUri;
                client.Timeout = TimeSpan.FromMinutes(5);

                using (var content = new MultipartFormDataContent())
                {
                    var fileContent = new StreamContent(file.InputStream);
                    fileContent.Headers.ContentDisposition =
                        new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                        {
                            Name = "\"file\"",
                            FileName = "\"" + file.FileName + "\""
                        };
                    fileContent.Headers.ContentType =
                        new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                    content.Add(fileContent);

                    var response = await client.PostAsync("api/CompanyRegistration/upload", content);
                    var result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("API response JSON: " + result);

                    if (!response.IsSuccessStatusCode)
                        return null;

                    dynamic data = JsonConvert.DeserializeObject(result);

                    // Support multiple possible property names
                    string filePath = data?.Path ?? data?.path ?? data?.filePath ?? data?.FilePath;
                    return filePath;
                }
            }
        }

        // -----------------------------
        // Submit registration to API
        // -----------------------------
        public async Task<ResponseDto> SubmitRegistration(CompanyRegistrationDTO dto)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                var reqDto = ModelConverter.CreateRequestDto(
                dto,
                ApiHelpers.BaseUrl,
                "api/CompanyRegistration/Submit",
                eHTTPRequestType.POST);

                 response = await _apiHelper.SendRequestAsync(reqDto);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(null, ex);
                throw;
            }
            return response;
            
        }
    }
}