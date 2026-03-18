using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Utils
{
    public class ApiException : Exception
    {
        public ApiException(string message) : base(message) { }
    }
}