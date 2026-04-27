using Company_Registration_API.DataAccess;
using Company_Registration_API.Models;
using Company_Registration_API.Models.CompanyApplicant;
using Company_Registration_API.Models.DTO;
using Company_Registration_API.Models.SystemUser;
using Company_Registration_API.Utils;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Services
{
    public class SystemUserService : BaseServices,ISystemUserService
    {
        private readonly SystemUsersDao _dao;
        private readonly ILog _logger;

        public SystemUserService()
        {
            _dao = new SystemUsersDao();
            _logger = LogManager.GetLogger(typeof(SystemUserService));
        }

        public ResGetAllSystemUsers GetAllSystemUsers()
        {
            var response = new ResGetAllSystemUsers();

            try
            {
                response.Data = _dao.GetAllSystemUsers();
                response.Result = CreateResult(Constants.ACK_Result);
            }
            catch (Exception ex)
            {
                _logger.Error(null, ex);
                response.Result = CreateResult(Constants.NACK_Result, ex.Message);
            }
            finally
            {
                _logger.DebugFormat(CommonConstants.API_END, DateTime.UtcNow);
            }

            return response;
        }

        public ResRegSystemUser CreateUpdateSystemUser(long id, CreateUserDto dto)
        {
            var response = new ResRegSystemUser();

            try
            {
                if (dto == null)
                    throw new Exception("Input data is required.");

                if (string.IsNullOrEmpty(dto.UserName))
                    throw new Exception("UserName is required.");

                if (!dto.IsUpdate)
                {
                    if (string.IsNullOrEmpty(dto.EmailAddress))
                        throw new Exception("Email is required.");

                    if (string.IsNullOrEmpty(dto.Password))
                        throw new Exception("Password is required.");
                }

                dto = _dao.CreateUpdateSystemUser(id, dto);

                response.Data = dto;
                response.Result = CreateResult(Constants.ACK_Result,
                    dto.IsUpdate ? "User updated successfully." : "User created successfully.");
            }
            catch (Exception ex)
            {
                _logger.Error(null, ex);
                response.Result = CreateResult(Constants.NACK_Result, ex.Message);
            }
            finally
            {
                _logger.DebugFormat(CommonConstants.API_END, DateTime.UtcNow);
            }

            return response;
        }

        public ResRegSystemUser GetSystemUserById(long id)
        {
            var response = new ResRegSystemUser();

            try
            {
                if (id <= 0)
                    throw new Exception("Invalid user ID");

                response.Data = _dao.GetUserById(id);
                response.Result = CreateResult(Constants.ACK_Result);
            }
            catch (Exception ex)
            {
                _logger.Error(null, ex);
                response.Result = CreateResult(Constants.NACK_Result, ex.Message);
            }
            finally
            {
                _logger.DebugFormat(CommonConstants.API_END, DateTime.UtcNow);
            }

            return response;
        }

        public ResultBase DeleteUser(long id)
        {
            var response = new ResultBase();

            try
            {
                if (id <= 0)
                    throw new Exception("Valid user ID is required.");

                _dao.DeleteUser(id);

                response.Result = CreateResult(Constants.ACK_Result, "User deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.Error(null, ex);
                response.Result = CreateResult(Constants.NACK_Result, ex.Message);
            }
            finally
            {
                _logger.DebugFormat(CommonConstants.API_END, DateTime.UtcNow);
            }

            return response;
        }

        public ResLoginSystemUser ValidateUser(LoginDTO dto)
        {
            var response = new ResLoginSystemUser();

            try
            {
                if (dto == null)
                    throw new Exception("Request is null.");

                if (string.IsNullOrEmpty(dto.EmailAddress))
                    throw new Exception("Email is required.");

                if (string.IsNullOrEmpty(dto.Password))
                    throw new Exception("Password is required.");

                var user = _dao.ValidateUser(dto);

                if (user == null)
                    throw new Exception("Invalid email or password.");

                if (user.UserRole == "APPLICANT" && !user.EmailConfirmed)
                    throw new Exception("Please confirm your email before login.");

                response.Data = user;
                response.Result = CreateResult(Constants.ACK_Result, "Login successful.");
            }
            catch (Exception ex)
            {
                _logger.Error(null, ex);
                response.Result = CreateResult(Constants.NACK_Result, ex.Message);
            }
            finally
            {
                _logger.DebugFormat(CommonConstants.API_END, DateTime.UtcNow);
            }

            return response;
        }
    }
}
