namespace Web.Api.Models.Request
{
    public class MasterRequest
    {
        public int MasterId { get; set; }
        public string CompanyId { get; set; }
        public string TeamName { get; set; }
        public int NumberOfTeam { get; set; }
        //public int QuarantineDay { get; set; }
        //public int IsolationDay { get; set; }
        //public int PCRDay { get; set; }
        public bool IsUpdate { get; set; }
    }
    public class CompanyRequest
    {
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int NumberOfTeam { get; set; }
        public string TeamName { get; set; }
        public string Address { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsUpdate { get; set; }
    }
}