using Company_Registration_API.Models;
using Company_Registration_API.Models.DTO;
using System;

namespace Company_Registration_API.Common
{
    public class ModelConverter
    {
        internal static CompanyApplicantDto ToCompanyApplicantDto(CompanyApplicants applicant)
        {
            try
            {
                if (applicant == null)
                {
                    return new CompanyApplicantDto();
                }

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
            catch (Exception) 
            {
                throw;
            } 
        }
    }
}