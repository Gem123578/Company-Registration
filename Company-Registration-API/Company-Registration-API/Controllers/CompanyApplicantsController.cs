using Company_Registration_API.DataAccess;
using Company_Registration_API.Models;
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
        private ApplicantDbContext db = new ApplicantDbContext();

        [HttpPost]
        [Route("Register")]
        public IHttpActionResult Register([FromBody] ApplicantRegisterDTO dto)
        {
            // Step 0: Check dto is not null
            if (dto == null)
                return BadRequest("Invalid applicant data.");

            // Step 1: Check duplicate email
            if (db.CompanyApplicants.Any(x => x.EmailAddress == dto.EmailAddress))
                return BadRequest("Email already exists.");

            // Step 2: Create new applicant
            var applicant = new CompanyApplicant
            {
                FullName = dto.FullName,
                EmailAddress = dto.EmailAddress,
                PasswordHash = PasswordHashing(dto.Password),
                PhoneNumber = dto.PhoneNumber,
                Nationality = dto.Nationality,
                IdentityNumber = dto.IdentityNumber,
                CreatedAt = DateTime.Now
            };

            // Step 3: Save to database
            db.CompanyApplicants.Add(applicant);
            db.SaveChanges();

            return Ok("Registration Successful");
        }

        private string PasswordHashing(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                using (var sha256 = SHA256.Create())
                {
                    var bytes = Encoding.UTF8.GetBytes(password);
                    var hash = sha256.ComputeHash(bytes);
                    return Convert.ToBase64String(hash);
                }
            }
        }

        // ==========================
        // POST: api/CompanyApplicants/Login
        [HttpPost]
        [Route("Login")]
        public IHttpActionResult Login(ApplicantLoginDTO dto)
        {
            var applicant = db.CompanyApplicants
                .FirstOrDefault(x => x.EmailAddress == dto.EmailAddress
                                  && x.PasswordHash == dto.Password);

            if (applicant == null)
                return Unauthorized();

            return Ok(new
            {
                applicant.Id,
                applicant.FullName,
                applicant.EmailAddress
            });
        }
        // ==========================
        // GET: api/CompanyApplicants
        // ==========================
        [HttpGet]
        [Route("GetAllApplicants")]
        public IHttpActionResult GetAll()
        {
            var list = db.CompanyApplicants.ToList();
            return Ok(list);
        }
    }
}
