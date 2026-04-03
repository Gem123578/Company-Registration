using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Company_Registration_API.Services;
using Company_Registration_API.Models.DTO;
using Company_Registration_API.Models.SystemUser;
using Company_Registration_API.Models;

namespace Company_Registration_API.Controllers
{
    [RoutePrefix("api/SystemUser")]
    public class SystemUserController : ApiController
    {
        private readonly ISystemUserService _systemUserService;

        public SystemUserController()
        {
            _systemUserService = new SystemUserService();
        }

        // Get All System Users
        [HttpGet]
        [Route("GetAllUsers")]
        public IHttpActionResult GetAllUsers()
        {
            var users = _systemUserService.GetAllSystemUsers();
            return Ok(users);
        }
        // GET: api/SystemUser/GetUserById/5
        [HttpGet]
        [Route("CreateUser/{id}")]
        public IHttpActionResult GetUserById(long id)
        {
            var user = _systemUserService.GetSystemUserById(id); // Service မှာ method လည်း လိုအပ်
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
            long loginUserId = dto.Id;

            // call service
            ResRegSystemUser user = _systemUserService.CreateUpdateSystemUser(loginUserId, dto);
            return Ok(user);

        }

        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login([FromBody] LoginDTO dto)
        {
            if (dto == null) return BadRequest("Email and password are required");

            var response = _systemUserService.ValidateUser(dto);
            return Ok(response);
        }

        
        [HttpDelete]
        [Route("DeleteUser/{id}")]
        public IHttpActionResult DeleteUser(long id)
        {
            int loginUserId = 0;
            //get from session
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["UserId"] != null)
            {
                int.TryParse(HttpContext.Current.Session["UserId"].ToString(), out loginUserId);
            }

            //get from cookie
            if(loginUserId == 0 && HttpContext.Current.Request.Cookies["UserId"] != null)
            {
                int.TryParse(HttpContext.Current.Request.Cookies["UserId"].Value, out loginUserId);
            }

            //login user?
            if(loginUserId == 0)
            {
                return Content(System.Net.HttpStatusCode.Unauthorized, new BaseResponse
                {
                    IsSuccess = false,
                    Message = "User not authenticated."
                });
            }

            BaseResponse response = _systemUserService.DeleteUser(loginUserId, id);

            return Ok(response);
        }
    }
}