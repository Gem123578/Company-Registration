using Company_Registration_API.Models;
using Company_Registration_API.Services;
using log4net;
using Newtonsoft.Json;
using System.Web.Http;

namespace Company_Registration_API.Controllers
{
    [RoutePrefix("api/CompanyRegistration")]
    public class CompanyRegistrationController : ApiController
    {
        private readonly ICompanyRegistrationService _service;
        private readonly ILog _logger;
        public CompanyRegistrationController()
        {
            _service = new CompanyRegistrationService();
            _logger = LogManager.GetLogger(typeof(CompanyRegistrationController));
        }


        //post submit company registration form
        [HttpPost]
        [Route("Submit")]
        public IHttpActionResult SubmitRegistration([FromBody]CompanyRegistrationDTO dto)
        {
       
            _logger.Debug("api/CompanyRegistratioin/Submit");
            _logger.Debug(JsonConvert.SerializeObject(dto));
            var response = _service.SubmitCompanyRegistration(dto);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetCompany")]
        public IHttpActionResult GetCompany(long userId)
        {
            _logger.Debug(string.Format("api/CompanyRegistratioin/GetCompany/{0}", userId));
            var response = _service.GetAllCompanies(userId);
            return Ok(response);
        }

        [HttpGet]
        [Route("Get/{id}")]
        public IHttpActionResult GetById(long id)
        {
            _logger.Debug(string.Format("api/CompanyRegistratioin/Get/{0}", id));
            var response = _service.GetCompanyById(id);
            return Ok(response);
        }

        [HttpPost]
        [Route("Update/{id}")]
        public IHttpActionResult Update(long id, [FromBody] CompanyRegistrationDTO dto)
        {
            _logger.Debug(string.Format("api/CompanyRegistratioin/Update/{0}", id));
            _logger.Debug(JsonConvert.SerializeObject(dto));
            var result = _service.UpdateCompany(id, dto);
            return Ok(result);
        }

        [HttpPost]
        [Route("Delete/{id}")]
        public IHttpActionResult Delete(long id)
        {
            _logger.Debug(string.Format("api/CompanyRegistratioin/Delete/{0}", id));
            var result = _service.DeleteCompany(id);
            return Ok(result);
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