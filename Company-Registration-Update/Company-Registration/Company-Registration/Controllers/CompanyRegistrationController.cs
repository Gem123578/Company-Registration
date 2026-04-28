using Company_Registration.APIServices;
using Company_Registration.Common;
using Company_Registration.Models;
using Company_Registration.Utils;
using Company_Registration_API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace Company_Registration.Controllers
{
    public class CompanyRegistrationController : Controller
    {
        private readonly CompanyRegistrationService _service;

        public CompanyRegistrationController()
        {
            _service = new CompanyRegistrationService();
        }

        public async Task<ActionResult> CompanyGrid()
        {
            try
            {
                var response = await _service.GetAllCompanies();
                var companies = new List<CompanyRegistrationDTO>();

                if (response.IsSuccess && response.Data != null)
                {
                    companies = JsonConvert.DeserializeObject<List<CompanyRegistrationDTO>>(response.Data.ToString());
                }

                return PartialView("_CompanyGrid", companies);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to load companies: " + ex.Message;
                return PartialView("_CompanyGrid", new List<CompanyRegistrationDTO>());
            }
        }
        public async Task<ActionResult> GetCompanyById(long id)
        {
            var model = new CompanyRegistrationDTO();

            ResponseDto response = await _service.GetCompanyById(id);

            if (response.IsSuccess && response.Data != null)
            {
                model = JsonConvert.DeserializeObject<CompanyRegistrationDTO>(response.Data.ToString());
            }

            return PartialView("_CreateUpdateCompany", model);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateCompany(long id, CompanyRegistrationDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return PartialView("_CreateUpdateCompany", model);

                var response = await _service.UpdateCompany(id, model);

                if (!response.IsSuccess)
                {
                    ModelState.AddModelError("", response.Result?.Message ?? "Update failed");
                    return PartialView("_CreateUpdateCompany", model);
                }

                return Json(new { success = true, message = "Updated successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public async Task<ActionResult> DeleteCompany(long id)
        {
            try
            {
                var response = await _service.DeleteCompany(id);

                if (!response.IsSuccess)
                {
                    return Json(new { success = false, message = response.Result?.Message });
                }

                return Json(new { success = true, message = "Deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        // GET: Register Form
        public ActionResult Register()
        {
            var model = new CompanyRegistrationViewModel
            {
                ShareCapital = new CompanyShareCapitalViewModel(),
                UHC = new UltimateHoldingCompanyViewModel(),
                Constitution = new CompanyConstitutionViewModel(),
                Shareholders = new List<CompanyShareholderViewModel>
        {
            new CompanyShareholderViewModel()
        }
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Register(CompanyRegistrationViewModel model,HttpPostedFileBase ConstitutionFilePath)
            {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
                       
            int applicantId = 0;
            int systemUserId = 0;

            // Check Login
            if (User.Identity.IsAuthenticated)
            {
                var identity = User.Identity as FormsIdentity;

                if (identity != null)
                {
                    string userData = identity.Ticket.UserData;

                    if (!string.IsNullOrEmpty(userData))
                    {
                        var data = userData.Split('|');

                        if (data.Length == 2)
                        {
                            int userId = Convert.ToInt32(data[0]);
                            string role = data[1];

                            if (role == "APPLICANT")
                                applicantId = userId;
                            else
                                systemUserId = userId;
                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Please Login");
                return View(model);
            }

            model.ApplicantId = applicantId;
            model.UserId = systemUserId;


            // 1️⃣ Upload File
            if (ConstitutionFilePath != null)
            {
                string filePath = await _service.UploadFile(ConstitutionFilePath);

                if (string.IsNullOrEmpty(filePath))
                {
                    ModelState.AddModelError("", "File upload failed. Please try again.");
                    return View(model);
                }

                model.Constitution = new CompanyConstitutionViewModel
                {
                    ConstitutionType = "MODEL",
                    ConstitutionFilePath = filePath
                };
            }
            else
            {
                ModelState.AddModelError("", "Please upload the constitution file.");
                return View(model);
            }

            // 2️⃣ Prepare DTO for API with full JSON structure
            var dto = new CompanyRegistrationDTO
            {
                ApplicantId = model.ApplicantId == 0 ? (long?)null : model.ApplicantId,
                UserId = model.UserId == 0 ? (long?)null : model.UserId,
                CompanyName = model.CompanyName,
                RegistrationNumber = model.RegistrationNumber ?? GenerateRegistrationNumber(),
                CompanyType = model.CompanyType,
                BusinessActivity = model.BusinessActivity,
                RegisteredAddress = model.RegisteredAddress,
                RegistrationStatus = "PENDING",
                IncorporationDate = model.IncorporationDate,
                CreatedAt = DateTime.Now,

                // CompanyStakeholders
                CompanyStakeholders = model.CompanyStakeholders?.Select(s => new CompanyStakeholderDTO
                {
                    FullName = s.FullName,
                    StakeholderRole = s.StakeholderRole,
                    SharePercentage = s.SharePercentage,
                    Nationality = s.Nationality,
                    IdentityNumber = s.IdentityNumber,
                    EmailAddress = s.EmailAddress
                }).ToList(),

                // ShareCapital
                ShareCapital = new CompanyShareCapitalDTO
                {
                    AuthorizedShareCapital = model.ShareCapital?.AuthorizedShareCapital ?? 0,
                    IssuedShareCapital = model.ShareCapital?.IssuedShareCapital ?? 0,
                    PaidUpShareCapital = model.ShareCapital?.PaidUpShareCapital ?? 0,
                    ShareCurrency = model.ShareCapital?.ShareCurrency
                },

                // Shareholders
                Shareholders = model.Shareholders?.Select(s => new CompanyShareholderDTO
                {
                    ShareholderName = s.ShareholderName,
                    ShareholderType = s.ShareholderType,
                    Nationality = s.Nationality,
                    IdentityNumber = s.IdentityNumber,
                    NumberOfShares = s.NumberOfShares,
                    SharePercentage = s.SharePercentage,
                    EmailAddress = s.EmailAddress
                }).ToList(),

                // UHC
                UHC = model.UHC != null ? new UltimateHoldingCompanyDTO
                {
                    UHCName = model.UHC.UHCName,
                    RegistrationNumber = model.UHC.RegistrationNumber,
                    CountryOfIncorporation = model.UHC.CountryOfIncorporation,
                    OwnershipPercentage = model.UHC.OwnershipPercentage
                } : null,

                // Constitution
                Constitution = new CompanyConstitutionDTO
                {
                    ConstitutionType = model.Constitution.ConstitutionType,
                    ConstitutionFilePath = model.Constitution.ConstitutionFilePath
                }
            };

            //Submit Registration
            var response = await _service.SubmitRegistration(dto);

            if (response.IsSuccess)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", response.Result?.Message ?? "Registration failed.");
            return View(model);
        }

        // Example: RegistrationNumber generator
        private string GenerateRegistrationNumber()
        {
            return "REG-" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }

}
