namespace Web.Api.Models.Request
{
    public class StaffRequest
    {
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public string StaffType { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        public string CompanyId { get; set; }
        public string TeamName { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsUpdate { get; set; }
    }
}