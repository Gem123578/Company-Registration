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

        public ApplicantRegistrationDao(ApplicantDbContext context)
        {
            db = context;
        }

        public bool IsEmailExist(string email)
        {
            return db.CompanyApplicants.Any(x => x.EmailAddress == email);
        }

        public CompanyAplicantDto CreateApplicant(ApplicantRegisterDTO dto , string token)
        {
            PasswordHasher hasher = new PasswordHasher();
            var applicant = new CompanyApplicants
            {
                FullName = dto.FullName,
                EmailAddress = dto.EmailAddress,
                PasswordHash = hasher.Hash(dto.Password),
                PhoneNumber = dto.PhoneNumber,
                Nationality = dto.Nationality,
                IdentityNumber = dto.IdentityNumber,
                EmailToken = token,
                EmailConfirmed = false,
                CreatedAt = DateTime.Now
            };

            db.CompanyApplicants.Add(applicant);
            db.SaveChanges();
            return new CompanyAplicantDto
            {
                Id = applicant.Id,
                FullName = applicant.FullName,
                EmailAddress = applicant.EmailAddress,
                PhoneNumber = applicant.PhoneNumber,
                Nationality = applicant.Nationality,
                IdentityNumber = applicant.IdentityNumber,
                CreatedAt = applicant.CreatedAt
            };
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
                EmailConfirmed = applicant.EmailConfirmed // <-- important
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
            var user = db.CompanyApplicants.FirstOrDefault(x => x.EmailAddress == email && x.EmailToken == token);

            if (user == null)
                return false;

            user.EmailConfirmed = true;
            user.EmailToken = null;

            db.SaveChanges();

            return true;
        }
    }
}