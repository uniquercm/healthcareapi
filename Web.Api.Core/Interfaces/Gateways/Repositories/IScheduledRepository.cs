using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseRequests;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface IScheduledRepository 
    {
        Task<List<ScheduledDetails>> GetScheduledDetails(string companyId, string scheduledId, string patientId, bool isFieldAllocation, IPatientRepository patientRepository, DateTime scheduledFromDate, DateTime scheduledToDate, string searchAllowTeamType, string serviceName, string serviceStatus, bool isTeam, string areaNames);
        Task<List<CallDetails>> GetCallDetails(string callId, string scheduledId);
        Task<bool> CreateScheduled(ScheduledRequest scheduledRequest);
        string GenerateUUID();
        Task<bool> CreateCall(CallRequest callRequest);
        Task<bool> EditScheduled(ScheduledRequest scheduledRequest);
        Task<bool> EditCall(CallRequest callRequest);
        Task<bool> EditFieldAllocation(FieldAllocationRequest fieldAllocationRequest);
    }
}
