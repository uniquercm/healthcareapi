using System;
using System.Collections.Generic;
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
        public string ServiceName { get; set; }//all, tracker, sticker, 4pcr, 8pcr, discharge
        public string ServiceStatus { get; set; }//all, applied, removed, replaced, visited, notvisited, discharged, pending, others
        public int UserType { get; set; }
        public string DateSearchType { get; set; }//schedule, allocated, reallocaed
        public string SearchStatus { get; set; }//all, completed, pending, scheduled, notscheduled
        public bool IsTeamFieldAllocation { get; set; }
        public string AreaNames { get; set; }

        public GetDetailsRequest()
        {   }
        public GetDetailsRequest(string id)
        {
            Id = id;
        }
        public GetDetailsRequest(string id, string companyId, int userType)
        {
            Id = id;
            CompanyId = companyId;
            UserType = userType;
        }
        public GetDetailsRequest(string companyId, string patientId)//, string gmapSatus)
        {
            CompanyId = companyId;
            PatientId = patientId;
            //GMapLinkStatus = gmapSatus;
        }
        public GetDetailsRequest(string companyId, string patientId, string lableName)
        {
            CompanyId = companyId;
            PatientId = patientId;
            LableName = lableName;
        }
        public GetDetailsRequest(string companyId, string patientId, DateTime scheduledFromDate, DateTime scheduledToDate, string lableName, string searchStatus, string areaNames)
        {
            CompanyId = companyId;
            PatientId = patientId;
            ScheduledFromDate = scheduledFromDate;
            ScheduledToDate = scheduledToDate;
            LableName = lableName;
            SearchStatus = searchStatus;
            AreaNames = areaNames; 
        }
        public GetDetailsRequest(string companyId, DateTime scheduledFromDate, DateTime scheduledToDate, string lableName, string callStatus, string teamUserName, string serviceName, string dateSearchType)
        {
            CompanyId = companyId;
            ScheduledFromDate = scheduledFromDate;
            ScheduledToDate = scheduledToDate;
            LableName = lableName;
            CallStatus = callStatus;
            TeamUserName = teamUserName;
            ServiceName = serviceName;
            DateSearchType = dateSearchType;
        }
        public GetDetailsRequest(string companyId, DateTime scheduledFromDate, DateTime scheduledToDate, string lableName, string callStatus, string teamUserName, string serviceName, string serviceStatus, string dateSearchType)
        {
            CompanyId = companyId;
            ScheduledFromDate = scheduledFromDate;
            ScheduledToDate = scheduledToDate;
            LableName = lableName;
            CallStatus = callStatus;
            TeamUserName = teamUserName;
            ServiceName = serviceName;
            ServiceStatus = serviceStatus;
            DateSearchType = dateSearchType;
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
        public GetDetailsRequest(string companyId, string patientId, string scheduledId, DateTime scheduledFromDate, DateTime scheduledToDate, string lableName, string fieldAllocationStatus, string serviceName, string serviceStatus, bool isTeamFieldAllocation, string areaNames)
        {
            CompanyId = companyId;
            PatientId = patientId;
            ScheduledId = scheduledId;
            ScheduledFromDate = scheduledFromDate;
            ScheduledToDate = scheduledToDate;
            LableName = lableName;
            FieldAllocationStatus = fieldAllocationStatus;
            ServiceName = serviceName;
            ServiceStatus = serviceStatus;
            IsTeamFieldAllocation = isTeamFieldAllocation;
            AreaNames = areaNames;
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