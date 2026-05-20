using Company_Registration_API.Models;
using Company_Registration_API.Models.CompanyApplicant;
using Company_Registration_API.Models.DTO;
using Company_Registration_API.Models.SystemUser;
using Company_Registration_API.Services;
using log4net;
using Newtonsoft.Json;
using System.Web.Http;

namespace Company_Registration_API.Controllers
{
    [RoutePrefix("api/SystemUser")]
    public class SystemUserController : ApiController
    {
        private readonly ISystemUserService _systemUserService;
        private readonly ILog _logger;

        public SystemUserController()
        {
            _systemUserService = new SystemUserService();
            _logger = LogManager.GetLogger(typeof(SystemUserController));
        }

        // Get All System Users
        [HttpGet]
        [Route("GetAllUsers")]
        public IHttpActionResult GetAllUsers()
        {
            _logger.Debug("api/SystemUser/GetAllUsers");
            var users = _systemUserService.GetAllSystemUsers();
            return Ok(users);
        }
        // GET: api/SystemUser/GetUserById/5
        [HttpGet]
        [Route("CreateUser/{id}")]
        public IHttpActionResult GetUserById(long id)
        {
            _logger.Debug(string.Format("api/SystemUser/CreaterUser/{0}", id));
            var user = _systemUserService.GetSystemUserById(id); 
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }


        //Create/Update System User
        [HttpPost]
        [HttpPut]
        [Route("CreateUser")]
        public IHttpActionResult CreateUser([FromBody] CreateUserDto dto)
        {
            _logger.Debug("api/SystemUser/CreateUser");
            _logger.Debug(JsonConvert.SerializeObject(dto));
            long loginUserId = dto.Id;

            // call service
            ResRegSystemUser user = _systemUserService.CreateUpdateSystemUser(loginUserId, dto);
            return Ok(user);

        }

        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login([FromBody] LoginDTO dto)
        {
            _logger.Debug("api/SystemUser/Login");
            _logger.Debug(JsonConvert.SerializeObject(dto));
            var response = _systemUserService.ValidateUser(dto);

            return Ok(response);
        }

        
        [HttpPost]
        [Route("DeleteUser/{id}")]
        public IHttpActionResult DeleteUser( long id)
        {
            _logger.Debug(string.Format("api/SystemUser/DeleteUser/{0}", id));
            ResultBase response = _systemUserService.DeleteUser(id);

            return Ok(response);
        }
    }
}