using Company_Registration_API.DataAccess;
using Company_Registration_API.Models;
using Company_Registration_API.Models.CompanyApplicant;
using Company_Registration_API.Models.DTO;
using Company_Registration_API.Models.SystemUser;
using Company_Registration_API.Utils;
using log4net;
using System;

namespace Company_Registration_API.Services
{
    public class SystemUserService : BaseServices,ISystemUserService
    {
        private readonly SystemUsersDao _Userdao;
        private readonly ApplicantRegistrationDao _applicantDao;    
        private readonly ILog _logger;

        public SystemUserService()
        {
            _Userdao = new SystemUsersDao();
            _logger = LogManager.GetLogger(typeof(SystemUserService));
        }

        public ResGetAllSystemUsers GetAllSystemUsers()
        {
            var response = new ResGetAllSystemUsers();

            try
            {
                response.Data = _Userdao.GetAllSystemUsers();
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
                ModalValidator.ValidateCUSystemUser(id, dto);
                dto = _Userdao.CreateUpdateSystemUser(id, dto);
                _applicantDao.IsEmailExist(dto.EmailAddress);

                response.Data = dto;
                response.Result = CreateResult(Constants.ACK_Result,
                    dto.IsUpdate ? CommonMessages.MSG_UUPDATE : CommonMessages.MSG_UCREATE);
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
                ModalValidator.ValidateSystemUserId(id);
                response.Data = _Userdao.GetUserById(id);
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
                ModalValidator.ValidateSystemUserId(id);

                _Userdao.DeleteUser(id);

                response.Result = CreateResult(Constants.ACK_Result, CommonMessages.MSG_USERDELETE);
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
                ModalValidator.ValidateLoginUser(dto);

                var user = _Userdao.ValidateUser(dto);

                response.Data = user;
                response.Result = CreateResult(Constants.ACK_Result, CommonMessages.MSG_LOGINSUC);
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
