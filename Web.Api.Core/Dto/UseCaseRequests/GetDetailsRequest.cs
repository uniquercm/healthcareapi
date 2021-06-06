using System;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class GetDetailsRequest  : IUseCaseRequest<GetDetailsResponse>
    {
        public string Id { get; set; }
        public string CompanyId { get; set; }
        public string PatientId { get; set; }
        public string StaffId { get; set; }
        public string PatientStaffId { get; set; }
        public string ScheduledId { get; set; }
        public string LableName { get; set; }
        public DateTime ScheduledFromDate { get; set; }
        public DateTime ScheduledToDate { get; set; }
        public string TeamUserName { get; set; }
        public string ExtractData { get; set; }//all, no, yes
        public string SendClaim { get; set; }//all, no, yes
        public string CallStatus { get; set; }//all, called, pending
        public string FieldAllocationStatus { get; set; }//all, allowed, notallowed

        public GetDetailsRequest()
        {   }
        public GetDetailsRequest(string id)
        {
            Id = id;
        }
        public GetDetailsRequest(string companyId, string patientId)
        {
            CompanyId = companyId;
            PatientId = patientId;
        }
        public GetDetailsRequest(string companyId, string patientId, string lableName)
        {
            CompanyId = companyId;
            PatientId = patientId;
            LableName = lableName;
        }
        public GetDetailsRequest(string companyId, DateTime scheduledFromDate, DateTime scheduledToDate, string lableName, string callStatus, string teamUserName)
        {
            CompanyId = companyId;
            ScheduledFromDate = scheduledFromDate;
            ScheduledToDate = scheduledToDate;
            LableName = lableName;
            CallStatus = callStatus;
            TeamUserName = teamUserName;
        }
        public GetDetailsRequest(string id, string patientId, string staffId, string patientStaffId, string scheduledId, string lableName)
        {
            Id = id;
            PatientId = patientId;
            StaffId = staffId;
            PatientStaffId = patientStaffId;
            ScheduledId = scheduledId;
            LableName = lableName;
        }
        public GetDetailsRequest(string companyId, string patientId, string scheduledId, DateTime scheduledFromDate, DateTime scheduledToDate, string lableName)
        {
            CompanyId = companyId;
            PatientId = patientId;
            ScheduledId = scheduledId;
            ScheduledFromDate = scheduledFromDate;
            ScheduledToDate = scheduledToDate;
            LableName = lableName;
        }
        public GetDetailsRequest(string companyId, string patientId, string scheduledId, DateTime scheduledFromDate, DateTime scheduledToDate, string lableName, string fieldAllocationStatus)
        {
            CompanyId = companyId;
            PatientId = patientId;
            ScheduledId = scheduledId;
            ScheduledFromDate = scheduledFromDate;
            ScheduledToDate = scheduledToDate;
            LableName = lableName;
            FieldAllocationStatus = fieldAllocationStatus;
        }
        public GetDetailsRequest(string companyId, string patientId, string scheduledId, DateTime scheduledFromDate, DateTime scheduledToDate, string lableName, string extractData, string sendClaim)
        {
            CompanyId = companyId;
            PatientId = patientId;
            ScheduledId = scheduledId;
            ScheduledFromDate = scheduledFromDate;
            ScheduledToDate = scheduledToDate;
            LableName = lableName;
            ExtractData = extractData;
            SendClaim = sendClaim;
        }

    }
}