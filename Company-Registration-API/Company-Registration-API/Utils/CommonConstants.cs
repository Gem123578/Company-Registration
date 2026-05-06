using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Company_Registration_API.Utils
{
    public class CommonConstants
    {
        public const string TBLNAME_USERS = "System User";
        public const string TBLNAME_APP_USERS = "Applicant User";
        public const string TBLNAME_EMAIL_TOKEN = "Email Token";
        public const string API_START = "API START : {0}";

        public const string API_END = "API END : {0}";

        public static int MAX_FULLNAME_LENGHT = 50;

        public static int MAX_EMAIL_LENGHT = 100;

        public static int MAX_TOKEN_LENGHT = 500;
        public static int MAX_Phno_LENGHT = 20;
        public static int MAX_NRC_LENGHT = 30;
        public static int MAX_NATIONALITY_LENGHT = 50;

        public static string EMAIL_PATTERN = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        public static string PASSWORD_PATTERN = @"^(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[a-z\d@$!%*?&]{6,20}$";

        public static string TOKEN_PATTERN = @"^[A-Za-z0-9\-_]{10,200}$";

        
    }
}