using Company_Registration_API.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Services
{
    public class BaseServices
    {
        protected Result CreateResult(string code, string message = null)
        {
            Result result = new Result();

            result.Code = code;
            result.Message = message;
            return result;
        }
    }
}