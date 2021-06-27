using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using System.Collections.Generic;

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
    public class AreaRequest : IUseCaseRequest<AcknowledgementResponse>
    {
        public int AreaId { get; set; }
        public int RegionId { get; set;}
        public string RegionName { get; set; }
        public string AreaName { get; set; }
    }

    public class LoginUserDetails
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int UserType { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
    }

    public class UserRequest : IUseCaseRequest<AcknowledgementResponse>
    {
        public string UserId { get; set; }//user_id – varchar(128)
        public string FullName { get; set; }//full_name – varchar(128)
        public string UserName { get; set; }//user_name  - Varchar(128)
        public string Password { get; set; }//password – varchar(128)
        public int UserType { get; set; }//user_type - bigint(20) – (1-SuperAdmin, 2- Admin, 3- Doctor, 4- Nurse, 5- Receptionist)
        public string CompanyId { get; set; }//company_id – varchar(128)
        public string AreaList { get; set; }//area_list - varchar(500)
        public string CreatedBy { get; set; } //created_by
        public string ModifiedBy { get; set; } //modified_by
        public bool IsUpdate { get; set; }
    }

    public class UserDetails
    {
        public string UserId { get; set; }//user_id – varchar(128)
        public string FullName { get; set; }//full_name – varchar(128)
        public string UserName { get; set; }//user_name  - Varchar(128)
        public string Password { get; set; }//password – varchar(128)
        public int UserType { get; set; }//user_type - bigint(20) – (1-SuperAdmin, 2- Admin, 3- Doctor, 4- Nurse, 5- Receptionist)
        public string CompanyId { get; set; }//company_id – varchar(128)
        public string CompanyName { get; set; }
        public string Status { get; set; }//ststus - enum('Active','InActive')
        public string AreaList { get; set; }//area_list - varchar(500)
        public string CreatedBy { get; set; } //created_by
        public string ModifiedBy { get; set; } //modified_by
    }

    public class DashBoardDetails
    {
        public TodayPatientStatusDetails TodayPatientStatusDetails { get; set; }
        public AllUserTypeDetails AllUserTypeDetails { get; set; }
        public PatientStatusDetails PatientStatusDetails { get; set; }
        public ReceptionStatusDetails ReceptionStatusDetails { get; set; }
        public DoctorStatusDetails DoctorStatusDetails { get; set; }
        public NurseStatusDetails NurseStatusDetails { get; set; }
        public List<TeamStatusDetails> TeamStatusDetailsList { get; set; }
        public DashBoardDetails()
        {
            TodayPatientStatusDetails = new TodayPatientStatusDetails();
            AllUserTypeDetails = new AllUserTypeDetails();
            PatientStatusDetails = new PatientStatusDetails();
            ReceptionStatusDetails = new ReceptionStatusDetails();
            DoctorStatusDetails = new DoctorStatusDetails();
            NurseStatusDetails = new NurseStatusDetails();
            TeamStatusDetailsList = new List<TeamStatusDetails>();
        }
    }
    public class TodayPatientStatusDetails
    {
        public int TodayPatientRegNumber { get; set; }
        public int TodayScheduledPatientNumber { get; set; }
    }
    public class AllUserTypeDetails
    {
        public int TotalUserTypeMemberNumber { get; set; }
        public int TotalTeamNumber { get; set; }
        public int TotalAdminUserNumber { get; set; }
        public int TotalDoctorUserNumber { get; set; }
        public int TotalManagerUserNumber { get; set; }
        public int TotalNurseUserNumber { get; set; }
        public int TotalReceptionistUserNumber { get; set; }
        public int TotalTeamUserNumber { get; set; }
    }
    public class PatientStatusDetails
    {
        public int TotalPatientNumber { get; set; }
        public int TotalEnrolledPatientNumber { get; set; }
        public int CurrentPatientNumber { get; set; }
        public int DischargePatientNumber { get; set; }
        public int ActivePatientNumber { get; set; }
    }
    public class ReceptionStatusDetails
    {
        public int ReceptionTotalCount { get; set; }
        public int ReceptionCompletedCount { get; set; }
        public int ReceptionPendingCount { get; set; }
    }
    public class DoctorStatusDetails
    {
        public int DoctorCallTotalCount { get; set; }
        public int DoctorCalledCount { get; set; }
        public int DoctorPendingCount { get; set; }
    }
    public class NurseStatusDetails
    {
        public int NurseCallTotalCount { get; set; }
        public int NurseCalledCount { get; set; }
        public int NursePendingCount { get; set; }
    }
    public class TeamStatusDetails
    {
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string TeamName { get; set; }
        public string TeamUserName{ get; set; }
        public int AllocatedCount { get; set; }
        //public int CallStatusCalledCount { get; set; }
        public int CallStatusVisitedCount { get; set; }
        public int CallStatusPendingCount { get; set; }
    }
}
