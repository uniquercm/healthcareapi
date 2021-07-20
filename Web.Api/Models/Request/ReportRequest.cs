using System;
using System.Collections.Generic;

namespace Web.Api.Models.Request
{
    public class ReportDetails
    {
        public string ScheduledId { get; set; }
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int RequestId { get; set; }
        public string RequestCrmName { get; set; }
        public string CRMNo { get; set; }
        public string EIDNo { get; set; }
        public string MobileNo { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime RecptionCallDate { get; set; }
        public string RecptionCallStatus { get; set; }
        public string RecptionCallRemarks { get; set; }
        public string Day2CallId { get; set; }
        public string DrCallStatus { get; set; }
        public string DrCallRemarks { get; set; }
        public DateTime PCR4DayTestDate { get; set; }
        public DateTime PCR4DaySampleDate { get; set; }
        public string PCR4DayResult { get; set; }
        public DateTime PCR8DayTestDate { get; set; }
        public DateTime PCR8DaySampleDate { get; set; }
        public string PCR8DayResult { get; set; }
        public string Day3CallId { get; set; }
        public string Day3CallStatus { get; set; }
        public string Day3CallRemarks { get; set; }
        public string Day4CallId { get; set; }
        public string Day4CallStatus { get; set; }
        public string Day4CallRemarks { get; set; }
        public string Day5CallId { get; set; }
        public string Day5CallStatus { get; set; }
        public string Day5CallRemarks { get; set; }
        public string Day6CallId { get; set; }
        public string Day6CallStatus { get; set; }
        public string Day6CallRemarks { get; set; }
        public string Day7CallId { get; set; }
        public string Day7CallStatus { get; set; }
        public string Day7CallRemarks { get; set; }
        public string Day9CallId { get; set; }
        public string Day9CallStatus { get; set; }
        public string Day9CallRemarks { get; set; }
        public DateTime DischargeDate { get; set; }
        public string DischargeStatus { get; set; }
        public string DischargeRemarks { get; set; }
        public string IsExtractTreatementDate { get; set; }
        public string IsSendClaim { get; set; }
        public DateTime SendingClaimDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}