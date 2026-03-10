using Company_Registration.Models;
using Company_Registration.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Company_Registration.Controllers
{
    public class CompanyRegistrationController : Controller
    {
        // GET: Register Form
        public ActionResult Register()
        {
            var model = new CompanyRegistrationViewModel();

            model.Shareholders = new System.Collections.Generic.List<CompanyShareholderViewModel>();
            model.Shareholders.Add(new CompanyShareholderViewModel());

            return View(model);
        }

        // =========================
        // Upload File API Call
        // =========================
        private async Task<string> UploadFile(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
                return null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = ApiHelpers.BaseUri;

                using (var content = new MultipartFormDataContent())
                {
                    var fileContent = new StreamContent(file.InputStream);

                    fileContent.Headers.ContentDisposition =
                        new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                        {
                            Name = "file",
                            FileName = file.FileName
                        };

                    content.Add(fileContent);

                    var response = await client.PostAsync("api/CompanyRegistration/upload", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();

                        dynamic data = JsonConvert.DeserializeObject(result);

                        return data.path;
                    }
                }
            }

            return null;
        }


        // =========================
        // POST: Submit Registration
        // =========================
        [HttpPost]
        public async Task<ActionResult> Register(
            CompanyRegistrationViewModel model,
            HttpPostedFileBase ConstitutionFilePath)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.ApplicantId = Convert.ToInt32(Session["ApplicantId"]);

            // 1️⃣ Upload File First
            if (ConstitutionFilePath != null)
            {
                string path = await UploadFile(ConstitutionFilePath);

                model.Constitution = new CompanyConstitutionViewModel
                {
                    ConstitutionType = "MODEL",
                    ConstitutionFilePath = path
                };
            }

            // 2️⃣ Submit Registration API
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = ApiHelpers.BaseUri;

                var json = JsonConvert.SerializeObject(model);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response =
                    await client.PostAsync("api/CompanyRegistration/Submit", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Registration failed");
                return View(model);
            }
        }

    }
}