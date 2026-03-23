using Company_Registration_API.Models.DTO;
using Company_Registration_API.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Models.SystemUser
{
    public class ResSystemUser : BaseResponse
    {
        [JsonProperty("data")]
        public CreateUserDto Data { get; set; }
    }
}