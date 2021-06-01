using System;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class ReportDetails
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
        public string RecptionCallStatus { get; set; }
        public string RecptionCallRemarks { get; set; }
        public string Day2CallId { get; set; }//2day_call_ id – varchar(128)
        public string DrCallStatus { get; set; }
        public string DrCallRemarks { get; set; }
        public string PCR4DaySampleDate { get; set; }//4day_pcr_test_sample_date – datetime
        public string PCR4DayResult { get; set; }//4day_pcr_test_result - enum('Negative','Positive')
        public DateTime PCR8DaySampleDate { get; set; }//8day_pcr_test_sample_date – datetime
        public string PCR8DayResult { get; set; }//8day_pcr_test_result - enum('Negative','Positive')
        public string Day3CallId { get; set; }//3day_call_ id – varchar(128)
        public string Day3CallStatus { get; set; }
        public string Day5CallId { get; set; }//5day_call_ id – varchar(128)
        public string Day5CallStatus { get; set; }
        public string Day6CallId { get; set; }//6day_call_ id – varchar(128)
        public string Day6CallStatus { get; set; }
        public string Day7CallId { get; set; }//7day_call_ id – varchar(128)
        public string Day7CallStatus { get; set; }
        public string Day9CallId { get; set; }//9day_call_ id – varchar(128)
        public string Day9CallStatus { get; set; }
        public DateTime DischargeDate { get; set; }//discharge_date – datetime
        public string DischargeStatus { get; set; }
        public string IsExtractData { get; set; }//Yes or No
        public string IsSentClaim { get; set; }//Yes or No
        public DateTime SendingClaimDate { get; set; }
    }
}