using System;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class ReportDetails : IUseCaseRequest<AcknowledgementResponse>
    {
        public string ScheduledId { get; set; }//scheduled_id – varchar(128)
        public string PatientId { get; set; }//patient_id – varchar(128)
        public string PatientName { get; set; }//patient_name – varchar(128)
        public string CompanyId { get; set; }//company_id – varchar(128)
        public string CompanyName { get; set; }
        public int RequestId { get; set; }//request_id - int(10)
        public string RequestCrmName { get; set; }
        public string CRMNo { get; set; }//crm_no	- varchar(128)
        public string EIDNo { get; set; }//eid_no - varchar(128)
        public string MobileNo { get; set; }//mobile_no - varchar(25)
        public DateTime AssignedDate { get; set; }//assigned_date - datetime
        public int EnrolledCount { get; set; }//enrolled_count - int(25)
        public string EnrolledDetails { get; set; }//enrolled_details - varchar(500)

        public DateTime RecptionCallDate { get; set; }//reception_date - datetime
        public string RecptionCallStatus { get; set; }//reception_status - varchar(100)
        public string RecptionCallRemarks { get; set; }//reception_remarks - varchar(256)
        public string Day2CallId { get; set; }//2day_call_id – varchar(128)
        public string DrCallStatus { get; set; }
        public string DrCallRemarks { get; set; }
        public DateTime PCR4DayTestDate { get; set; }//4day_pcr_test_date - datetime
        public DateTime PCR4DaySampleDate { get; set; }//4day_pcr_test_sample_date – datetime
        public string PCR4DayResult { get; set; }//4day_pcr_test_result - enum('Negative','Positive')
        public DateTime PCR6DayTestDate { get; set; }//6day_pcr_test_date - datetime
        public DateTime PCR6DaySampleDate { get; set; }//6day_pcr_test_sample_date – datetime
        public string PCR6DayResult { get; set; }//6day_pcr_test_result - varchar(100) enum('Negative','Positive')
        public DateTime PCR8DayTestDate { get; set; }//8day_pcr_test_date - datetime
        public DateTime PCR8DaySampleDate { get; set; }//8day_pcr_test_sample_date – datetime
        public string PCR8DayResult { get; set; }//8day_pcr_test_result - enum('Negative','Positive')
        public DateTime PCR11DayTestDate { get; set; }//11day_pcr_test_date - datetime
        public DateTime PCR11DaySampleDate { get; set; }//11day_pcr_test_sample_date – datetime
        public string PCR11DayResult { get; set; }//11day_pcr_test_result - varchar(100) enum('Negative','Positive')
        public string Day3CallId { get; set; }//3day_call_id – varchar(128)
        public string Day3CallStatus { get; set; }
        public string Day3CallRemarks { get; set; }
        public string Day4CallId { get; set; }//4day_call_id – varchar(128)
        public string Day4CallStatus { get; set; }
        public string Day4CallRemarks { get; set; }
        public string Day5CallId { get; set; }//5day_call_id – varchar(128)
        public string Day5CallStatus { get; set; }
        public string Day5CallRemarks { get; set; }
        public string Day6CallId { get; set; }//6day_call_id – varchar(128)
        public string Day6CallStatus { get; set; }
        public string Day6CallRemarks { get; set; }
        public string Day7CallId { get; set; }//7day_call_ id – varchar(128)
        public string Day7CallStatus { get; set; }
        public string Day7CallRemarks { get; set; }
        public string Day9CallId { get; set; }//9day_call_id – varchar(128)
        public string Day9CallStatus { get; set; }
        public string Day9CallRemarks { get; set; }
        public DateTime DischargeDate { get; set; }//discharge_date – datetime
        public string DischargeStatus { get; set; }//discharge_status - varchar(100)
        public string DischargeRemarks { get; set; }//discharge_remarks - varchar(128)
        public string IsExtractTreatementDate { get; set; }//have_treatement_extract - varchar(25) Yes or No
        public string IsSendClaim { get; set; }//have_send_claim - varchar(25)Yes or No
        public DateTime SendingClaimDate { get; set; }//claim_send_date - datetime
        public string ModifiedBy { get; set; } //modified_by
    }
}