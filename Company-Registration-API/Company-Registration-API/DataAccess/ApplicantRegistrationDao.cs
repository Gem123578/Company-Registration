using Company_Registration_API.Common;
using Company_Registration_API.Controllers;
using Company_Registration_API.Models;
using Company_Registration_API.Models.DTO;
using Company_Registration_API.Utils;
using log4net;
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
                var applicants = db.CompanyApplicants.Where(x => x.EmailAddress == email).FirstOrDefault();
                if (applicants != null)
                {
                    _logger.Error(string.Format(CommonMessages.MSG_EMAIL_EXIST, email));
                    throw new ApiException(string.Format(CommonMessages.MSG_EMAIL_EXIST, email));
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
        
        public CompanyApplicantDto CreateApplicant(ApplicantRegisterDTO dto)
        {
            try
            {
                    
                    var applicant = new CompanyApplicants
                    {
                        FullName = dto.FullName,
                        EmailAddress = dto.EmailAddress,
                        PasswordHash = new PasswordHasher().Hash(dto.Password),
                        PhoneNumber = dto.PhoneNumber,
                        Nationality = dto.Nationality,
                        IdentityNumber = dto.IdentityNumber,
                        CreatedAt = DateTime.UtcNow,
                        EmailConfirmed = false,
                        EmailConfirmedAt = null
                    };

                    db.CompanyApplicants.Add(applicant);
                    var rows = db.SaveChanges();
                    return ModelConverter.ToCompanyApplicantDto(applicant);
                
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
                //Remove old token
                List<EmailConfirmationToken> oldToken = db.EmailConfirmationTokens.Where(t => t.ApplicantId == applicantId).ToList();
                if (oldToken.Any())
                    db.EmailConfirmationTokens.RemoveRange(oldToken);

                //generate new token
                string newToken = Guid.NewGuid().ToString();
                var emailToken = new EmailConfirmationToken
                {
                    ApplicantId = applicantId,
                    Token = newToken,
                    CreatedAt = DateTime.UtcNow,
                    ExpireAt = DateTime.UtcNow.AddMinutes(30)
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

                var tokenData = db.EmailConfirmationTokens
                    .FirstOrDefault(t => t.Token == token
                                      && t.ApplicantId == user.Id
                                      && t.ExpireAt > DateTime.UtcNow);

                if (tokenData == null)
                {
                    _logger.Error(CommonMessages.MSG_EMAIL_NOTF);
                    throw new ApiException(CommonMessages.MSG_EMAIL_NOTF);
                }

                user.EmailConfirmed = true;
                user.EmailConfirmedAt = DateTime.UtcNow;

                // Remove used token
                db.EmailConfirmationTokens.Remove(tokenData);

                db.SaveChanges();
                return true;
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

        //internal List<EmailConfirmationToken> GetEmailTokens(string email)
        //{
        //    try
        //    {
        //        var user = db.CompanyApplicants.FirstOrDefault(x => x.EmailAddress == email);
        //        if (user == null)
        //        {
        //            _logger.Warn(string.Format(CommonMessages.MSG_EMAIL_EXIST, email));
        //            throw new ApiException(string.Format(CommonMessages.MSG_EMAIL_EXIST, email));
        //        }
        //        return db.EmailConfirmationTokens.Where(t => t.ApplicantId == user.Id).OrderByDescending(t => t.CreatedAt)
        //                 .ToList();
        //    }
        //    catch (ApiException)
        //    {
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(null, ex);
        //        throw new ApiException(string.Format(CommonMessages.MSG_READ_FAIL, CommonConstants.TBLNAME_APP_USERS));
        //    }

        //}

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