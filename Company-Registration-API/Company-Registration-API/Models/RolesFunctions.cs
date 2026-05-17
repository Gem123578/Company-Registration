namespace Company_Registration_API.DataAccess
{
    public class RolesFunctions
    {
        public long Id { get; set; }
        public long RoleId { get; set; }
        public long FunctionId { get; set; }
        public Functions Functions { get; set; }
    }
}