using Company_Registration_API.DataAccess;
using Company_Registration_API.Models;
using Company_Registration_API.Models.CompanyApplicant;
using Company_Registration_API.Models.CompanyApplicant.Request;
using Company_Registration_API.Services;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;

namespace Company_Registration_API.Controllers
{
    [RoutePrefix("api/CompanyApplicants")]
    public class CompanyApplicantsController : ApiController
    {
        private readonly ICompanyApplicantService _service;

        public CompanyApplicantsController(ICompanyApplicantService service)
        {
            _service = service;
        }

        // ==========================
        // Register Applicant
        // ==========================
        [HttpPost]
        [Route("Register")]
        public IHttpActionResult Register([FromBody] ApplicantRegisterDTO dto)
        {
            if (dto == null)
                return BadRequest("Invalid applicant data.");

            var response = _service.Register(dto);
            return Ok(response);
        }

        // ==========================
        // Login Applicant
        // ==========================
        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login([FromBody] ApplicantLoginDTO dto)
        {
            if (dto == null) return BadRequest("Email and password are required");

            var response = _service.Login(dto);
            return Ok(response);
        }
        [HttpGet]
        [Route("confirm-email")]
        public IHttpActionResult ConfirmEmail(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
                return BadRequest("Invalid confirmation data");

            var response = _service.ConfirmEmail(token, email);

            return Ok(response);
        }

        //    // ==========================
        //    // Get All Applicants
        //    // ==========================
        //    [HttpGet]
        //    [Route("GetAllApplicants")]
        //    public IHttpActionResult GetAll()
        //    {
        //        var response = _service.GetAllApplicants();
        //        return Ok(response);
        //    }
        //}
    }
}
