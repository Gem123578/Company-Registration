using Company_Registration_API.Models;
using Company_Registration_API.Models.DTO;
using Company_Registration_API.Utils;
using System;
using System.Linq;

namespace Company_Registration_API.DataAccess
{
    public class SystemUsersDao
    {
        private readonly ApplicantDbContext db;

        public SystemUsersDao(ApplicantDbContext context)
        {
            db = context;
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
                    user = db.SystemUsers.Where(u => u.Id == id).FirstOrDefault();
                    if (user == null)
                    {
                        throw new ApiException(string.Format(CommonMessages.User_NOT_FOUND, CommonConstants.TBLNAME_USERS));
                    }
                }

                //common fields for both create and update
                user.UserName = dto.UserName;
                user.UserRole = dto.UserRole;
                user.AccountStatus = dto.AccountStatus;

                //update login
                user.LastLoginAt = DateTime.UtcNow;

                //create case
                if (!dto.IsUpdate)
                {
                    user.EmailAddress = dto.EmailAddress;
                    user.PasswordHash = hasher.Hash(dto.Password);
                    user.CreatedAt = DateTime.UtcNow;
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

        internal SystemUsers ValidateUser(LoginDTO dto)
        {
            try
            {
                PasswordHasher hasher = new PasswordHasher();

                // 1️⃣ Find user by email
                var user = db.SystemUsers.FirstOrDefault(x => x.EmailAddress == dto.EmailAddress);
                if (user == null)
                {
                    throw new ApiException(string.Format(CommonMessages.User_NOT_FOUND, CommonConstants.TBLNAME_USERS));
                }

                string hashedPassword = hasher.Hash(dto.Password); // Hash method you already have
                if (dto.Password != hashedPassword)
                    throw new ApiException(string.Format(CommonMessages.MSG_INVALID_PASS, CommonConstants.TBLNAME_USERS));
                
                // 3️⃣ Check account status
                if (user.AccountStatus != "ACTIVE")
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_DISABLE_ACC, CommonConstants.TBLNAME_USERS));
                }

                return user;
            }
            catch (ApiException)
            {
                // Rethrow known API exceptions
                throw;
            }
            catch (Exception)
            {
                // Wrap unknown exceptions with friendly message
                throw new ApiException(string.Format(CommonMessages.MSG_Login_FAIL, CommonConstants.TBLNAME_USERS));
            }
        }
    }
}