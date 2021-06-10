using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseRequests;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface IDrNurseCallFieldAllocationRepository 
    {
        Task<List<DrNurseCallDetails>> GetFieldAllowCallDetails(string companyId, string teamUserName, string callName, DateTime scheduledFromDate, DateTime scheduledToDate, string callStatus, string serviceStatus);
        Task<List<DrNurseCallDetails>> GetDrNurseCallDetails(string companyId, string teamUserName, string callName, DateTime scheduledFromDate, DateTime scheduledToDate, string callStatus, string serviceStatus);
        Task<List<DrNurseCallDetails>> GetTeamFieldAllowCallDetails1(string companyId, string teamUserName, DateTime scheduledFromDate, DateTime scheduledToDate, string callStatus, string serviceStatus);
        Task<bool> EditPCRCall(CallRequest callRequest);
        Task<bool> EditServicePlan(ServicePlanRequest servicePlanRequest);

    }
}