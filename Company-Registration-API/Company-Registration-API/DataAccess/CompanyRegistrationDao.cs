using Company_Registration_API.Models;
using Company_Registration_API.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration_API.DataAccess
{
    public class CompanyRegistrationDao
    {
        private ApplicantDbContext db;

        public CompanyRegistrationDao(ApplicantDbContext context)
        {
            db = context;
        }
        public long CreateCompanyRegistration(CompanyRegistrationDTO dto)
        {
            try
            {
                var existingApplicant = db.RegisteredCompanies.FirstOrDefault(x => x.ApplicantId == dto.ApplicantId);

                if (existingApplicant != null)
                {
                    throw new ApiException(CommonMessages.MSG_APPLICANT_EXIST);
                }

                // ========== 1. Company ==========
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
                db.SaveChanges();

                // ========== 2. Share Capital ==========
                if (dto.ShareCapital != null)
                {
                    db.CompanyShareCapital.Add(new CompanyShareCapital
                    {
                        CompanyId = company.Id,
                        AuthorizedShareCapital = dto.ShareCapital.AuthorizedShareCapital,
                        IssuedShareCapital = dto.ShareCapital.IssuedShareCapital,
                        PaidUpShareCapital = dto.ShareCapital.PaidUpShareCapital,
                        ShareCurrency = dto.ShareCapital.ShareCurrency,
                        CreatedAt = DateTime.Now
                    });
                }

                // ========== 3. Shareholders ==========
                if (dto.Shareholders != null)
                {
                    foreach (var sh in dto.Shareholders)
                    {
                        db.CompanyShareholders.Add(new CompanyShareholder
                        {
                            CompanyId = company.Id,
                            ShareholderName = sh.ShareholderName,
                            ShareholderType = sh.ShareholderType,
                            Nationality = sh.Nationality,
                            IdentityNumber = sh.IdentityNumber,
                            NumberOfShares = sh.NumberOfShares,
                            SharePercentage = sh.SharePercentage,
                            EmailAddress = sh.EmailAddress,
                            CreatedAt = DateTime.Now
                        });
                    }
                }

                // ========== 4. UHC ==========
                if (dto.UHC != null)
                {
                    db.UltimateHoldingCompanies.Add(new UltimateHoldingCompany
                    {
                        CompanyId = company.Id,
                        UHCName = dto.UHC.UHCName,
                        RegistrationNumber = dto.UHC.RegistrationNumber,
                        CountryOfIncorporation = dto.UHC.CountryOfIncorporation,
                        OwnershipPercentage = dto.UHC.OwnershipPercentage,
                        CreatedAt = DateTime.Now
                    });
                }

                // ========== 5. Constitution ==========
                if (dto.Constitution != null)
                {
                    db.CompanyConstitutions.Add(new CompanyConstitution
                    {
                        CompanyId = company.Id,
                        ConstitutionType = dto.Constitution.ConstitutionType,
                        ConstitutionFilePath = dto.Constitution.ConstitutionFilePath,
                        UploadedAt = DateTime.Now
                    });
                }

                db.SaveChanges();

                return company.Id;

            }
            catch (ApiException)
            {
                throw new ApiException(CommonMessages.MSG_APPLICANT_EXIST);
            }
        }
                
    }
}