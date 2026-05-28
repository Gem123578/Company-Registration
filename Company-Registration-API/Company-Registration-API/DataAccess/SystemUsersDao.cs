using Company_Registration_API.Models;
using Company_Registration_API.Models.CompanyApplicant;
using Company_Registration_API.Models.DTO;
using Company_Registration_API.Utils;
using log4net;
using Microsoft.AspNet.Identity;
using QPSOS.Web.API.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Company_Registration_API.DataAccess
{
    public class SystemUsersDao : BaseDao
    {
        private readonly ApplicantDbContext db;
        private readonly ILog _logger;

        public SystemUsersDao()
        {
            db = new ApplicantDbContext();
            _logger = LogManager.GetLogger(typeof(SystemUsersDao));
        }

        internal List<CreateUserDto> GetAllSystemUsers()
        {
            try
            {
                var users = (from su in db.SystemUsers join u in db.Users
                            on su.Id equals u.SystemId
                            join r in db.Roles
                            on u.RoleId equals r.Id
                            where su.AccountStatus 
                             select new CreateUserDto
                             {
                                 Id = su.Id,
                                 UserName = su.UserName,
                                 EmailAddress = su.EmailAddress,
                                 RoleId = u.RoleId,
                                 RoleName = r.RoleName,
                                 AccountStatus = su.AccountStatus
                             }).ToList();

                return users;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(null, ex);
                throw new ApiException(string.Format(CommonMessages.MSG_READ_FAIL, CommonConstants.TBLNAME_USERS));
            }
        }


        public CreateUserDto CreateUpdateSystemUser(long id, CreateUserDto userDto)
        {
            try
            {
               
                    SystemUsers sysuser = new SystemUsers();
                    Users user = new Users();
                    PasswordHasher hasher = new PasswordHasher();
                    //Update case
                    if (userDto.IsUpdate)
                    {
                        sysuser = db.SystemUsers.FirstOrDefault(u => u.Id == id);
                        if (sysuser == null)
                        {
                            throw new ApiException(string.Format(CommonMessages.User_NOT_FOUND, CommonConstants.TBLNAME_USERS));
                        }
                    }

                    //common fields for both create and update
                    sysuser.UserName = userDto.UserName;
                    user.RoleId = userDto.RoleId;
                if (userDto.AccountStatus.HasValue)
                {
                    sysuser.AccountStatus = userDto.AccountStatus.Value;
                }

                //update login
                sysuser.LastLoginAt = DateTime.UtcNow;

                    //create case
                    if (!userDto.IsUpdate)
                    {
                    using (TransactionScope scope = GetReadUncommittedScope())
                    {
                        sysuser.EmailAddress = userDto.EmailAddress;
                        sysuser.PasswordHash = hasher.HashPassword(userDto.Password);
                        sysuser.CreatedAt = DateTime.UtcNow;
                        sysuser.AccountStatus = userDto.AccountStatus ?? true;
                        db.SystemUsers.Add(sysuser);
                        user = new Users
                        {
                            ApplicantId = 0, // system user
                            SystemId = user.Id,
                            RoleId = user.RoleId,
                            IsUser = true,
                            CreatedAt = DateTime.UtcNow
                        };
                        db.Users.Add(user);
                        db.SaveChanges();
                        scope.Complete();
                    }
                }
               
                return userDto;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(null, ex);
                throw new ApiException(string.Format(CommonMessages.MSG_WRITE_FAIL, CommonConstants.TBLNAME_USERS));
            }
        }
        public CreateUserDto GetUserById(long id)
        {
            try
            {
                // Example using Entity Framework or in-memory
                var sysuser = db.SystemUsers.FirstOrDefault(u => u.Id == id);
                Users user = db.Users.FirstOrDefault(x => x.SystemId == sysuser.Id);
                if (user == null) return null;

                return new CreateUserDto
                {
                    Id = sysuser.Id,
                    UserName = sysuser.UserName,
                    EmailAddress = sysuser.EmailAddress,
                    RoleId = user.RoleId,
                    IsUpdate = false
                };
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(null, ex);
                throw new ApiException(string.Format(CommonMessages.MSG_WRITE_FAIL, CommonConstants.TBLNAME_USERS));
            }
            
        }


        internal void DeleteUser(long id)
        {
            try
            {
                var user = db.SystemUsers.Where(u => u.Id == id).FirstOrDefault();
                if (user == null)
                {
                    throw new ApiException(string.Format(CommonMessages.User_NOT_FOUND, CommonConstants.TBLNAME_USERS));
                }

                //Prevent self delete
                if (user.Id == 0)
                {
                    throw new ApiException("Users cannot delete themselves.");
                }
                //delete with account status
                user.AccountStatus = false;
                //audit field for delete
                user.LastLoginAt = DateTime.UtcNow;
                db.SaveChanges();
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(null, ex);
                throw new ApiException(string.Format(CommonMessages.MSG_Delete_FAIL, CommonConstants.TBLNAME_USERS));
            }
        }


        internal LoginUserDto ValidateUser(LoginDTO dto)
        {
            LoginResponse response = new LoginResponse();
            try
            {
                var hasher = new PasswordHasher();

                // Check SystemUsers first (JOIN with Roles)
                var sysUser = db.SystemUsers.FirstOrDefault(x => x.EmailAddress == dto.EmailAddress);

                if (sysUser != null)
                {
                    var result = hasher.VerifyHashedPassword(sysUser.PasswordHash,dto.Password);

                    if (result != PasswordVerificationResult.Success)
                    {
                        throw new ApiException(
                            string.Format(CommonMessages.MSG_INVALID_PASS, CommonConstants.TBLNAME_USERS)
                        );
                    }

                    if (!sysUser.AccountStatus)
                    {
                        throw new ApiException(string.Format(CommonMessages.MSG_DISABLE_ACC, CommonConstants.TBLNAME_USERS));
                    }

                    var user = db.Users.FirstOrDefault(x => x.SystemId == sysUser.Id);
                    
                    var role = db.Roles.FirstOrDefault(x => x.Id == user.RoleId);
                    var functions = (
                        from rf in db.RolesFunctions
                        join f in db.Functions
                            on rf.FunctionId equals f.Id
                        where rf.RoleId == user.RoleId
                        select f.FunctionName
                    ).ToList();

                    return new LoginUserDto
                    {
                        UserId = sysUser.Id,
                        UserName = sysUser.UserName,
                        UserRole = role.RoleName,
                        Functions = functions
                    };
                }

                // Check Applicant
                var applicant = db.CompanyApplicants.FirstOrDefault(x => x.EmailAddress == dto.EmailAddress);

                if (applicant != null)
                {
                    var result = hasher.VerifyHashedPassword(applicant.PasswordHash,dto.Password);

                    if (result != PasswordVerificationResult.Success)
                    {
                        throw new ApiException(
                            string.Format(CommonMessages.MSG_INVALID_PASS, CommonConstants.TBLNAME_USERS)
                        );
                    }

                    if (!applicant.EmailConfirmed)
                    {
                        _logger.Error(CommonMessages.MSG_NEED_EMAILCONFIRM);
                        throw new ApiException(string.Format(CommonMessages.MSG_NEED_EMAILCONFIRM, CommonConstants.TBLNAME_APP_USERS));
                    }

                    var user = db.Users.FirstOrDefault(x => x.ApplicantId == applicant.Id);

                    if (user == null)
                        throw new ApiException("User mapping not found");

                    var role = db.Roles.FirstOrDefault(x => x.Id == user.RoleId);

                    if (role == null)
                        throw new ApiException("Role not found");

                    var functions = (
                        from rf in db.RolesFunctions
                        join f in db.Functions
                            on rf.FunctionId equals f.Id
                        where rf.RoleId == user.RoleId
                        select f.FunctionName
                    ).ToList();

                    return new LoginUserDto
                    {
                        UserId = applicant.Id,
                        UserName = applicant.FullName,
                        UserRole = role.RoleName,
                        Functions = functions
                    };
                }

                // Not found
                throw new ApiException(string.Format(CommonMessages.User_NOT_FOUND, CommonConstants.TBLNAME_USERS));
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw new ApiException(string.Format(CommonMessages.MSG_Login_FAIL, CommonConstants.TBLNAME_USERS));
            }
        }
    }
}