using Company_Registration.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Company_Registration.Utils
{
    public interface IAPIAccessHelper
    {
        Task<ResponseDto> SendRequestAsync(RequestDto request);
    }
}