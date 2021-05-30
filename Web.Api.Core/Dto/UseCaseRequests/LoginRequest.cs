using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

using Web.Api.Core.Enums;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class LoginRequest : IUseCaseRequest<LoginResponse>
    {
        public string UserName { get; }
        public string Password { get; }

        public LoginRequest(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }

    public class LoginUserDetails
    {
        public string UserId { get; set; }
        public int UserType { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
    }

    public class UserRequest : IUseCaseRequest<AcknowledgementResponse>
    {
        public string UserId { get; set; }//user_id – varchar(128)
        public string UserName { get; set; }//user_name  - Varchar(128)
        public string Password { get; set; }//password – varchar(128)
        public int UserType { get; set; }//user_type - bigint(20) – (1-SuperAdmin, 2- Admin, 3- Doctor, 4- Nurse, 5- Receptionist)
        public string CompanyId { get; set; }//company_id – varchar(128)
        public string CreatedBy { get; set; } //created_by
        public string ModifiedBy { get; set; } //modified_by
        public bool IsUpdate { get; set; }
    }

    public class UserDetails
    {
        public string UserId { get; set; }//user_id – varchar(128)
        public string UserName { get; set; }//user_name  - Varchar(128)
        public string Password { get; set; }//password – varchar(128)
        public int UserType { get; set; }//user_type - bigint(20) – (1-SuperAdmin, 2- Admin, 3- Doctor, 4- Nurse, 5- Receptionist)
        public string CompanyId { get; set; }//company_id – varchar(128)
        public string CompanyName { get; set; }
        public string CreatedBy { get; set; } //created_by
        public string ModifiedBy { get; set; } //modified_by
    }

    public class DashBoardDetails
    {
        public int TotalPatientNumber { get; set; }
        public int CurrentPatientNumber { get; set; }
        public int DischargePatientNumber { get; set; }
        public int TodayPatientRegNumber { get; set; }
        public int TotalUserTypeMemberNumber { get; set; }
        public int TotalTeamNumber { get; set; }
    }
}
