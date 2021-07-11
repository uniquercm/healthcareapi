using System;
using System.Collections.Generic;
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
        public DateTime AllocatedDate { get; set; }//team_allocated_date - datetime
        public string ReAllocatedTeamName { get; set; }//reallocated_team_name - varchar(100)
        public DateTime ReAllocatedDate { get; set; }//team_reallocated_date - datetime


        public DateTime DischargeDate { get; set; }//discharge_date – datetime
        public string DischargeStatus { get; set; }//discharge_status - varchar(100)
        public string DischargeRemarks { get; set; }//discharge_remarks - varchar(128)

        public string TreatmentType { get; set; }//treatment_type - varchar(100) enum('Quarantine','Isolation')
        public DateTime TreatmentFromDate { get; set; }//treatment_from_date - datetime
        public DateTime TreatmentToDate { get; set; }//treatment_to_date – datetime

        public DateTime PCR4DayTestDate { get; set; }//4day_pcr_test_date – datetime
        public DateTime PCR4DaySampleDate { get; set; }//4day_pcr_test_sample_date – datetime
        public string PCR4DayResult { get; set; }//4day_pcr_test_result - varchar(100) enum('Negative','Positive')
        public DateTime PCR6DayTestDate { get; set; }//6day_pcr_test_date - datetime
        public DateTime PCR6DaySampleDate { get; set; }//6day_pcr_test_sample_date – datetime
        public string PCR6DayResult { get; set; }//6day_pcr_test_result - varchar(100) enum('Negative','Positive')
        public DateTime PCR8DayTestDate { get; set; }//8day_pcr_test_date – datetime
        public DateTime PCR8DaySampleDate { get; set; }//8day_pcr_test_sample_date – datetime
        public string PCR8DayResult { get; set; }//8day_pcr_test_result - varchar(100) enum('Negative','Positive')
        public DateTime PCR11DayTestDate { get; set; }//11day_pcr_test_date - datetime
        public DateTime PCR11DaySampleDate { get; set; }//11day_pcr_test_sample_date – datetime
        public string PCR11DayResult { get; set; }//11day_pcr_test_result - varchar(100) enum('Negative','Positive')

        public DateTime TrackerScheduleDate { get; set; }//tracker_schedule_date - datetime
        public DateTime TrackerAppliedDate { get; set; }//tracker_applied_date - datetime
        public DateTime StickerScheduleDate { get; set; }//sticker_schedule_date - datetime
        public DateTime StickerRemovedDate { get; set; }//sticker_removed_date - datetime
        public string StickerTrackerNumber { get; set; }//sticker_tracker_no - varchar(50)
        public DateTime TrackerReplacedDate { get; set; }//tracker_replace_date - datatime
        public string TrackerReplaceNumber { get; set; }//tracker_replace_no - varchar(50)
        public string StickerTrackerResult { get; set; }//sticker_tracker_result - varchar(50)

        public DateTime FirstCallScheduledDate { get; set; }
        public string Day2CallId { get; set; }//2day_call_id – varchar(128)
        public string Day3CallId { get; set; }//3day_call_id – varchar(128)
        public string Day4CallId { get; set; }//4day_call_id – varchar(128)
        public string Day5CallId { get; set; }//5day_call_id – varchar(128)
        public string Day6CallId { get; set; }//6day_call_id – varchar(128)
        public string Day7CallId { get; set; }//7day_call_id – varchar(128)
        public string Day9CallId { get; set; }//9day_call_id – varchar(128)
        public string CreatedBy { get; set; } //created_by
        public string ModifiedBy { get; set; } //modified_by
        public bool IsUpdate { get; set; }
        public int RequestId { get; set; }//request_id - int(10)
        public string RequestCrmName { get; set; }//1-HQP, 2- HIP, 3- CRM
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
        public int Age { get; set; }
        public int RequestId { get; set; }//request_id - int(10)
        public string RequestCrmName { get; set; }//1-HQP, 2- HIP, 3- CRM
        public PatientDetails PatientInformation { get; set; }
        public DateTime PCRTestDate { get; set; }//initial_pcr_test_date – datetime
        public string PCRResult { get; set; }//initial_pcr_test_result - enum('Negative','Positive')
        public string HaveVaccine { get; set; }//have_vaccine - varchar(64)
        public string AllocatedTeamName { get; set; }//allocated_team_name - varchar(100)
        public DateTime AllocatedDate { get; set; }//team_allocated_date - datetime
        public string ReAllocatedTeamName { get; set; }//reallocated_team_name - varchar(100)
        public DateTime ReAllocatedDate { get; set; }//team_reallocated_date - datetime

        public DateTime DischargeDate { get; set; }//discharge_date – datetime
        public string DischargeStatus { get; set; }//discharge_status - varchar(100)
        public string DischargeRemarks { get; set; }//discharge_remarks - varchar(128)
        public string DischargeTeamUserName { get; set; }//discharge_team_user_name - varchar(128)
        public string DischargeTeamStatus { get; set; }//discharge_team_status - varchar(100) - enum('visited','notvisited','pending')
        public string DischargeTeamRemark { get; set; }//discharge_team_remark - varchar(256)
        public DateTime DischargeTeamStatusDate { get; set; }//discharge_team_date - datetime

        public string TreatmentType { get; set; }//treatment_type - enum('Quarantine','Isolation')

        public DateTime TreatmentFromDate { get; set; }//treatment _from_date - datetime
        public DateTime TreatmentToDate { get; set; }//treatment _to_date – datetime

        public DateTime PCR4DayTestDate { get; set; }//4day_pcr_test_date – datetime
        public DateTime PCR4DaySampleDate { get; set; }//4day_pcr_test_sample_date – datetime
        public string PCR4DayResult { get; set; }//4day_pcr_test_result - enum('Negative','Positive')
        public string PCR4DayTeamUserName { get; set; }//4day_pcr_team_user_name - varchar(128)
        public string PCR4DayTeamStatus { get; set; }//4day_pcr_team_status - varchar(100) - enum('visited','notvisited','pending')
        public string PCR4DayTeamRemark { get; set; }//4day_pcr_team_remark - varchar(256)
        public DateTime PCR4DayTeamStatusDate { get; set; }//4day_pcr_team_date - datetime

        public DateTime PCR6DayTestDate { get; set; }//6day_pcr_test_date – datetime
        public DateTime PCR6DaySampleDate { get; set; }//6day_pcr_test_sample_date – datetime
        public string PCR6DayResult { get; set; }//6day_pcr_test_result - enum('Negative','Positive')
        public string PCR6DayTeamUserName { get; set; }//6day_pcr_team_user_name - varchar(128)
        public string PCR6DayTeamStatus { get; set; }//6day_pcr_team_status - varchar(100) - enum('visited','notvisited','pending')
        public string PCR6DayTeamRemark { get; set; }//6day_pcr_team_remark - varchar(256)
        public DateTime PCR6DayTeamStatusDate { get; set; }//6day_pcr_team_date - datetime

        public DateTime PCR8DayTestDate { get; set; }//8day_pcr_test_date - datetime
        public DateTime PCR8DaySampleDate { get; set; }//8day_pcr_test_sample_date – datetime
        public string PCR8DayResult { get; set; }//8day_pcr_test_result - enum('Negative','Positive')
        public string PCR8DayTeamUserName { get; set; }//8day_pcr_team_user_name - varchar(128)
        public string PCR8DayTeamStatus { get; set; }//8day_pcr_team_status - varchar(100) - enum('visited','notvisited','pending')
        public string PCR8DayTeamRemark { get; set; }//8day_pcr_team_remark - varchar(256)
        public DateTime PCR8DayTeamStatusDate { get; set; }//8day_pcr_team_date - datetime

        public DateTime PCR11DayTestDate { get; set; }//11day_pcr_test_date – datetime
        public DateTime PCR11DaySampleDate { get; set; }//11day_pcr_test_sample_date – datetime
        public string PCR11DayResult { get; set; }//11day_pcr_test_result - enum('Negative','Positive')
        public string PCR11DayTeamUserName { get; set; }//11day_pcr_team_user_name - varchar(128)
        public string PCR11DayTeamStatus { get; set; }//11day_pcr_team_status - varchar(100) - enum('visited','notvisited','pending')
        public string PCR11DayTeamRemark { get; set; }//11day_pcr_team_remark - varchar(256)
        public DateTime PCR11DayTeamStatusDate { get; set; }//11day_pcr_team_date - datetime


        public DateTime TrackerScheduleDate { get; set; }//tracker_schedule_date - datetime
        public DateTime TrackerAppliedDate { get; set; }//tracker_applied_date - datetime
        public string TrackerTeamUserName { get; set; }//tracker_team_user_name - varchar(128)
        public string TrackerTeamStatus { get; set; }//tracker_team_status - varchar(100) - enum('visited','notvisited','pending')
        public string TrackerTeamRemark { get; set; }//tracker_team_remark - varchar(256)
        public DateTime TrackerTeamStatusDate { get; set; }//tracker_team_date - datetime
        public DateTime StickerScheduleDate { get; set; }//sticker_schedule_date - datetime
        public DateTime StickerRemovedDate { get; set; }//sticker_removed_date - datetime
        public string StickerTeamUserName { get; set; }//sticker_team_user_name - varchar(128)
        public string StickerTeamStatus { get; set; }//sticker_team_status - varchar(100) - enum('visited','notvisited','pending')
        public string StickerTeamRemark { get; set; }//sticker_team_remark - varchar(256)
        public DateTime StickerTeamStatusDate { get; set; }//sticker_team_date - datetime
        public string StickerTrackerNumber { get; set; }//sticker_tracker_no - varchar(50)
        public DateTime TrackerReplacedDate { get; set; }//tracker_replace_date - datatime
        public string TrackerReplaceNumber { get; set; }//tracker_replace_no - varchar(50)
        public string TrackerReplaceTeamUserName { get; set; }//tracker_replace_team_user_name - varchar(128)
        public string TrackerReplaceTeamStatus { get; set; }//tracker_replace_team_status - varchar(100) - enum('visited','notvisited','pending')
        public string TrackerReplaceTeamRemark { get; set; }//tracker_replace_team_remark - varchar(256)
        public DateTime TrackerReplaceTeamStatusDate { get; set; }//tracker_replace_team_date - datetime
        public string StickerTrackerResult { get; set; }//sticker_tracker_result - varchar(50)
       
        public string Day2CallId { get; set; }//2day_call_id – varchar(128)
        public CallDetails Day2CallDetails { get; set; }
        public string Day3CallId { get; set; }//3day_call_id – varchar(128)
        public CallDetails Day3CallDetails { get; set; }
        public string Day4CallId { get; set; }//4day_call_id – varchar(128)
        public CallDetails Day4CallDetails { get; set; }
        public string Day5CallId { get; set; }//5day_call_id – varchar(128)
        public CallDetails Day5CallDetails { get; set; }
        public string Day6CallId { get; set; }//6day_call_id – varchar(128)
        public CallDetails Day6CallDetails { get; set; }
        public string Day7CallId { get; set; }//7day_call_id – varchar(128)
        public CallDetails Day7CallDetails { get; set; }
        public string Day9CallId { get; set; }//9day_call_id – varchar(128)
        public CallDetails Day9CallDetails { get; set; }
        public string IsExtractTreatementDate { get; set; }//have_treatement_extract - varchar(25)
        public string Status { get; set; } //status - enum('Active','InActive')
        public string CreatedBy { get; set; } //created_by
        public string ModifiedBy { get; set; } //modified_by
    }
}