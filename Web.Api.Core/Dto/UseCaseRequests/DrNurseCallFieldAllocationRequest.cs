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
    public class CallRequest : IUseCaseRequest<AcknowledgementResponse>
    {
        public string CallId { get; set; }//call_id – varchar(128)
        public string ScheduledId { get; set; }//scheduled_id – varchar(128)
        public DateTime CallScheduledDate { get; set; }//call_scheduled_date – datetime 
        public DateTime CalledDate { get; set; }//called_date – datetime
        public string CallStatus { get; set; }//call_status -  enum('Called','Pending')
        public string Remarks { get; set; }//remarks – varchar(128)
        public string EMRDone { get; set; }//emr_done - varchar(50)
        public bool IsPCRCall { get; set; }//is_pcr - tinyint(1)/bool
        public string CreatedBy { get; set; } //created_by
        public string ModifiedBy { get; set; } //modified_by
        public bool IsUpdate { get; set; }
    }
    public class CallDetails
    {
        public string CallId { get; set; }//call_id – varchar(128)
        public string ScheduledId { get; set; }//scheduled_id – varchar(128)
        public DateTime CallScheduledDate { get; set; }//call_scheduled_date – datetime 
        public DateTime CalledDate { get; set; }//called_date – datetime
        public string CallStatus { get; set; }//call_status - enum('Called','Pending')
        public string Remarks { get; set; }//remarks – varchar(128)
        public string EMRDone { get; set; }//emr_done - varchar(50)
        public string CreatedBy { get; set; } //created_by
        public string ModifiedBy { get; set; } //modified_by
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
        public string ReAllocatedTeamName { get; set; }//reallocated_team_name - varchar(100)
        public string ModifiedBy { get; set; } //modified_by
    }
    public class DrNurseCallDetails : CallDetails
    {
        public string PatientId { get; set; }//patient_id – varchar(128)
        public string PatientName { get; set; }//patient_name – varchar(128)
        public int RequestId { get; set; }//request_id - int(10)
        public string CRMNo { get; set; }//crm_no - varchar(128)
        public string EIDNo { get; set; }//eid_no - varchar(128)
        public string Area { get; set; }//area – varchar(128)
        public int CityId { get; set; }//city_id - int(10)
        public string CityName { get; set; }
        public int NationalityId { get; set; }//nationality_id – int(10)
        public string NationalityName { get; set; }
        public string MobileNo { get; set; }//mobile_no - varchar(25)
    }
}