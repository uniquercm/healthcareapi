using System;

namespace Web.Api.Models.Request
{
    public class PatientStaffRequest
    {
        public string PatientStaffId { get; set; }
        public string PatientId { get; set; }
	    public string StaffId { get; set; }
        public int AdultsCount { get; set; }
        public int ChildrensCount { get; set; }
        public DateTime ScheduledFromDate { get; set; }
        public DateTime ScheduledToDate { get; set; }
        public DateTime CalledDate { get; set; }
        public string CallStatus { get; set; }
        public string Remarks { get; set; }
        public string EMRDone { get; set; }
        public string VisitStatus { get; set; }
        public DateTime VisitedDate { get; set; }
        public string ReallocatedTeamName { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsUpdate { get; set; }
    }
}