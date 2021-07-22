using System;
using System.Collections.Generic;

namespace Web.Api.Models.Request
{
    public class FilePatientRequest
    {
        public List<PatientRequest> PatientRequestList { get; set; }
    }
    public class PatientRequest
    {
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string CompanyId { get; set; }
        public int RequestId { get; set; }
        public string RequestCrmName { get; set; }
        public string CRMNo { get; set; }
        public string EIDNo { get; set; }
        public string DateOfBirth { get; set; }//Thanam
        //public DateTime DateOfBirth { get; set; }//Thanam
        public DateTime DOB { get; set; }//Thanam
        public int Age { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public string LandMark { get; set; }
        public string Area { get; set; }
        public int CityId { get; set; }
        public int NationalityId { get; set; }
        public string NationalityName { get; set; }
        public string AssignedDate { get; set; }//Thanam
        //public DateTime AssignedDate { get; set; }//Thanam
        public DateTime AssignDate { get; set; }//Thanam
        public string MobileNo { get; set; }
        public string GoogleMapLink { get; set; }
        public int AdultsCount { get; set; }
        public int ChildrensCount { get; set; }
        public int EnrolledCount { get; set; }
        public string EnrolledDetails { get; set; }
        public string StickerApplication { get; set; }
        public int TrackerApplication { get; set; }
        public string StickerRemoval { get; set; }
        public int TrackerRemoval { get; set; }
        public string StickerTrackerAppliedNumber { get; set; }
        public DateTime RecptionCallDate { get; set; }
        public string RecptionCallStatus { get; set; }
        public string RecptionCallRemarks { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsReception { get; set; }
    }
    
    public class ServicePlanRequest
    {
        public string PatientId { get; set; }//patient_id – varchar(128)
        public string CompanyId { get; set; }//company_id – varchar(128)
        public string ScheduledId { get; set; }//scheduled_id - varchar(128)

        public string ServiceName { get; set; }
        public DateTime PCRSampleDate { get; set; }
        public string PCRResult { get; set;}

        public DateTime DischargeDate { get; set; }
        public string DischargeRemarks { get; set; }
        public string DischargeStatus { get; set; }

        public DateTime TrackerScheduleDate { get; set; }//tracker_schedule_date - datetime
        public DateTime TrackerAppliedDate { get; set; }//tracker_applied_date - datetime
        public DateTime StickerScheduleDate { get; set; }//sticker_schedule_date - datetime
        public DateTime StickerRemovedDate { get; set; }//sticker_removed_date - datetime
        public string StickerTrackerNumber { get; set; }//sticker_tracker_no - varchar(50)
        public DateTime TrackerReplacedDate { get; set; }//tracker_replace_date - datatime
        public string TrackerReplaceNumber { get; set; }//tracker_replace_no - varchar(50)
        public string StickerTrackerResult { get; set; }//sticker_tracker_result - varchar(50)
        public int EnrolledCount { get; set; }//enrolled_count - int(25)
        public string EnrolledDetails { get; set; }//enrolled_details - int(25)
        public string StickerApplication { get; set; }//sticker_application - enum('No','Yes')
        public int TrackerApplication { get; set; }//tracker_application – int(10)
        public string StickerRemoval { get; set; }//sticker_removal - enum('No','Yes')
        public int TrackerRemoval { get; set; }//tracker_removal – int(10)
        public string ModifiedBy { get; set; } //modified_by
    }
}