using Company_Registration_API.Models;
using Company_Registration_API.Models.CompanyApplicant;
using Company_Registration_API.Models.DTO;
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
            var applicant = new CompanyApplicants
            {
                FullName = dto.FullName,
                EmailAddress = dto.EmailAddress,
                PasswordHash = Hash(dto.Password),
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
        public CompanyAplicantDto Login(ApplicantLoginDTO dto)
        {
            // 1. Find user by email
            var applicant = db.CompanyApplicants
                .FirstOrDefault(x => x.EmailAddress == dto.EmailAddress);

            if (applicant == null)
                return null; // email not found

            // 2. Verify password
            string hashedPassword = Hash(dto.Password); // Hash method you already have
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

        private string Hash(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return password;

            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }

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