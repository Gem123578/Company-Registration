using Company_Registration_API.Common;

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