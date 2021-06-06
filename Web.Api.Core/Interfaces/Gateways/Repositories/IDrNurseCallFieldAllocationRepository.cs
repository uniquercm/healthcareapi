using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseRequests;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface IDrNurseCallFieldAllocationRepository 
    {
        Task<List<DrNurseCallDetails>> GetFieldAllowCallDetails(string companyId, DateTime scheduledFromDate, DateTime scheduledToDate, string callStatus);
        Task<List<DrNurseCallDetails>> GetDrNurseCallDetails(string companyId, string callName, DateTime scheduledFromDate, DateTime scheduledToDate, string callStatus);
        Task<List<DrNurseCallDetails>> GetTeamFieldAllowCallDetails(string companyId, string callName, DateTime scheduledFromDate, DateTime scheduledToDate, string callStatus);
        Task<bool> EditPCRCall(CallRequest callRequest);

    }
}