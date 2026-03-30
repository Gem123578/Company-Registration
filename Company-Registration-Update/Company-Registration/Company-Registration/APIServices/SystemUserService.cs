using Company_Registration.Common;
using Company_Registration.Models.DTO;
using Company_Registration.Utils;
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

        public SystemUserService()
        {
            _apiHelper = new ApiHelpers();
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
                    response.Message = "Input data is required.";
                    return response;
                }

                if (string.IsNullOrEmpty(dto.UserName))
                {
                    response.IsSuccess = false;
                    response.Message = "UserName is required.";
                    return response;
                }

                if (!dto.IsUpdate)
                {
                    if (string.IsNullOrEmpty(dto.EmailAddress))
                    {
                        response.IsSuccess = false;
                        response.Message = "Email is required.";
                        return response;
                    }

                    if (string.IsNullOrEmpty(dto.Password))
                    {
                        response.IsSuccess = false;
                        response.Message = "Password is required.";
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
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
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
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }

    }
}