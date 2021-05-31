using System;
//using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class ScheduledRequest : IUseCaseRequest<AcknowledgementResponse>
    {
        public string ScheduledId { get; set; }//scheduled_id – varchar(128)
        public string PatientStaffId { get; set; }//patient_staff_id – varchar(128)
        public string PatientId { get; set; }//patient_id – varchar(128)
        public DateTime PCRTestDate { get; set; }//initial_pcr_test_date – datetime
        public string PCRResult { get; set; }//initial_pcr_test_result - varchar(100) enum('Negative','Positive')
        public string HaveVaccine { get; set; }//have_vaccine - varchar(64)
        public string AllocatedTeamName { get; set; }//allocated_team_name - varchar(100)
        public string ReAllocatedTeamName { get; set; }//reallocated_team_name - varchar(100)
        public DateTime DischargeDate { get; set; }//discharge_date – datetime
        public string TreatmentType { get; set; }//treatment_type - varchar(100) enum('Quarantine','Isolation')
        public DateTime TreatmentFromDate { get; set; }//treatment_from_date - datetime
        public DateTime TreatmentToDate { get; set; }//treatment_to_date – datetime
        public DateTime PCR4DayTestDate { get; set; }//4day_pcr_test_date – datetime
        public DateTime PCR4DaySampleDate { get; set; }//4day_pcr_test_sample_date – datetime
        public string PCR4DayResult { get; set; }//4day_pcr_test_result - varchar(100) enum('Negative','Positive')
        public DateTime PCR8DayTestDate { get; set; }//8day_pcr_test_date - datetime
        public DateTime PCR8DaySampleDate { get; set; }//8day_pcr_test_sample_date – datetime
        public string PCR8DayResult { get; set; }//8day_pcr_test_result - varchar(100) enum('Negative','Positive')
        public DateTime FirstCallScheduledDate { get; set; }
        public string Day2CallId { get; set; }//2day_call_id – varchar(128)
        public string Day3CallId { get; set; }//3day_call_id – varchar(128)
        public string Day5CallId { get; set; }//5day_call_id – varchar(128)
        public string Day6CallId { get; set; }//6day_call_id – varchar(128)
        public string Day7CallId { get; set; }//7day_call_id – varchar(128)
        public string Day9CallId { get; set; }//9day_call_id – varchar(128)
        public string CreatedBy { get; set; } //created_by
        public string ModifiedBy { get; set; } //modified_by
        public bool IsUpdate { get; set; }
    }
    public class ScheduledDetails
    {
        public string ScheduledId { get; set; }//scheduled_id – varchar(128)
        public string PatientStaffId { get; set; }//patient_staff_id – varchar(128)
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        //public List<StaffDetails> StaffDetailsList { get; set; }
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public PatientDetails PatientInformation { get; set; }
        public DateTime PCRTestDate { get; set; }//initial_pcr_test_date – datetime
        public string PCRResult { get; set; }//initial_pcr_test_result - enum('Negative','Positive')
        public string HaveVaccine { get; set; }//have_vaccine - varchar(64)
        public string AllocatedTeamName { get; set; }//allocated_team_name - varchar(100)
        public string ReAllocatedTeamName { get; set; }//reallocated_team_name - varchar(100)
        public DateTime DischargeDate { get; set; }//discharge_date – datetime
        public string TreatmentType { get; set; }//treatment_type - enum('Quarantine','Isolation')
        public DateTime TreatmentFromDate { get; set; }//treatment _from_date - datetime
        public DateTime TreatmentToDate { get; set; }//treatment _to_date – datetime
        public DateTime PCR4DayTestDate { get; set; }//4day_pcr_test_date – datetime
        public DateTime PCR4DaySampleDate { get; set; }//4day_pcr_test_sample_date – datetime
        public string PCR4DayResult { get; set; }//4day_pcr_test_result - enum('Negative','Positive')
        public DateTime PCR8DayTestDate { get; set; }//8day_pcr_test_date - datetime
        public DateTime PCR8DaySampleDate { get; set; }//8day_pcr_test_sample_date – datetime
        public string PCR8DayResult { get; set; }//8day_pcr_test_result - enum('Negative','Positive')
        public string Day2CallId { get; set; }//2day_call_ id – varchar(128)
        public CallDetails Day2CallDetails { get; set; }
        public string Day3CallId { get; set; }//3day_call_ id – varchar(128)
        public CallDetails Day3CallDetails { get; set; }
        public string Day5CallId { get; set; }//5day_call_ id – varchar(128)
        public CallDetails Day5CallDetails { get; set; }
        public string Day6CallId { get; set; }//6day_call_ id – varchar(128)
        public CallDetails Day6CallDetails { get; set; }
        public string Day7CallId { get; set; }//7day_call_ id – varchar(128)
        public CallDetails Day7CallDetails { get; set; }
        public string Day9CallId { get; set; }//9day_call_ id – varchar(128)
        public CallDetails Day9CallDetails { get; set; }
        public string CreatedBy { get; set; } //created_by
        public string ModifiedBy { get; set; } //modified_by
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
    }
    public class FieldAllocationRequest : IUseCaseRequest<AcknowledgementResponse>
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
        public string MobileNo { get; set; }//mobile_no - varchar(25)
    }
}