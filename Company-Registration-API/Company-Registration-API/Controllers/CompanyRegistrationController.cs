using Company_Registration_API.DataAccess;
using Company_Registration_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Company_Registration_API.Controllers
{
    [RoutePrefix("api/CompanyRegistration")]
    public class CompanyRegistrationController : ApiController
    {
        private ApplicantDbContext db = new ApplicantDbContext();

        //post submit company registration form
        [HttpPost]
        [Route("Submit")]
        public IHttpActionResult SubmitRegistration(CompanyRegistrationDTO dto)
        {
            if (dto == null)
                return BadRequest("Registration data is null.");

            // ========== 1. Registered Company ==========
            var company = new RegisteredCompany
            {
                CompanyName = dto.CompanyName,
                RegistrationNumber = dto.RegistrationNumber,
                CompanyType = dto.CompanyType,
                BusinessActivity = dto.BusinessActivity,
                RegisteredAddress = dto.RegisteredAddress,
                RegistrationStatus = dto.RegistrationStatus ?? "PENDING",
                ApplicantId = dto.ApplicantId,
                IncorporationDate = dto.IncorporationDate,
                CreatedAt = DateTime.Now
            };

            db.RegisteredCompanies.Add(company);
            db.SaveChanges(); // 🔥 Save first to generate company.Id

            // ========== 2. Share Capital ==========
            if (dto.ShareCapital != null)
            {
                var capital = new CompanyShareCapital
                {
                    CompanyId = company.Id,  // ✅ FIXED
                    AuthorizedShareCapital = dto.ShareCapital.AuthorizedShareCapital,
                    IssuedShareCapital = dto.ShareCapital.IssuedShareCapital,
                    PaidUpShareCapital = dto.ShareCapital.PaidUpShareCapital,
                    ShareCurrency = dto.ShareCapital.ShareCurrency,
                    CreatedAt = DateTime.Now
                };

                db.CompanyShareCapital.Add(capital);
            }

            // ========== 3. Shareholders ==========
            if (dto.Shareholders != null)
            {
                foreach (var sh in dto.Shareholders)
                {
                    var shareholder = new CompanyShareholder
                    {
                        CompanyId = company.Id,  // ✅ FIXED
                        ShareholderName = sh.ShareholderName,
                        ShareholderType = sh.ShareholderType,
                        Nationality = sh.Nationality,
                        IdentityNumber = sh.IdentityNumber,
                        NumberOfShares = sh.NumberOfShares,
                        SharePercentage = sh.SharePercentage,
                        EmailAddress = sh.EmailAddress,
                        CreatedAt = DateTime.Now
                    };

                    db.CompanyShareholders.Add(shareholder);
                }
            }

            // ========== 4. Company Stakeholders ==========
            if (dto.CompanyStakeholders != null)
            {
                foreach (var cs in dto.CompanyStakeholders)
                {
                    var stakeholder = new CompanyStakeholder
                    {
                        CompanyId = company.Id,  // ✅ FIXED
                        FullName = cs.FullName,
                        StakeholderRole = cs.StakeholderRole,
                        SharePercentage = cs.SharePercentage,
                        Nationality = cs.Nationality,
                        IdentityNumber = cs.IdentityNumber,
                        EmailAddress = cs.EmailAddress
                    };

                    db.CompanyStakeholders.Add(stakeholder);
                }
            }

            // ========== 5. Ultimate Holding Company ==========
            if (dto.UHC != null)
            {
                var uhc = new UltimateHoldingCompany
                {
                    CompanyId = company.Id,  // ✅ FIXED
                    UHCName = dto.UHC.UHCName,
                    RegistrationNumber = dto.UHC.RegistrationNumber,
                    CountryOfIncorporation = dto.UHC.CountryOfIncorporation,
                    OwnershipPercentage = dto.UHC.OwnershipPercentage,
                    CreatedAt = DateTime.Now
                };

                db.UltimateHoldingCompanies.Add(uhc);
            }

            // ========== 6. Company Constitution ==========
            if (dto.Constitution != null)
            {
                var cons = new CompanyConstitution
                {
                    CompanyId = company.Id,  // ✅ THIS FIXES YOUR FK ERROR
                    ConstitutionType = dto.Constitution.ConstitutionType,
                    ConstitutionFilePath = dto.Constitution.ConstitutionFilePath,
                    UploadedAt = DateTime.Now
                };

                db.CompanyConstitutions.Add(cons);
            }

            // ========== 7. Registration Payment ==========
            if (dto.Payment != null)
            {
                var payment = new RegistrationPayment
                {
                    CompanyId = company.Id,  // ✅ FIXED
                    TransactionId = dto.Payment.TransactionId,
                    PaymentMethod = dto.Payment.PaymentMethod,
                    Amount = dto.Payment.Amount,
                    CurrencyCode = dto.Payment.CurrencyCode,
                    PaymentStatus = dto.Payment.PaymentStatus,
                    PaidAt = dto.Payment.PaidAt
                };

                db.RegistrationPayments.Add(payment);
            }

            db.SaveChanges(); // Final Save

            return Ok("Company registration submitted successfully.");
        }

    }
}