using System;

namespace QSS.POS.Front.UI.Models.Common
{
    public class JsonResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public string View { get; set; }

        public Object Obj { get; set; }

        public bool RedirectToLogin { get; set; }
    }
}
