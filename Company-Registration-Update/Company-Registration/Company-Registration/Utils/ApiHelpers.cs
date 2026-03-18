using Company_Registration.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebGrease.Css.Ast;

namespace Company_Registration.Utils
{
    public class ApiHelpers : IAPIAccessHelper
    {
        public static readonly string BaseUrl = "https://localhost:44378/";
        public static Uri BaseUri => new Uri(BaseUrl);

        public async Task<ResponseDto> SendRequestAsync(RequestDto request)
        {
            ResponseDto responseDto = new ResponseDto();

            try
            {
                // Use CookieContainer for session cookies
                var cookieContainer = new CookieContainer();
                var handler = new HttpClientHandler
                {
                    CookieContainer = cookieContainer,
                    UseCookies = true
                };

                using (var client = new HttpClient(handler) { Timeout = TimeSpan.FromMinutes(2) })
                {
                    // Optional: attach session cookie manually
                    if (!string.IsNullOrEmpty(request.AccessToken))
                    {
                        cookieContainer.Add(BaseUri, new Cookie("ASP.NET_SessionId", request.AccessToken));
                    }

                    var requestMessage = new HttpRequestMessage
                    {
                        RequestUri = new Uri(request.RequestUrl),
                        Method = HttpMethod.Get
                    };

                    // Set HTTP method
                    switch (request.RequestType)
                    {
                        case eHTTPRequestType.POST:
                            requestMessage.Method = HttpMethod.Post;
                            break;
                        case eHTTPRequestType.PUT:
                            requestMessage.Method = HttpMethod.Put;
                            break;
                        case eHTTPRequestType.DELETE:
                            requestMessage.Method = HttpMethod.Delete;
                            break;
                        default:
                            requestMessage.Method = HttpMethod.Get;
                            break;
                    }

                    // Add JSON content if data exists
                    if (request.Data != null)
                    {
                        string jsonData = JsonConvert.SerializeObject(request.Data);
                        requestMessage.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    }

                    // Add extra headers
                    if (request.Headers != null && request.Headers.Count > 0)
                    {
                        foreach (var header in request.Headers)
                        {
                            requestMessage.Headers.Add(header.Key, header.Value);
                        }
                    }

                    // Send request
                    HttpResponseMessage responseMessage = await client.SendAsync(requestMessage).ConfigureAwait(false);

                    var apiContent = await responseMessage.Content.ReadAsStringAsync();
                    responseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

                    responseDto.StatusCode = responseMessage.StatusCode;
                    responseDto.IsSuccess = responseMessage.IsSuccessStatusCode;

                    if (!responseDto.IsSuccess)
                    {
                        string errorMessage = GenerateErrorMessage(apiContent, responseMessage, ref responseDto);
                        throw new ApiException(errorMessage);
                    }
                }
            }
            catch (ApiException ex)
            {
                responseDto.IsSuccess = false;
                GetResponseErrorInfo(ref responseDto, ex.Message);
            }
            catch (Exception ex)
            {
                responseDto.IsSuccess = false;
                GetResponseErrorInfo(ref responseDto, ex.Message);
            }

            return responseDto;
        }

        private string GenerateErrorMessage(string apiContent, HttpResponseMessage responseMessage, ref ResponseDto responseDto)
        {
            responseDto.StatusCode = responseMessage.StatusCode;

            string errorMessage = string.IsNullOrEmpty(responseDto.Message)? responseMessage.StatusCode.ToString(): responseDto.Message;
            return errorMessage;
        }

        private void GetResponseErrorInfo(ref ResponseDto response, string errorMessage)
        {
            response.IsSuccess = false;
            response.Message = errorMessage;
        }
    }
}