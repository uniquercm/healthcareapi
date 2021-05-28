using System;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class PatientStaffRequest : IUseCaseRequest<AcknowledgementResponse>
    {
        public string PatientStaffId { get; set; }//patient_staff_id – varchar(128)
        public string PatientId { get; set; }//patient_id – varchar(128)
	    public string StaffId { get; set; }//staff_id – varchar(128)
        public int AdultsCount { get; set; }//no_of_adults – int(10)
        public int ChildrensCount { get; set; }//no_of_childrens – int(10)
        public DateTime ScheduledFromDate { get; set; }//scheduled_from_date - datetime
        public DateTime ScheduledToDate { get; set; }//scheduled_to_date – datetime
        public DateTime CalledDate { get; set; }//called_date – datetime
        public string CallStatus { get; set; }//call_status - enum('Called','Pending')
        public string Remarks { get; set; }//remarks – varchar(128)
        public string EMRDone { get; set; }//emr_done - enum('No','Yes')
        public string VisitStatus { get; set; }//visit_status - enum('Visited','Pending') 
        public DateTime VisitedDate { get; set; }//visited_date – datetime
        public string ReallocatedTeamName { get; set; }//reallocated_team_name – varchar(128)
        public string CreatedBy { get; set; } //created_by
        public string ModifiedBy { get; set; } //modified_by
        public bool IsUpdate { get; set; }
    }
    public class PatientStaffDetails
    {
        public string PatientStaffId { get; set; }//patient_staff_id – varchar(128)
        public string PatientId { get; set; }//patient_id – varchar(128)
        public string PatientName { get; set; }
	    public string StaffId { get; set; }//staff_id – varchar(128)
        public string StaffName { get; set; }
        public int AdultsCount { get; set; }//no_of_adults – int(10)
        public int ChildrensCount { get; set; }//no_of_childrens – int(10)
        public DateTime ScheduledFromDate { get; set; }//scheduled_from_date - datetime
        public DateTime ScheduledToDate { get; set; }//scheduled_to_date – datetime
        public DateTime CalledDate { get; set; }//called_date – datetime
        public string CallStatus { get; set; }//call_status - enum('Called','Pending')
        public string Remarks { get; set; }//remarks – varchar(128)
        public string EMRDone { get; set; }//emr_done - enum('No','Yes')
        public string VisitStatus { get; set; }//visit_status - enum('Visited','Pending') 
        public DateTime VisitedDate { get; set; }//visited_date – datetime
        public string ReallocatedTeamName { get; set; }//reallocated_team_name – varchar(128)
        public string CreatedBy { get; set; } //created_by
        public string ModifiedBy { get; set; } //modified_by
    }
}