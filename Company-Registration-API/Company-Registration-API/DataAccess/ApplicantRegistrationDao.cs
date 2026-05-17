using Company_Registration_API.Common;
using Company_Registration_API.Controllers;
using Company_Registration_API.Models;
using Company_Registration_API.Models.DTO;
using Company_Registration_API.Utils;
using log4net;
using Microsoft.AspNet.Identity;
using QPSOS.Web.API.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Helpers;

namespace Company_Registration_API.DataAccess
{
    public class ApplicantRegistrationDao :BaseDao
    {
        private readonly ILog _logger;
        private readonly ApplicantDbContext db;


        public ApplicantRegistrationDao()
        {
            _logger = LogManager.GetLogger(typeof(ApplicantRegistrationDao));
            db = new ApplicantDbContext();
        }

        public bool IsEmailExist(string email)
        {
            try
            {
                bool emailExists =db.CompanyApplicants.Any(x => x.EmailAddress == email)|| db.SystemUsers.Any(x => x.EmailAddress == email);

                if (emailExists)
                {
                    _logger.Error(string.Format(CommonMessages.MSG_EMAIL_EXIST, email));

                    throw new ApiException(
                        string.Format(CommonMessages.MSG_EMAIL_EXIST, email));
                }
                return false;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(null, ex);
                throw new ApiException(string.Format(CommonMessages.MSG_EMAIL_EXIST, CommonConstants.TBLNAME_APP_USERS));
            }

        }
        
        public (CompanyApplicantDto applicant, string token) CreateApplicant(ApplicantRegisterDTO dto)
        {
            try
            {
                using ( TransactionScope scope = GetReadUncommittedScope())
                {
                    var hasher = new PasswordHasher();
                    var applicant = new CompanyApplicants
                    {
                        FullName = dto.FullName,
                        EmailAddress = dto.EmailAddress,
                        PasswordHash = hasher.HashPassword(dto.Password),
                        PhoneNumber = dto.PhoneNumber,
                        Nationality = dto.Nationality,
                        IdentityNumber = dto.IdentityNumber,
                        CreatedAt = DateTime.UtcNow,
                        EmailConfirmed = false,
                        EmailConfirmedAt = null
                    };

                    db.CompanyApplicants.Add(applicant);
                    db.SaveChanges();

                    // Create user record
                    var user = new Users
                    {
                        ApplicantId = applicant.Id,
                        SystemId = null, // keep 0 if using applicant account
                        RoleId = 3,   // Applicant RoleId
                        IsUser = true,
                        CreatedAt = DateTime.UtcNow
                    };
                    db.Users.Add(user);

                    db.SaveChanges();
                    string token = CreateEmailToken(applicant.Id);
                    scope.Complete();
                    return (ModelConverter.ToCompanyApplicantDto(applicant), token);
                }
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(null, ex);
                throw new ApiException(string.Format(CommonMessages.MSG_READ_FAIL, CommonConstants.TBLNAME_APP_USERS));
            }
        }


        public string CreateEmailToken(long applicantId)
        {
            try
            {
                /// Remove expired tokens first
                var expiredTokens = db.EmailConfirmationTokens
                    .Where(t => t.ExpireAt <= DateTime.UtcNow)
                    .ToList();

                if (expiredTokens.Any())
                {
                    db.EmailConfirmationTokens.RemoveRange(expiredTokens);
                    db.SaveChanges();
                }

                // Remove old token for this applicant
                var oldTokens = db.EmailConfirmationTokens
                    .Where(t => t.ApplicantId == applicantId)
                    .ToList();

                if (oldTokens.Any())
                {
                    db.EmailConfirmationTokens.RemoveRange(oldTokens);
                    db.SaveChanges();
                }
                //generate new token
                string newToken = Guid.NewGuid().ToString();
                var emailToken = new EmailConfirmationToken
                {
                    ApplicantId = applicantId,
                    Token = newToken,
                    CreatedAt = DateTime.UtcNow,
                    ExpireAt = DateTime.UtcNow.AddMinutes(1)
                };
                db.EmailConfirmationTokens.Add(emailToken);
                db.SaveChanges();

                // Return all tokens for the applicant
                return emailToken.Token;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(null, ex);
                throw new ApiException(string.Format(CommonMessages.MSG_CREATE_FAIL, CommonConstants.TBLNAME_EMAIL_TOKEN));
            }
        }
        
