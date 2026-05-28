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

        public CompanyApplicants IsEmailExist(string email)
        {
            try
            {
                // exist applicant and confirmemail
                bool confirmedApplicant = db.CompanyApplicants
                    .Any(x => x.EmailAddress == email && x.EmailConfirmed);

                // SystemUsers 
                bool systemUserExists = db.SystemUsers
                    .Any(x => x.EmailAddress == email);

                // Confirm applicant or systemuser
                if (confirmedApplicant || systemUserExists)
                {
                    _logger.Error(string.Format(CommonMessages.MSG_EMAIL_EXIST, email));
                    throw new ApiException(
                        string.Format(CommonMessages.MSG_EMAIL_EXIST, email));
                }

                // unconfirmed applicant user
                var unconfirmedApplicant = db.CompanyApplicants
                    .FirstOrDefault(x => x.EmailAddress == email && !x.EmailConfirmed);

                return unconfirmedApplicant;
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
                using (TransactionScope scope = GetReadUncommittedScope())
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
                    var oldToken = db.EmailConfirmationTokens
                                      .Where(x => x.ApplicantId == user.Id).FirstOrDefault();
                    
                    if (oldToken != null)
                    {
                        db.EmailConfirmationTokens.Remove(oldToken);
                        db.SaveChanges();
                    }

                    // create new token
                    string newToken = CreateEmailToken(user.Id);
                    scope.Complete();

                    return newToken;
                }
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

        public (CompanyApplicantDto applicant, string token)UpdateUnconfirmedApplicant(CompanyApplicants applicant, ApplicantRegisterDTO dto)
        {
            try
            {
                using (TransactionScope scope = GetReadUncommittedScope())
                {
                    var hasher = new PasswordHasher();

                    // Update applicant data
                    applicant.FullName = dto.FullName;
                    applicant.PasswordHash = hasher.HashPassword(dto.Password);
                    applicant.PhoneNumber = dto.PhoneNumber;
                    applicant.Nationality = dto.Nationality;
                    applicant.IdentityNumber = dto.IdentityNumber;
                    applicant.CreatedAt = DateTime.UtcNow;

                    db.SaveChanges();

                    // remove old token
                    var oldTokens = db.EmailConfirmationTokens
                        .Where(x => x.ApplicantId == applicant.Id)
                        .ToList();

                    if (oldTokens.Any())
                    {
                        db.EmailConfirmationTokens.RemoveRange(oldTokens);
                        db.SaveChanges();
                    }

                    // create new token
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

                throw new ApiException(
                    string.Format(CommonMessages.MSG_CREATE_FAIL,CommonConstants.TBLNAME_APP_USERS));
            }
        }
    }
}