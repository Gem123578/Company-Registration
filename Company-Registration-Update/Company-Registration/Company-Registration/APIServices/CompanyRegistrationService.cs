using Company_Registration.Common;
using Company_Registration.Utils;
using Company_Registration_API.Models;
using Newtonsoft.Json;
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
        public CompanyRegistrationService(ApiHelpers apiHelper)
        {
            _apiHelper = apiHelper;
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
            if (dto == null)
                return new ResponseDto { IsSuccess = false, Message = "Invalid registration data" };

            var reqDto = ModelConverter.CreateRequestDto(
                dto,
                ApiHelpers.BaseUrl,
                "api/CompanyRegistration/Submit",
                eHTTPRequestType.POST);

            return await _apiHelper.SendRequestAsync(reqDto);
        }
    }
}