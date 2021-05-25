using System;

namespace Web.Api.Models.Request
{
    public class PatientRequest
    {
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string CompanyId { get; set; }
        public int RequestId { get; set; }
        public string CRMNo { get; set; }
        public string EIDNo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public string LandMark { get; set; }
        public string Area { get; set; }
        public int CityId { get; set; }
        public int NationalityId { get; set; }
        public string MobileNo { get; set; }
        public string GoogleMapLink { get; set; }
        public string StickerApplication { get; set; }
        public string StickerRemoval { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsUpdate { get; set; }
    }
}