        internal bool ConfirmEmail(string token, string email)
        {
            try
            {
                var user = db.CompanyApplicants.FirstOrDefault(x => x.EmailAddress == email);

                if (user == null)
                {
                    _logger.Error(CommonMessages.MSG_EXIST_TOKEN);
                    throw new ApiException(CommonMessages.MSG_EXIST_TOKEN);
                }

               
                var emailToken = db.EmailConfirmationTokens
                    .FirstOrDefault(x =>x.ApplicantId == user.Id && x.Token == token);

                if (emailToken == null)
                {
                    _logger.Error(CommonMessages.MSG_EMAIL_NOTF);
                    throw new ApiException(CommonMessages.MSG_EMAIL_NOTF);
                }

                // Expire Check
                if (emailToken.ExpireAt < DateTime.UtcNow)
                {
                    throw new ApiException(CommonMessages.MSG_TOKEN_EX);
                }

                user.EmailConfirmed = true;
                user.EmailConfirmedAt = DateTime.UtcNow;

                // Remove used token
                db.EmailConfirmationTokens.Remove(emailToken);

                db.SaveChanges();
                return true;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw new ApiException(string.Format(CommonMessages.MSG_READ_FAIL, CommonConstants.TBLNAME_APP_USERS));
            }
        }

        internal string ResendConfirmationEmail(string email)
        {
            try
            {
                var user = db.CompanyApplicants
                             .FirstOrDefault(x => x.EmailAddress == email);

                if (user == null)
                {
                    throw new ApiException(CommonMessages.User_NOT_FOUND);
                }

                if (user.EmailConfirmed)
                {
                    throw new ApiException(CommonMessages.MSG_EMAIL_EXIST);
                }

                // remove old tokens
                var oldTokens = db.EmailConfirmationTokens
                                  .Where(x => x.ApplicantId == user.Id)
                                  .ToList();

                if (oldTokens.Any())
                {
                    db.EmailConfirmationTokens.RemoveRange(oldTokens);
                    db.SaveChanges();
                }

                // create new token
                string newToken = CreateEmailToken(user.Id);

                return newToken;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }
        }

        internal bool ValidateIdentityNumber(string identityNumber)
        {
            try
            {
                var applicants = db.CompanyApplicants.Where(x => x.IdentityNumber == identityNumber).FirstOrDefault();
                if (applicants != null)
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_EXIST_NRC, identityNumber));
                }
                return false;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(null, ex);
                throw new ApiException(string.Format(CommonMessages.MSG_EXIST_NRC, identityNumber));
            }
        }

        internal bool ValidatePhoneNumber(string phoneNumber)
        {
            try
            {
                var applicants = db.CompanyApplicants.Where(x => x.PhoneNumber == phoneNumber).FirstOrDefault();
                if (applicants != null)
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_EXIST_PHNO, phoneNumber));
                }
                return false;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(null, ex);
                throw new ApiException(string.Format(CommonMessages.MSG_EXIST_PHNO, phoneNumber));
            }

        }

        internal bool ValidateToken(string tokenString)
        {
            try
            {
                if (string.IsNullOrEmpty(tokenString))
                {
                    throw new ApiException(string.Format(CommonMessages.MSG_EXIST_TOKEN));
                }
                return false;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(null, ex);
                throw new ApiException(string.Format(CommonMessages.MSG_EXIST_TOKEN));

            }
        }
    }
}