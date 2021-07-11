using System;
using System.Collections.Generic;

namespace Web.Api.Models.Request
{
    public class CallRequest : TeamVisitDetails
    {
        public string CallId { get; set; }
        //public string ScheduledId { get; set; }
        public DateTime CallScheduledDate { get; set; }
        public DateTime CalledDate { get; set; }
        public string CallStatus { get; set; }
        public string Remarks { get; set; }
        public string EMRDone { get; set; }
        public bool IsPCRCall { get; set; }
        public string CreatedBy { get; set; }
        //public string ModifiedBy { get; set; }
        public bool IsUpdate { get; set; }
    }
    public class TeamVisitDetails
    {
        public string ScheduledId { get; set; }
        public string PatientId { get; set; }
        public string ServiceName { get; set; }
        public string TeamUserName { get; set; }
        public string TeamStatus { get; set; }
        public string TeamRemark { get; set; }
        public DateTime TeamStatusDate { get; set; }
        public bool ShowDischage { get; set;}
        public string ModifiedBy { get; set; } //modified_by
    }
    public class FieldAllocationRequest
    {
        public List<FieldAllocationDetails> FieldAllocationDetailsList { get; set; }
    }
    public class FieldAllocationDetails
    {
        public string ScheduledId { get; set; }
        public string PatientId { get; set; }
        public string AllocatedTeamName { get; set; }
        public string ReAllocatedTeamName { get; set; }
        public string ModifiedBy { get; set; }
    }

}