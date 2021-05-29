using System;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class PatientRequest : IUseCaseRequest<AcknowledgementResponse>
    {
        public string PatientId { get; set; }//patient_id – varchar(128)
        public string PatientName { get; set; }//patient_name – varchar(128)
        public string CompanyId { get; set; }//company_id – varchar(128)
        public int RequestId { get; set; }//request_id - int(10)
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
        public string MobileNo { get; set; }//mobile_no - varchar(25)
        public string GoogleMapLink { get; set; }//google_map_link – varchar(156)
        public string StickerApplication { get; set; }//sticker_application - enum('No','Yes')
        public string StickerRemoval { get; set; }//sticker_removal - enum('No','Yes')
        public string CreatedBy { get; set; } //created_by
        public string ModifiedBy { get; set; } //modified_by
        public bool IsUpdate { get; set; }
    }
    
    public class PatientDetails
    {
        public string PatientId { get; set; }//patient_id – varchar(128)
        public string PatientName { get; set; }//patient_name – varchar(128)
        public string CompanyId { get; set; }//company_id – varchar(128)
        public string CompanyName { get; set; }
        public int RequestId { get; set; }//request_id - int(10)
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
        public string MobileNo { get; set; }//mobile_no - varchar(25)
        public string GoogleMapLink { get; set; }//google_map_link – varchar(156)
        public string StickerApplication { get; set; }//sticker_application - enum('No','Yes')
        public string StickerRemoval { get; set; }//sticker_removal - enum('No','Yes')
        public string DrCallId { get; set; }
        public string ScheduledId { get; set; }
        public string CreatedBy { get; set; } //created_by
        public string ModifiedBy { get; set; } //modified_by
    }

}