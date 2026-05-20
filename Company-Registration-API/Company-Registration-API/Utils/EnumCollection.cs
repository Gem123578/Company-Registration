namespace Company_Registration_API.Utils
{
    public class EnumCollection 
    {
        public enum CompanyTypeEnum : byte
        {
            LLC = 1,
            PLC = 2,
            Partnership = 3
        }

        public enum RegistrationStatusEnum : byte
        {
            Pending = 1,
            Approved = 2,
            Rejected = 3
        }
    }
}