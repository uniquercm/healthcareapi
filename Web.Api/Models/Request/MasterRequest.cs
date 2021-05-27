namespace Web.Api.Models.Request
{
    public class CompanyRequest
    {
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsUpdate { get; set; }
    }
}