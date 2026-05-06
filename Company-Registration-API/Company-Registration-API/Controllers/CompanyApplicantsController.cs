using Company_Registration_API.DataAccess;
using Company_Registration_API.Models;
using Company_Registration_API.Models.CompanyApplicant;
using Company_Registration_API.Services;
using log4net;
using Newtonsoft.Json;
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
        private readonly ILog _logger;
        public CompanyApplicantsController()
        {
            _service = new CompanyApplicantService();
            _logger = LogManager.GetLogger(typeof(CompanyApplicantsController));
        }

        // ==========================
        // Register Applicant
        // ==========================
        [HttpPost]
        [Route("Register")]
        public IHttpActionResult Register([FromBody] ApplicantRegisterDTO dto)
        {
            _logger.Debug("api/CompanyApplicants/Register");
            _logger.Debug(JsonConvert.SerializeObject(dto));
            var response = _service.Register(dto);
            return Ok(response);
        }

        [HttpGet]
        [Route("confirm-email")]
        public IHttpActionResult ConfirmEmail(string token, string email)
        {
            _logger.Debug("api/CompanyApplicants/confirm-email");
            _logger.Debug(" Email: {email}");
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
