using Company_Registration.Common;
using Company_Registration.Models.DTO;
using Company_Registration.Utils;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Company_Registration.APIServices
{
    public class SystemUserService : ISystemUserService
    {
        private readonly ApiHelpers _apiHelper;
        private readonly ILog _logger;
        public long loginUserId { get; set; }
        public SystemUserService()
        {
            _apiHelper = new ApiHelpers();
            _logger = LogManager.GetLogger(typeof(SystemUserService));
        }
        public async Task<ResponseDto> GetUserById(int id)
        {
            var response = new ResponseDto();

            try
            {
                var reqDto = ModelConverter.CreateRequestDto(
                    null,
                    ApiHelpers.BaseUrl,
                    $"api/SystemUser/CreateUser/{id}",
                    eHTTPRequestType.GET
                );

                response = await _apiHelper.SendRequestAsync(reqDto);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Result.Message = ex.Message;
            }

            return response;
        }

        public async Task<ResponseDto> CreateUpdateSystemUser(int id, CreateUpdateUserDto dto)
        {
            var response = new ResponseDto();
            try
            {
                // Validate input
                if (dto == null)
                {
                    response.IsSuccess = false;
                    response.Result.Message = "Input data is required.";
                    return response;
                }

                if (string.IsNullOrEmpty(dto.UserName))
                {
                    response.IsSuccess = false;
                    response.Result.Message = "UserName is required.";
                    return response;
                }

                if (!dto.IsUpdate)
                {
                    if (string.IsNullOrEmpty(dto.EmailAddress))
                    {
                        response.IsSuccess = false;
                        response.Result.Message = "Email is required.";
                        return response;
                    }

                    if (string.IsNullOrEmpty(dto.Password))
                    {
                        response.IsSuccess = false;
                        response.Result.Message = "Password is required.";
                        return response;
                    }
                }

                // Create request DTO
                var reqDto = ModelConverter.CreateRequestDto(
                    dto,
                    ApiHelpers.BaseUrl,
                    $"api/SystemUser/CreateUser",
                    eHTTPRequestType.POST);

                // Send API request
                return await _apiHelper.SendRequestAsync(reqDto);
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

        }

        public async Task<ResponseDto> GetAllUsers()
        {
            var response = new ResponseDto();
            try
            {
                var reqDto = ModelConverter.CreateRequestDto(
                    null, // GET request has no body
                    ApiHelpers.BaseUrl,
                    "api/SystemUser/GetAllUsers",
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
        public async Task<ResponseDto> DeleteUser(long userId)
        {
            var response = new ResponseDto();
            try
            {
                
                // Create request DTO for DELETE
                var reqDto = ModelConverter.CreateRequestDto(
                    new {  userId }, // body or parameters if your API expects JSON
                    ApiHelpers.BaseUrl,
                    $"api/SystemUser/DeleteUser/"+userId,
                    eHTTPRequestType.POST // using POST for deletion to include body
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
    }

}
