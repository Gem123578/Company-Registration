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
    public class ApplicantRegistrationDao
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
                    throw new ApiException(string.Format(CommonMessages.MSG_EMAIL_EXIST, "EmailAddress", email));
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
        //public bool ValidatePassword(string password)
        //{
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(password))
        //        {
        //            throw new ApiException(string.Format(CommonMessages.MSG_REQUIRED_FIELD, "Password"));
        //        }

        //        return true;
        //    }
        //    catch (ApiException)
        //    {
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error(null, ex);
        //        throw new ApiException(string.Format(CommonMessages.MSG_REQUIRED_FIELD, "Password"));
        //    }
        //}


        public CompanyApplicantDto CreateApplicant(ApplicantRegisterDTO dto)
        {
            try
            {
                using (TransactionScope transaction = BaseDao.GetReadUncommittedScope())
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
                var test = db.CompanyApplicants.ToList();
                _logger.Info("Total rows now: " + test.Count);

                _logger.Info(db.Entry(applicant).State.ToString());

                _logger.Info(db.Database.Connection.ConnectionString);

                _logger.Info("Rows inserted: " + rows);
                //transaction.Complete();

                db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

                return new CompanyApplicantDto
                    {
                        Id = applicant.Id,
                        FullName = applicant.FullName,
                        EmailAddress = applicant.EmailAddress,
                        PhoneNumber = applicant.PhoneNumber,
                        EmailConfirmed = applicant.EmailConfirmed,
                        IdentityNumber = applicant.IdentityNumber,
                        CreatedAt = applicant.CreatedAt,
                        Nationality = applicant.Nationality
                    };
                }
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(null, ex);
                throw;
                //throw new ApiException(string.Format(CommonMessages.MSG_READ_FAIL, CommonConstants.TBLNAME_APP_USERS));
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
        //public CompanyAplicantDto Login(LoginDTO dto)
        //{
        //    try
        //    {
        //        Pass
        //    }
        //    catch (ApiException)
        //    {
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ApiException(string.Format(CommonMessages.MSG_Login_FAIL, CommonConstants.TBLNAME_APP_USERS));
        //    }
        //}

        //public List<CompanyApplicant> GetAllApplicants()
        //{
        //    return db.CompanyApplicants.Select(x => new ApplicantRegisterDTO
        //    {
        //        Id = x.Id,
        //        FullName = x.FullName,
        //        EmailAddress = x.EmailAddress,
        //        PhoneNumber = x.PhoneNumber,
        //        Nationality = x.Nationality,
        //        IdentityNumber = x.IdentityNumber,
        //        CreatedAt = x.CreatedAt
        //    }).ToList();
        //}


        internal bool ConfirmEmail(string token, string email)
        {
            try
            {
                var tokenData = db.EmailConfirmationTokens.FirstOrDefault(t => t.Token == token && t.ExpireAt > DateTime.UtcNow);

                if (tokenData == null) return false;

                var user = db.CompanyApplicants.FirstOrDefault(x => x.Id == tokenData.ApplicantId);

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
            catch(Exception ex)
            {
                _logger.Error(null, ex);
                throw new ApiException(string.Format(CommonMessages.MSG_READ_FAIL, CommonConstants.TBLNAME_APP_USERS));
            }
        }

        internal List<EmailConfirmationToken> GetEmailTokens(string email)
        {
            try
            {
                 var user = db.CompanyApplicants.FirstOrDefault(x => x.EmailAddress == email);
            if (user == null)
                return null;
            return db.EmailConfirmationTokens.Where(t => t.ApplicantId == user.Id).OrderByDescending(t => t.CreatedAt)
                     .ToList();
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
    }
}