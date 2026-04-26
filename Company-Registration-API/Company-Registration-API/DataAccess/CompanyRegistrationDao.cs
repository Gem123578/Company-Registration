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

        public CompanyRegistrationDao()
        {
            db = new ApplicantDbContext();
        }

        //Get company registration
        internal List<CompanyRegistrationDTO> GetAll()
        {
            try
            {
                var list = db.RegisteredCompanies
                    .Select(x => new CompanyRegistrationDTO
                    {
                        Id = x.Id,
                        CompanyName = x.CompanyName,
                        RegistrationNumber = x.RegistrationNumber,
                        CompanyType = x.CompanyType,
                        BusinessActivity = x.BusinessActivity,
                        RegisteredAddress = x.RegisteredAddress,
                        RegistrationStatus = x.RegistrationStatus,
                        ApplicantId = x.ApplicantId,
                        UserId = x.UserId,
                        IncorporationDate = x.IncorporationDate,
                        CreatedAt = x.CreatedAt
                    })
                    .OrderByDescending(x => x.Id)
                    .ToList();

                return list;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ApiException(CommonMessages.MSG_READ_FAIL);
            }
        }

        // GET BY ID
        internal CompanyRegistrationDTO GetById(long id)
        {
            try
            {
                var x = db.RegisteredCompanies.FirstOrDefault(c => c.Id == id);

                if (x == null) return null;

                return new CompanyRegistrationDTO
                {
                    Id = x.Id,
                    CompanyName = x.CompanyName,
                    RegistrationNumber = x.RegistrationNumber,
                    CompanyType = x.CompanyType,
                    BusinessActivity = x.BusinessActivity,
                    RegisteredAddress = x.RegisteredAddress,
                    RegistrationStatus = x.RegistrationStatus,
                    ApplicantId = x.ApplicantId,
                    UserId = x.UserId,
                    IncorporationDate = x.IncorporationDate,
                    CreatedAt = x.CreatedAt
                };
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ApiException("Failed to get company");
            }
        }
        // UPDATE
        internal RegisteredCompany UpdateCompany(long id, CompanyRegistrationDTO dto)
        {
            try
            {
                var company = db.RegisteredCompanies.FirstOrDefault(x => x.Id == id);

                if (company == null)
                {
                    throw new ApiException("Company not found");
                }

                company.CompanyName = dto.CompanyName;
                company.CompanyType = dto.CompanyType;
                company.BusinessActivity = dto.BusinessActivity;
                company.RegisteredAddress = dto.RegisteredAddress;
                company.RegistrationStatus = dto.RegistrationStatus;
                company.IncorporationDate = dto.IncorporationDate;

                db.SaveChanges();
                return company;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ApiException("Failed to update company");
            }
        }

        //  DELETE
        internal void DeleteCompany(long id)
        {
            try
            {
                var company = db.RegisteredCompanies.FirstOrDefault(x => x.Id == id);

                if (company == null)
                {
                    throw new ApiException("Company not found");
                }

                db.RegisteredCompanies.Remove(company);
                db.SaveChanges();
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ApiException(CommonMessages.MSG_Delete_FAIL);
            }
        }
        public long CreateCompanyRegistration(CompanyRegistrationDTO dto)
        {
            try
            {
                if(dto.ApplicantId == 0) 
                {
                    dto.ApplicantId = 0;
                }
                dto.UserId = 0;

                if (db.RegisteredCompanies.Any(x => x.ApplicantId == dto.ApplicantId))
                {
                    throw new ApiException(CommonMessages.MSG_APPLICANT_EXIST);
                }

                // 1. Company save
                var company = SaveCompany(db, dto);

                // 2. Share Capital save
                if (dto.ShareCapital != null)
                    SaveShareCapital(db, dto.ShareCapital, company.Id);

                // 3. Shareholders save
                if (dto.Shareholders != null)
                    SaveShareholders(db, dto.Shareholders, company.Id);

                // 4. UHC save
                if (dto.UHC != null)
                    SaveUHC(db, dto.UHC, company.Id);

                // 5. Constitution save
                if (dto.Constitution != null)
                    SaveConstitution(db, dto.Constitution, company.Id);

                return company.Id;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ApiException(CommonMessages.MSG_APPLICANT_EXIST);
            }
        }
        public RegisteredCompany SaveCompany(ApplicantDbContext context, CompanyRegistrationDTO dto)
        {
            var company = new RegisteredCompany
            {
                CompanyName = dto.CompanyName,
                RegistrationNumber = dto.RegistrationNumber,
                CompanyType = dto.CompanyType,
                BusinessActivity = dto.BusinessActivity,
                RegisteredAddress = dto.RegisteredAddress,
                RegistrationStatus = dto.RegistrationStatus ?? "PENDING",
                ApplicantId = dto.ApplicantId == 0 ? (long?)null : dto.ApplicantId,
                UserId = dto.UserId == 0 ? (long?)null : dto.UserId,
                IncorporationDate = dto.IncorporationDate,
                CreatedAt = DateTime.Now
            };

            context.RegisteredCompanies.Add(company);
            context.SaveChanges(); // Save company to get Id
            return company;
        }

        public void SaveShareCapital(ApplicantDbContext context, CompanyShareCapitalDTO shareCapital, long companyId)
        {
            var entity = new CompanyShareCapital
            {
                CompanyId = companyId,
                AuthorizedShareCapital = shareCapital.AuthorizedShareCapital,
                IssuedShareCapital = shareCapital.IssuedShareCapital,
                PaidUpShareCapital = shareCapital.PaidUpShareCapital,
                ShareCurrency = shareCapital.ShareCurrency,
                CreatedAt = DateTime.Now
            };

            context.CompanyShareCapital.Add(entity);
            context.SaveChanges();
        }

        public void SaveShareholders(ApplicantDbContext context, List<CompanyShareholderDTO> shareholders, long companyId)
        {
            foreach (var sh in shareholders)
            {
                var entity = new CompanyShareholder
                {
                    CompanyId = companyId,
                    ShareholderName = sh.ShareholderName,
                    ShareholderType = sh.ShareholderType,
                    Nationality = sh.Nationality,
                    IdentityNumber = sh.IdentityNumber,
                    NumberOfShares = sh.NumberOfShares,
                    SharePercentage = sh.SharePercentage,
                    EmailAddress = sh.EmailAddress,
                    CreatedAt = DateTime.Now
                };

                context.CompanyShareholders.Add(entity);
                context.SaveChanges();
            }
        }

        public void SaveUHC(ApplicantDbContext context, UltimateHoldingCompanyDTO uhc, long companyId)
        {
            var entity = new UltimateHoldingCompany
            {
                CompanyId = companyId,
                UHCName = uhc.UHCName,
                RegistrationNumber = uhc.RegistrationNumber,
                CountryOfIncorporation = uhc.CountryOfIncorporation,
                OwnershipPercentage = uhc.OwnershipPercentage,
                CreatedAt = DateTime.Now
            };

            context.UltimateHoldingCompanies.Add(entity);
            context.SaveChanges();
        }

        public void SaveConstitution(ApplicantDbContext context, CompanyConstitutionDTO constitution, long companyId)
        {
            var entity = new CompanyConstitution
            {
                CompanyId = companyId,
                ConstitutionType = constitution.ConstitutionType,
                ConstitutionFilePath = constitution.ConstitutionFilePath,
                UploadedAt = DateTime.Now
            };

            context.CompanyConstitutions.Add(entity);
            context.SaveChanges();
        }
    }

}
