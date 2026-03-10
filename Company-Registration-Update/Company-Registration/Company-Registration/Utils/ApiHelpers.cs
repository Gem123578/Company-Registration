using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration.Utils
{
    public class ApiHelpers
    {
        public static readonly string BaseUrl = "https://localhost:44378/";

        public static Uri BaseUri => new Uri(BaseUrl);
    }
}