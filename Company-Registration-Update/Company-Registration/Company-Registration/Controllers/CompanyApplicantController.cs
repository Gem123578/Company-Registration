using Company_Registration.APIServices;
using Company_Registration.Models;
using Company_Registration.Models.DTO;
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
        private readonly ICompanyApplicantService _service;

        public CompanyApplicantController()
        {
            _service = new CompanyApplicantService();
        }

        // GET: Register
        [HttpGet]
        public ActionResult Register()
        {
            return View(new CompanyApplicantViewModel());
        }

        // POST: Register
        [HttpPost]
        public async Task<ActionResult> Register(CompanyApplicantViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var request = new ApplicantRegisterDTO
            {
                FullName = model.FullName,
                EmailAddress = model.EmailAddress,
                Password = model.PasswordHash,
                PhoneNumber = model.PhoneNumber,
                Nationality = model.Nationality,
                IdentityNumber = model.IdentityNumber
            };

            var response = await _service.RegisterUser(request);

            if (response.IsSuccess)
                return RedirectToAction("Login","CompanyApplicant");

            ModelState.AddModelError("", string.IsNullOrEmpty(response.Message) ? "Registration Fail!" : response.Message);
            return View(model);
        }

        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["ApplicantId"] != null || User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View(new LoginViewModel());
        }

        // POST: Login
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) return View(model);

                var request = new LoginDTO
                {
                    EmailAddress = model.EmailAddress,
                    Password = model.Password
                };

                var response = await _service.LoginUser(request);

                if (!response.IsSuccess)
                {
                    ModelState.AddModelError("", string.IsNullOrEmpty(response.Message) ? "Invalid email or password" : response.Message);
                    return View(model);
                }

                //  Deserialize as common DTO
                var user = JsonConvert.DeserializeObject<LoginUserDTO>(response.Data.ToString());

                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid data returned from server.");
                    return View(model);
                }

                // 🔸 Save Session
                Session["UserId"] = user.UserId;
                Session["UserName"] = user.UserName;
                Session["UserRole"] = user.UserRole;

                // 🔸 FormsAuthentication
                var authTicket = new FormsAuthenticationTicket(
                                    1,
                                    user.UserName,
                                    DateTime.Now,
                                    model.RememberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddMinutes(30),
                                    model.RememberMe,
                                    user.UserRole // 🔥 store role
                                    );

                var encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket));

                // Redirect by role
                if (user.UserRole == "ADMIN" || user.UserRole == "OFFICER")
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (user.UserRole == "APPLICANT")
                {
                    return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("Index", "Home");

            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return View(model);
            }
            
        }


        // Logout
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        [HttpGet]
        public async Task<ActionResult> ConfirmEmail(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                ViewBag.Message = "Invalid confirmation link";
                return View();
            }

            var response = await _service.ConfirmEmail(token, email);

            ViewBag.Message = response.IsSuccess? "Email confirmed successfully. You can login now."
                : response.Message;

            return View();
        }
    }
}