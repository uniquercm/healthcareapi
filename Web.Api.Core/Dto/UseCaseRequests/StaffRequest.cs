using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class StaffRequest : IUseCaseRequest<AcknowledgementResponse>
    {
        public string StaffId { get; set; }//staff_id – varchar(128)
        public string StaffName { get; set; }//staff_name – varchar(128)
        public int StaffType { get; set; }//staff_type - enum('Admin','Doctor','Nurse','Receptionist')
        public string UserId { get; set; }//user_id – varchar(128)
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }//address - varchar(500)
        public string MobileNo { get; set; }//mobile_no - varchar(25)
        public string CompanyId { get; set; }//company_id – varchar(128)
        public string TeamName { get; set; }//team_name – varchar(128)
        public string CreatedBy { get; set; } //created_by
        public string ModifiedBy { get; set; } //modified_by
        public bool IsUpdate { get; set; }
    }
    public class StaffDetails
    {
        public string StaffId { get; set; }//staff_id – varchar(128)
        public string StaffName { get; set; }//staff_name – varchar(128)
        public int StaffType { get; set; }//staff_type - enum('Admin','Doctor','Nurse','Receptionist')
        public string UserId { get; set; }//user_id – varchar(128)
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }//address - varchar(500)
        public string MobileNo { get; set; }//mobile_no - varchar(25)
        public string CompanyId { get; set; }//company_id – varchar(128)
        public string CompanyName { get; set; }
        public string TeamName { get; set; }//team_name – varchar(128)
        public string CreatedBy { get; set; } //created_by
        public string ModifiedBy { get; set; } //modified_by
    }
}