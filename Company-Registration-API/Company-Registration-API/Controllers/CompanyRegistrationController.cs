using Company_Registration_API.DataAccess;
using Company_Registration_API.Models;
using Company_Registration_API.Services;
using Company_Registration_API.Utils;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace Company_Registration_API.Controllers
{
    [RoutePrefix("api/CompanyRegistration")]
    public class CompanyRegistrationController : ApiController
    {
        private readonly ICompanyRegistrationService _service;
        public CompanyRegistrationController(ICompanyRegistrationService service)
        {
            _service = service;
        }


        //post submit company registration form
        [HttpPost]
        [Route("Submit")]
        public IHttpActionResult SubmitRegistration([FromBody]CompanyRegistrationDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Registration data is null.");
                if (dto.ApplicantId == 0)
                    return BadRequest("ApplicantId missing.");

                // register call
                var response = _service.SubmitCompanyRegistration(dto);
                return Ok(response);
            }
            catch(ApiException ex)
            {
                return Content(System.Net.HttpStatusCode.BadRequest, new
                {
                    IsSuccess = false,
                    message = ex.Message
                });
            }
        }

        // post upload constitution document
        [HttpPost]
        [Route("upload")]
        public IHttpActionResult UploadConstitution()
        {
            var response = _service.UploadConstitution();
            return Ok(response);

        }
    }
}