using System;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class PCRCallRequest : IUseCaseRequest<AcknowledgementResponse>
    {
        public string ScheduledId { get; set; }//scheduled_id – varchar(128)
        public string Remarks { get; set; }//remarks – varchar(128)
        public string EMRDone { get; set; }//emr_done - varchar(50)
    }
    public class CallRequest : TeamVisitDetails, IUseCaseRequest<AcknowledgementResponse>
    {
        public string CallId { get; set; }//call_id – varchar(128)
        //public string ScheduledId { get; set; }//scheduled_id – varchar(128)
        public DateTime CallScheduledDate { get; set; }//call_scheduled_date – datetime 
        public DateTime CalledDate { get; set; }//called_date – datetime
        public string CallStatus { get; set; }//call_status -  enum('Called','Pending')
        public string Remarks { get; set; }//remarks – varchar(128)
        public string EMRDone { get; set; }//emr_done - varchar(50)
        public bool IsPCRCall { get; set; }//is_pcr - tinyint(1)/bool
        public string CreatedBy { get; set; } //created_by
        //public string ModifiedBy { get; set; } //modified_by
        public bool IsUpdate { get; set; }
    }
    public class CallDetails : TeamVisitDetails
    {
        public string CallId { get; set; }//call_id – varchar(128)
        //public string ScheduledId { get; set; }//scheduled_id – varchar(128)
        public DateTime CallScheduledDate { get; set; }//call_scheduled_date – datetime 
        public DateTime CalledDate { get; set; }//called_date – datetime
        public string CallStatus { get; set; }//call_status - enum('Called','Pending')
        public string Remarks { get; set; }//remarks – varchar(128)
        public string EMRDone { get; set; }//emr_done - varchar(50)
        public string CreatedBy { get; set; } //created_by
        //public string ModifiedBy { get; set; } //modified_by
        public bool IsPCRCall { get; set; }
    }
    public class FieldAllocationRequest : IUseCaseRequest<AcknowledgementResponse>
    {
        public List<FieldAllocationDetails> FieldAllocationDetailsList { get; set; }
    }
    public class FieldAllocationDetails
    {
        public string ScheduledId { get; set; }//scheduled_id – varchar(128)
        public string PatientStaffId { get; set; }//patient_staff_id – varchar(128)
        public string PatientId { get; set; }//patient_id – varchar(128)
        public string AllocatedTeamName { get; set; }//allocated_team_name - varchar(100)
        public DateTime AllocatedDate { get; set; }//team_allocated_date - datetime
        public string ReAllocatedTeamName { get; set; }//reallocated_team_name - varchar(100)
        public DateTime ReAllocatedDate { get; set; }//team_reallocated_date - datetime
        public string ModifiedBy { get; set; } //modified_by
    }
    public class TeamVisitDetails : IUseCaseRequest<AcknowledgementResponse>
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
    public class DrNurseCallDetails : CallDetails//, TeamVisitDetails
    {
        //public string PatientId { get; set; }//patient_id – varchar(128)
        public string PatientName { get; set; }//patient_name – varchar(128)
        public int Age { get; set; }
        public int RequestId { get; set; }//request_id - int(10)
        public string RequestCrmName { get; set; }
        public string CRMNo { get; set; }//crm_no - varchar(128)
        public string EIDNo { get; set; }//eid_no - varchar(128)
        public string Area { get; set; }//area – varchar(128)
        public int CityId { get; set; }//city_id - int(10)
        public string CityName { get; set; }
        public int NationalityId { get; set; }//nationality_id – int(10)
        public string NationalityName { get; set; }
        public string MobileNo { get; set; }//mobile_no - varchar(25)
        public DateTime TrackerReplacedDate { get; set; }//tracker_replace_date - datatime
        public string TrackerReplaceNumber { get; set; }//tracker_replace_no - varchar(50)
        public string TrackerReplaceTeamUserName { get; set; }//tracker_replace_team_user_name - varchar(128)
        public string TrackerReplaceTeamStatus { get; set; }//tracker_replace_team_status - varchar(100) - enum('visited','notvisited','pending')
        public string TrackerReplaceTeamRemark { get; set; }//tracker_replace_team_remark - varchar(256)
        public DateTime TrackerReplaceTeamStatusDate { get; set; }//tracker_replace_team_date - datetime
    }
}