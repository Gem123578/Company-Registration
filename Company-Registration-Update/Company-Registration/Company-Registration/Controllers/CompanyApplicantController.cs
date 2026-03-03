using Company_Registration.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Company_Registration.Controllers
{
    public class CompanyApplicantController : Controller
    {
        // GET: Show Registeration Form
        public ActionResult Register()
        {
            return View();
        }

        //Post:Submit Registeration Form
        [HttpPost]
        public async Task<ActionResult> Register(CompanyApplicantViewModel applicant)
        {
            if (!ModelState.IsValid)
            {
                return View(applicant);
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44378/");

                var json = JsonConvert.SerializeObject(applicant);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(
                    "api/CompanyApplicants/Register",
                    content
                );
            }

            ModelState.AddModelError("", "An error occurred while processing your registration. Please try again.");
            return View(applicant);
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}