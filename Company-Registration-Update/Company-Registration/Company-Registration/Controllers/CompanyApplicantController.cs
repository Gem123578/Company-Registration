using Company_Registration.Models;
using Company_Registration.Utils;
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
using System.Web.Security;

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
                client.BaseAddress = ApiHelpers.BaseUri;

                var json = JsonConvert.SerializeObject(applicant);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("api/CompanyApplicants/Register", content);
                if (response.IsSuccessStatusCode)
                {
                    // Redirect to Login page after successful registration
                    return RedirectToAction("Login", "CompanyApplicant");
                }
            }

            ModelState.AddModelError("", "An error occurred while processing your registration. Please try again.");
            return View(applicant);
        }

        public ActionResult Success()
        {
            return View();
        }
        // GET: Show Login Form
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["ApplicantId"] != null || User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        // GET: Show Login Form
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = ApiHelpers.BaseUri;

                var loginData = new
                {
                    EmailAddress = model.EmailAddress,
                    Password = model.Password
                };

                var content = new StringContent(
                    JsonConvert.SerializeObject(loginData),
                    Encoding.UTF8,
                    "application/json"
                );

                HttpResponseMessage response = await client.PostAsync("api/CompanyApplicants/Login", content);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(json);

                    // Session ထဲသိမ်း
                    Session["ApplicantId"] = data.Id;
                    Session["UserName"] = data.FullName;

                    // ✅ Full Name ဖြင့် authentication ticket ပြုလုပ်
                    var authTicket = new FormsAuthenticationTicket(
                        1,
                        data.FullName.ToString(),      // Razor page မှာ @User.Identity.Name အတွက်
                        DateTime.Now,
                        DateTime.Now.AddMinutes(30),
                        true,                           // Remember Me
                        data.EmailAddress.ToString()    // optional UserData
                    );

                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
                    {
                        HttpOnly = true,
                        Expires = DateTime.Now.AddMinutes(30)
                    };

                    Response.Cookies.Add(authCookie);

                    return RedirectToAction("Index", "Home");
                }


                else
                {
                    ModelState.AddModelError("", "Invalid email or password");
                    return View(model);
                }
            }
        }
        //logout 
        public ActionResult Logout()
        {
            // Clear session
            Session.Clear();
            Session.Abandon();

            // Remove authentication cookie
            FormsAuthentication.SignOut();

            return RedirectToAction("Login");
        }

    }
}