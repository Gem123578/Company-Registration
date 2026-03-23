using Company_Registration_API.DataAccess;
using Company_Registration_API.Models;
using Company_Registration_API.Models.DTO;
using Company_Registration_API.Models.SystemUser;
using Company_Registration_API.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Services
{
    public class SystemUserService : ISystemUserService
    {
        private readonly SystemUsersDao _dao;
        public SystemUserService(SystemUsersDao dao)
        {
            _dao = dao;
        }

        public ResSystemUser CreateUpdateSystemUser(long id, CreateUserDto dto)
        {
            var response = new ResSystemUser();
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

                //call Dao
                dto = _dao.CreateUpdateSystemUser(id, dto);
                response.IsSuccess = true;
                response.Message = dto.IsUpdate
                    ? "User updated successfully."
                    : "User created successfully.";
                response.Data = dto;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public BaseResponse DeleteUser(long loginUserId, long id)
        {
            var response = new BaseResponse();
            try
            {
                // Validate input
                if (id <= 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Valid user ID is required.";
                    return response;
                }
                //call Dao
                _dao.DeleteUser(loginUserId, id);
                response.IsSuccess = true;
                response.Message = "User deleted successfully.";
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                return response;
            }
        }

        public ResLoginSystemUser ValidateUser(LoginDTO dto)
        {
            var response = new ResLoginSystemUser();
            try
            {
                if (dto == null)
                {
                    response.Success = false;
                    response.Message = "Request is null.";
                    return response;
                }

                if (string.IsNullOrEmpty(dto.EmailAddress))
                {
                    response.Success = false;
                    response.Message = "Email is required.";
                    return response;
                }

                if (string.IsNullOrEmpty(dto.Password))
                {
                    response.Success = false;
                    response.Message = "Password is required.";
                    return response;
                }
                 var user = _dao.ValidateUser(dto);
                response.Success = true;
                response.Message = "Login successful.";
                response.Data = user;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }
        }
    }
}