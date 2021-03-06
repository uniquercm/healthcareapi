namespace Web.Api.Models.Request
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class AreaRequest
    {
        public int AreaId { get; set; }
        public int RegionId { get; set;}
        public string RegionName { get; set; }
        public string AreaName { get; set; }
    }
    public class UserRequest
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserType { get; set; }
        public string CompanyId { get; set; }
        public string AreaList { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsUpdate { get; set; }
    }
}
