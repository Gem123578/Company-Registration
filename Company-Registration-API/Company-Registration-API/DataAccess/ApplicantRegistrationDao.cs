using Company_Registration_API.Models;
using Company_Registration_API.Models.CompanyApplicant;
using Company_Registration_API.Models.DTO;
using Company_Registration_API.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

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

        public CompanyAplicantDto CreateApplicant(ApplicantRegisterDTO dto, List<EmailConfirmationToken> token)
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
                    EmailConfirm = false,
                    ResendCount = 0,
                    LastResendAt = DateTime.UtcNow,
                };
                db.CompanyApplicants.Add(applicant);
                db.SaveChanges();
                token = CreateEmailToken(applicant.Id);
                return new CompanyAplicantDto
                {
                    Id = applicant.Id,
                    FullName = applicant.FullName,
                    EmailAddress = applicant.EmailAddress,
                    PhoneNumber = applicant.PhoneNumber,
                    EmailConfirmed = applicant.EmailConfirm,
                    IdentityNumber = applicant.IdentityNumber,
                    CreatedAt = applicant.CreatedAt,
                    EmailToken = applicant.EmailConfirmedToken,
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
                    EmailToken = newToken,
                    CreatedAt = DateTime.UtcNow,
                    ExpiredAt = DateTime.UtcNow.AddMinutes(2)
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
            catch (Exception ex)
            {
                throw new ApiException(string.Format(CommonMessages.MSG_CREATE_FAIL, CommonConstants.TBLNAME_EMAIL_TOKEN));
            }
        }
        public CompanyAplicantDto Login(LoginDTO dto)
        {
            PasswordHasher hasher = new PasswordHasher();
            // 1. Find user by email
            var applicant = db.CompanyApplicants
                .FirstOrDefault(x => x.EmailAddress == dto.EmailAddress);

            if (applicant == null)
                return null; // email not found

            // 2. Verify password
            string hashedPassword = hasher.Hash(dto.Password); // Hash method you already have
            if (applicant.PasswordHash != hashedPassword)
                return null; // wrong password

            // 3. Return DTO including EmailConfirmed
            return new CompanyAplicantDto
            {
                Id = applicant.Id,
                FullName = applicant.FullName,
                EmailAddress = applicant.EmailAddress,
                PhoneNumber = applicant.PhoneNumber,
                Nationality = applicant.Nationality,
                IdentityNumber = applicant.IdentityNumber,
                CreatedAt = applicant.CreatedAt,
                EmailConfirmed = applicant.EmailConfirm // <-- important
            };
        }

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
                var user = db.CompanyApplicants.FirstOrDefault(x => x.EmailAddress == email);
                if (user == null)
                    return false;

                var tokenData = db.EmailConfirmationTokens
                    .FirstOrDefault(t => t.ApplicantId == user.Id && t.EmailToken == token && t.ExpiredAt > DateTime.UtcNow);

                if (tokenData == null)
                    return false;

                user.EmailConfirm = true;
                user.EmailConfirmationDate = DateTime.UtcNow;

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

        //internal List<EmailConfirmToken> GetEmailTokens(string email)
        //{
        //    var user = db.CompanyApplicants.FirstOrDefault(x => x.EmailAddress == email);
        //    if (user == null)
        //        return null;
        //    return db.EmailConfirmationTokens.Where(t => t.ApplicantId == user.Id).ToList();
        //}
    }
}