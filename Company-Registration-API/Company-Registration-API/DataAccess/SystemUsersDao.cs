using Company_Registration_API.Models;
using Company_Registration_API.Models.CompanyApplicant;
using Company_Registration_API.Models.DTO;
using Company_Registration_API.Models.SystemUser;
using Company_Registration_API.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Company_Registration_API.DataAccess
{
    public class SystemUsersDao
    {
        private readonly ApplicantDbContext db;

        public SystemUsersDao()
        {
            db = new ApplicantDbContext();
        }

        internal List<CreateUserDto> GetAllSystemUsers()
        {
            try
            {
                var users = db.SystemUsers
                    .Select(u => new CreateUserDto
                    {
                        Id = u.Id,
                        UserName = u.UserName,
                        UserRole = u.UserRole,
                        AccountStatus = u.AccountStatus,
                        EmailAddress = u.EmailAddress
                    })
                    .ToList();

                return users;
            }
            catch (Exception)
            {
                throw new ApiException(string.Format(CommonMessages.MSG_READ_FAIL, CommonConstants.TBLNAME_USERS));
            }
        }


        public CreateUserDto CreateUpdateSystemUser(long id, CreateUserDto dto)
        {
            try
            {
                SystemUsers user = new SystemUsers();
                PasswordHasher hasher = new PasswordHasher();
                //Update case
                if (dto.IsUpdate)
                {
                    user = db.SystemUsers.FirstOrDefault(u => u.Id == id);
                    if (user == null)
                    {
                        throw new ApiException(string.Format(CommonMessages.User_NOT_FOUND, CommonConstants.TBLNAME_USERS));
                    }
                }

                //common fields for both create and update
                user.UserName = dto.UserName;
                user.UserRole = dto.UserRole;
                if (!string.IsNullOrEmpty(dto.AccountStatus))
                {
                    if (dto.AccountStatus != "ACTIVE" && dto.AccountStatus != "DISABLED")
                        throw new ApiException("Invalid AccountStatus. Must be 'ACTIVE' or 'DISABLED'.");
                    user.AccountStatus = dto.AccountStatus;
                }

                //update login
                user.LastLoginAt = DateTime.UtcNow;

                //create case
                if (!dto.IsUpdate)
                {
                    user.EmailAddress = dto.EmailAddress;
                    user.PasswordHash = hasher.Hash(dto.Password);
                    user.CreatedAt = DateTime.UtcNow;
                    user.AccountStatus = string.IsNullOrEmpty(dto.AccountStatus) ? "ACTIVE" : dto.AccountStatus;
                    db.SystemUsers.Add(user);
                }

                db.SaveChanges();
                return dto;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ApiException(string.Format(CommonMessages.MSG_WRITE_FAIL, CommonConstants.TBLNAME_USERS));
            }
        }
        public CreateUserDto GetUserById(long id)
        {
            try
            {
                // Example using Entity Framework or in-memory
                var user = db.SystemUsers.FirstOrDefault(u => u.Id == id);
                if (user == null) return null;

                return new CreateUserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    EmailAddress = user.EmailAddress,
                    UserRole = user.UserRole,
                    IsUpdate = false
                };
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ApiException(string.Format(CommonMessages.MSG_WRITE_FAIL, CommonConstants.TBLNAME_USERS));
            }
            
        }


        internal void DeleteUser(long loginUserId, long id)
        {
            try
            {
                var user = db.SystemUsers.Where(u => u.Id == id).FirstOrDefault();
                if (user == null)
                {
                    throw new ApiException(string.Format(CommonMessages.User_NOT_FOUND, CommonConstants.TBLNAME_USERS));
                }

                //Prevent self delete
                if (user.Id == loginUserId)
                {
                    throw new ApiException("Users cannot delete themselves.");
                }
                //delete with account status
                user.AccountStatus = "Disabled";
                //audit field for delete
                user.LastLoginAt = DateTime.UtcNow;
                db.SaveChanges();
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ApiException(string.Format(CommonMessages.MSG_Delete_FAIL, CommonConstants.TBLNAME_USERS));
            }
        }


        internal LoginUserDto ValidateUser(LoginDTO dto)
        {
            var response = new LoginResponse();
            try
            {
                PasswordHasher hasher = new PasswordHasher();

                // Check SystemUsers first
                var sysUser = db.SystemUsers.FirstOrDefault(x => x.EmailAddress == dto.EmailAddress);

                if (sysUser != null)
                {
                    string hashedPassword = hasher.Hash(dto.Password);

                    if (sysUser.PasswordHash != hashedPassword)
                        throw new ApiException(string.Format(CommonMessages.MSG_INVALID_PASS, CommonConstants.TBLNAME_USERS));

                    if (sysUser.AccountStatus != "ACTIVE")
                    {
                        throw new ApiException(string.Format(CommonMessages.MSG_DISABLE_ACC, CommonConstants.TBLNAME_USERS));
                    }

                    return new LoginUserDto
                    {
                        UserId = sysUser.Id,
                        UserName = sysUser.UserName,
                        UserRole = sysUser.UserRole
                    };
                }

                // Check Applicant
                var applicant = db.CompanyApplicants.FirstOrDefault(x => x.EmailAddress == dto.EmailAddress);

                if (applicant != null)
                {
                    string hashedPassword = hasher.Hash(dto.Password);

                    if (!applicant.EmailConfirmed)
                        throw new ApiException(string.Format(CommonMessages.MSG_NEED_EMAILCONFIRM, CommonConstants.TBLNAME_APP_USERS));

                    if (applicant.PasswordHash != hashedPassword)
                        throw new ApiException(string.Format(CommonMessages.MSG_INVALID_PASS, CommonConstants.TBLNAME_APP_USERS));

                    return new LoginUserDto
                    {
                        UserId = applicant.Id,
                        UserName = applicant.FullName,
                        UserRole = "APPLICANT"
                    };
                }

                // Not found
                throw new ApiException(string.Format(CommonMessages.User_NOT_FOUND, CommonConstants.TBLNAME_USERS));
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ApiException(string.Format(CommonMessages.MSG_Login_FAIL, CommonConstants.TBLNAME_USERS));
            }
        }
    }
}