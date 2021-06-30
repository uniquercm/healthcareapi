using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class MasterRequest : IUseCaseRequest<AcknowledgementResponse>
    {
        public int MasterId { get; set; }//master_id – int(10)
        public string CompanyId { get; set; }//company_id – varchar(128)
        public string TeamName { get; set; }//team_name – varchar(128)
        public int NumberOfTeam { get; set; }//no_of_team – int(10)
        //public int QuarantineDay { get; set; }//quarantine_no_of_day – int(10)
        //public int IsolationDay { get; set; }//isolation_no_of_day – int(10)
        //public int PCRDay { get; set; }//pcr_day – int(10)
        public bool IsUpdate { get; set; }
    }
    public class MasterDetails
    {
        public int MasterId { get; set; }//master_id – int(10)
        public string CompanyId { get; set; }//company_id – varchar(128)
        public string CompanyName { get; set; }
        public string TeamName { get; set; }//team_name – varchar(128)
        public int NumberOfTeam { get; set; }//no_of_team – int(10)
        public int QuarantineDay { get; set; }//quarantine_no_of_day – int(10)
        public int IsolationDay { get; set; }//isolation_no_of_day – int(10)
        public int PCRDay { get; set; }//pcr_day – int(10)
    }

    public class CityDetails
    {
        public int CityId { get; set; }//city_id – int(10)
        public string CityName { get; set; }//city_name – varchar(128)
    }
    public class NationalityDetails
    {
        public int NationalityId { get; set; }//nationality_id – int(10)
        public string NationalityName { get; set; }//nationality_name – varchar(128)
        public string CountryName { get; set; }//country_name – varchar(128)
    }
    public class SectionDetails
    {
        public int SectionId { get; set; }//section_id – int(10)
        public string SectionName { get; set; }//section_name – varchar(128)
    }
    public class RequestCRMDetails
    {
        public int RequestCRMId { get; set; }//request_crm_id - int(10)
        public string RequestCRMName { get; set; }//request_crm_name - varchar(128)
    }
    public class CompanyRequest : IUseCaseRequest<AcknowledgementResponse>
    {
        public string CompanyId { get; set; }//company_id – varchar(128)
        public string CompanyName { get; set; }//company_name – varchar(128
        public string Address { get; set; }//address - varchar(500)
        public int NumberOfTeam { get; set; }//no_of_team – int(10)
        public string TeamName { get; set; }//team_name – varchar(128)
        public string CreatedBy { get; set; } //created_by - varchar(128)
        public string ModifiedBy { get; set; } //modified_by - varchar(128)
        public bool IsUpdate { get; set; }
    }
    public class CompanyDetails
    {
        public string CompanyId { get; set; }//company_id – varchar(128)
        public string CompanyName { get; set; }//company_name – varchar(128)
        public int NumberOfTeam { get; set; }//no_of_team – int(10)
        public string TeamName { get; set; }//team_name – varchar(128)
        public string Address { get; set; }//address - varchar(500)
        public string Status { get; set; } //status - enum('Active','InActive')
        public string CreatedBy { get; set; } //created_by - varchar(128)
        public string ModifiedBy { get; set; } //modified_by - varchar(128)
    }
}