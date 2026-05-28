using Company_Registration.APIServices;
using Company_Registration.Common;
using Company_Registration.Models;
using Company_Registration.Models.DTO;
using Company_Registration.Utils;
using Newtonsoft.Json;
using QSS.POS.Front.UI.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
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
        [HttpGet]
        public ActionResult EmailConfirmed(string email)
        {
            ViewBag.Email = email;
            ViewBag.Message = "Please confirm your email.";

            return View();
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

            ResponseDto response = await _service.RegisterUser(request);

            if (response.IsSuccess)
            {
                return RedirectToAction(
                    "EmailConfirmed",
                    new { email = model.EmailAddress }
                );
            }

            ModelState.AddModelError("", string.IsNullOrEmpty(response.Result?.Message) ? "Registration Fail!" : response.Result?.Message);
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
                    ModelState.AddModelError("", string.IsNullOrEmpty(response.Result?.Message) ? "Invalid email or password" : response.Result?.Message);
                    return View(model);
                }

                var user = JsonConvert.DeserializeObject<LoginUserDTO>(response.Data.ToString());

                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid data returned from server.");
                    return View(model);
                }

                //  Save Session
                Session["UserId"] = user.UserId;
                Session["UserName"] = user.UserName;
                Session["UserRole"] = user.UserRole;
                Session["Functions"] = user.Functions;

                // Applicant session
                if (user.UserRole == "APPLICANT")
                {
                    Session["ApplicantId"] = user.UserId;
                }

                //  Store UserId + Role in Ticket
                string userData = user.UserId + "|" + user.UserRole;

                var authTicket = new FormsAuthenticationTicket(
                    1,
                    user.UserName,
                    DateTime.Now,
                    model.RememberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddMinutes(30),
                    model.RememberMe,
                    userData
                );

                var encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                Response.Cookies.Add(
                    new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
                );

                //  Redirect by Role
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
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error: " + ex.Message);
                return View(model);
            }
        }


        public ActionResult ResendConfirmationEmail()
        {
            return View();
        }
        // Logout
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult EmailConfirmSuccessful()
        {
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> ConfirmEmail(string token, string email)
        {
            var response = await _service.ConfirmEmail(token, email);

            if (response.IsSuccess)
            {
                return RedirectToAction("EmailConfirmSuccessful");
            }

            if (response.Result.Message == "Token expired.")
            {
                TempData["TokenExpired"] = true;
                TempData["ExpiredEmail"] = email;

                return RedirectToAction("ResendConfirmationEmail", "CompanyApplicant");
            }

            TempData["ErrorMessage"] = response.Result.Message;

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> ResendConfirmation(string email)
        {
            var response = await _service.ResendConfirmation(email);

            if (response.IsSuccess)
            {
                return RedirectToAction(
                    "EmailConfirmed",
                    new { email = email }
                );
            }
            TempData["ErrorMessage"] = response.Result?.Message ?? "Resend failed.";

            return RedirectToAction("ResendConfirmationEmail");
        }
    }
}