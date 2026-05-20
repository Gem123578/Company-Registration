using System.Collections.Generic;

namespace Company_Registration_API.Models.DTO
{
    public class LoginUserDto
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public List<string> Functions { get; set; }
    }
}