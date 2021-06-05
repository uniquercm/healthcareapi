using System;
using System.Collections.Generic;

namespace Web.Api.Models.Request
{
    public class ScheduledRequest
    {
        public string ScheduledId { get; set; }
        public string PatientStaffId { get; set; }
        public string PatientId { get; set; }
        public DateTime PCRTestDate { get; set; }
        public string PCRResult { get; set; }
        public string HaveVaccine { get; set; }
        public DateTime DischargeDate { get; set; }
        public string DischargeStatus { get; set; }
        public string DischargeRemarks { get; set; }
        public string AllocatedTeamName { get; set; }
        public string ReAllocatedTeamName { get; set; }
        public string TreatmentType { get; set; }
        public DateTime TreatmentFromDate { get; set; }
        public DateTime TreatmentToDate { get; set; }
        public DateTime PCR4DayTestDate { get; set; }
        public DateTime PCR4DaySampleDate { get; set; }
        public string PCR4DayResult { get; set; }
        public DateTime PCR8DayTestDate { get; set; }
        public DateTime PCR8DaySampleDate { get; set; }
        public string PCR8DayResult { get; set; }
        public DateTime FirstCallScheduledDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsUpdate { get; set; }
    }
}