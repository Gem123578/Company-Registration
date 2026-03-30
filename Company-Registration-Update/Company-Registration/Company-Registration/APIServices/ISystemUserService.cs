using Company_Registration.Common;
using Company_Registration.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Company_Registration.APIServices
{
    public interface ISystemUserService 
    {
        Task<ResponseDto> CreateUpdateSystemUser(int id, CreateUpdateUserDto dto);
        Task<ResponseDto> GetAllUsers();
    }
}