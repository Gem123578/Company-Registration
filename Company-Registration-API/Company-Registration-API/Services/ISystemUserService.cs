using Company_Registration_API.Models;
using Company_Registration_API.Models.DTO;
using Company_Registration_API.Models.SystemUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Services
{
    public interface ISystemUserService 
    {
        ResRegSystemUser CreateUpdateSystemUser(long id, CreateUserDto dto);
        BaseResponse DeleteUser( long userId);
        //BaseResponse ConfirmEmail(string token, string email);
        ResLoginSystemUser ValidateUser(LoginDTO dto);
        ResGetAllSystemUsers GetAllSystemUsers();
        ResRegSystemUser GetSystemUserById(long id);

    }
}