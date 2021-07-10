using System;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class FilePatientRequest : IUseCaseRequest<AcknowledgementResponse>
    {
        public List<PatientRequest> PatientRequestList { get; set; }
        public List<string> CreatedPatientIdList { get; set; }
        public List<PatientRequest> DuplicatedPatientRequestList { get; set; }
        public List<PatientRequest> ErroredPatientRequestList { get; set; }
    }
    public class PatientRequest : IUseCaseRequest<AcknowledgementResponse>
    {
        public string PatientId { get; set; }//patient_id – varchar(128)
        public string PatientName { get; set; }//patient_name – varchar(128)
        public string CompanyId { get; set; }//company_id – varchar(128)
        public int RequestId { get; set; }//request_id - int(10)
        public string RequestCrmName { get; set; }
        public string CRMNo { get; set; }//crm_no	- varchar(128)
        public string EIDNo { get; set; }//eid_no - varchar(128)
        public DateTime DateOfBirth { get; set; }//date_of_birth – datetime
        public int Age { get; set; }//age – int(10)
        public string Sex { get; set; }//sex - varchar(25)
        public string Address { get; set; }//address - varchar(500)
        public string LandMark { get; set; }//landmark - varchar(128)
        public string Area { get; set; }//area – varchar(128)
        public int CityId { get; set; }//city_id - int(10)
        public int NationalityId { get; set; }//nationality_id – int(10)
        public string NationalityName { get; set; }
        public DateTime AssignedDate { get; set; }//assigned_date - datetime
        public string MobileNo { get; set; }//mobile_no - varchar(25)
        public string GoogleMapLink { get; set; }//google_map_link – varchar(156)
        public int AdultsCount { get; set; }//no_of_adults – int(10)
        public int ChildrensCount { get; set; }//no_of_childrens – int(10)
        public int EnrolledCount { get; set; }//enrolled_count - int(25)
        public string EnrolledDetails { get; set; }//enrolled_details - int(25)
        public string StickerApplication { get; set; }//sticker_application - enum('No','Yes')
        public int TrackerApplication { get; set; }//tracker_application – int(10)
        public int PCRCount { get; set; }//pcr_count - int(10)
        public string StickerRemoval { get; set; }//sticker_removal - enum('No','Yes')
        public int TrackerRemoval { get; set; }//tracker_removal – int(10)
        public DateTime RecptionCallDate { get; set; }//reception_date - datetime
        public string RecptionCallStatus { get; set; }//reception_status - varchar(100)
        public string RecptionCallRemarks { get; set; }//reception_remarks - varchar(256)
        public string CreatedBy { get; set; } //created_by
        public string ModifiedBy { get; set; } //modified_by
        public bool IsUpdate { get; set; }
        public bool IsReception { get; set; }
        public string ErrorMsg { get; set; }
    }
    
    public class PatientDetails
    {
        public string PatientId { get; set; }//patient_id – varchar(128)
        public string PatientName { get; set; }//patient_name – varchar(128)
        public string CompanyId { get; set; }//company_id – varchar(128)
        public string CompanyName { get; set; }
        public int RequestId { get; set; }//request_id - int(10)
        public string RequestCrmName { get; set; }
        public string CRMNo { get; set; }//crm_no	- varchar(128)
        public string EIDNo { get; set; }//eid_no - varchar(128)
        public DateTime DateOfBirth { get; set; }//date_of_birth – datetime
        public int Age { get; set; }//age – int(10)
        public string Sex { get; set; }//sex - varchar(25)
        public string Address { get; set; }//address - varchar(500)
        public string LandMark { get; set; }//landmark - varchar(128)
        public string Area { get; set; }//area – varchar(128)
        public int CityId { get; set; }//city_id - int(10)
        public string CityName { get; set; }
        public int NationalityId { get; set; }//nationality_id – int(10)
        public string NationalityName { get; set; }
        public DateTime AssignedDate { get; set; }//assigned_date - datetime
        public string MobileNo { get; set; }//mobile_no - varchar(25)
        public string GoogleMapLink { get; set; }//google_map_link – varchar(156)
        public int AdultsCount { get; set; }//no_of_adults – int(10)
        public int ChildrensCount { get; set; }//no_of_childrens – int(10)
        public int EnrolledCount { get; set; }//enrolled_count - int(25)
        public string EnrolledDetails { get; set; }//enrolled_details - int(25)
        public string StickerApplication { get; set; }//sticker_application - enum('No','Yes')
        public int TrackerApplication { get; set; }//tracker_application – int(10)
        public string StickerRemoval { get; set; }//sticker_removal - enum('No','Yes')
        public int TrackerRemoval { get; set; }//tracker_removal – int(10)
        public string StickerTrackerAppliedNumber { get; set; }//sticker_tracker_applied_no - varchar(50)
        public int PCRCount { get; set; }//pcr_count - int(10)
        public string IsSendClaim { get; set; }//have_send_claim - varchar(25)Yes or No
        public DateTime RecptionCallDate { get; set; }//reception_date - datetime
        public string RecptionCallStatus { get; set; }//reception_status - varchar(100)
        public string RecptionCallRemarks { get; set; }//reception_remarks - varchar(256)
        public string DrCallId { get; set; }
        public string ScheduledId { get; set; }
        public string Status { get; set; } //status - enum('Active','InActive')
        public string CreatedBy { get; set; } //created_by
        public string ModifiedBy { get; set; } //modified_by
    }

    public class ServicePlanRequest : IUseCaseRequest<AcknowledgementResponse>
    {
        public string PatientId { get; set; }//patient_id – varchar(128)
        public string CompanyId { get; set; }//company_id – varchar(128)
        public string ScheduledId { get; set; }//scheduled_id - varchar(128)

        public string ServiceName { get; set; }
        public DateTime PCRSampleDate { get; set; }
        public string PCRResult { get; set;}

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