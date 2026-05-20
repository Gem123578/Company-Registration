using System;

namespace Company_Registration_API.Utils
{
    public class ApiException : Exception
    {
        public ApiException(string message) : base(message) { }
    }
}