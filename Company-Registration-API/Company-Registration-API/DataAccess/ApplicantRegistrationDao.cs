using Company_Registration_API.Models;
using Company_Registration_API.Models.DTO;
using Company_Registration_API.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Company_Registration_API.DataAccess
{
    public class ApplicantRegistrationDao
    {
        private readonly ApplicantDbContext db;

        public ApplicantRegistrationDao()
        {
            db = new ApplicantDbContext();
        }

        public bool IsEmailExist(string email)
        {
            return db.CompanyApplicants.Any(x => x.EmailAddress == email);
        }

        public CompanyAplicantDto CreateApplicant(ApplicantRegisterDTO dto)
        {
            try
            {
                PasswordHasher hasher = new PasswordHasher();
                CompanyApplicants applicant = new CompanyApplicants
                {
                    FullName = dto.FullName,
                    EmailAddress = dto.EmailAddress,
                    PasswordHash = hasher.Hash(dto.Password),
                    PhoneNumber = dto.PhoneNumber,
                    Nationality = dto.Nationality,
                    IdentityNumber = dto.IdentityNumber,
                    CreatedAt = DateTime.UtcNow,
                    EmailConfirmed = false,
                    EmailConfirmedAt = DateTime.UtcNow,
                    ResendCount = 0,
                    LastResendAt = DateTime.UtcNow,
                };
                db.CompanyApplicants.Add(applicant);
                db.SaveChanges();
                var token = CreateEmailToken(applicant.Id);
                return new CompanyAplicantDto
                {
                    Id = applicant.Id,
                    FullName = applicant.FullName,
                    EmailAddress = applicant.EmailAddress,
                    PhoneNumber = applicant.PhoneNumber,
                    EmailConfirmed = applicant.EmailConfirmed,
                    IdentityNumber = applicant.IdentityNumber,
                    CreatedAt = applicant.CreatedAt,
                    EmailToken = token,
                    Nationality = applicant.Nationality
                };
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(string.Format(CommonMessages.MSG_READ_FAIL, CommonConstants.TBLNAME_APP_USERS));
            }
        }
        public List<EmailConfirmationToken> CreateEmailToken(long applicantId)
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
                return db.EmailConfirmationTokens
                         .Where(t => t.ApplicantId == applicantId)
                         .OrderByDescending(t => t.CreatedAt)
                         .ToList();
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception)
            {
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
                throw new ApiException(string.Format(CommonMessages.MSG_READ_FAIL, CommonConstants.TBLNAME_APP_USERS));
            }
        }

        internal List<EmailConfirmationToken> GetEmailTokens(string email)
        {
            var user = db.CompanyApplicants.FirstOrDefault(x => x.EmailAddress == email);
            if (user == null)
                return null;
            return db.EmailConfirmationTokens.Where(t => t.ApplicantId == user.Id).OrderByDescending(t => t.CreatedAt)
                     .ToList();
        }
    }
}