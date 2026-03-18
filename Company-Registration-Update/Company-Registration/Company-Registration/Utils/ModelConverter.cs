using Company_Registration.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration.Utils
{
    public class ModelConverter
    {
        internal static RequestDto CreateRequestDto(Object data, string rootUrl,
            string apiUrl, eHTTPRequestType requestType, string token = null)
        {
            RequestDto request = new RequestDto();
            request.RequestUrl = string.Format("{0}/{1}", rootUrl.TrimEnd('/'), apiUrl.TrimStart('/'));
            request.RequestType = requestType;
            request.AccessToken = token;

            if (data != null)
            {
                request.Data = data;
            }

            return request;
        }
    }
}