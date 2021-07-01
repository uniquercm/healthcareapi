using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseRequests;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface IDrNurseCallFieldAllocationRepository 
    {
        Task<List<DrNurseCallDetails>> GetFieldAllowCallDetails(string companyId, string teamUserName, string callName, DateTime scheduledFromDate, DateTime scheduledToDate, string callStatus, string serviceName, string serviceStatus);
        Task<List<DrNurseCallDetails>> GetDrNurseCallDetails(string companyId, string teamUserName, string callName, DateTime scheduledFromDate, DateTime scheduledToDate, string callStatus, string serviceName);
        Task<List<DrNurseCallDetails>> GetTeamFieldAllowCallDetails(string companyId, string teamUserName, DateTime scheduledFromDate, DateTime scheduledToDate, string callStatus, string serviceName);
        Task<List<DrNurseCallDetails>> GetTrackerStickerCallDetails(string companyId, string teamUserName, bool isTracker, DateTime scheduledFromDate, DateTime scheduledToDate, string serviceStatus);
        Task<List<DrNurseCallDetails>> GetPCRCallDetails(string companyId, string teamUserName, bool is4thDay, DateTime scheduledFromDate, DateTime scheduledToDate, string serviceStatus);
        Task<bool> EditPCRCall(CallRequest callRequest);
        Task<bool> EditStickerTrackerDischargeCall(CallRequest callRequest);
        Task<bool> EditServicePlan(ServicePlanRequest servicePlanRequest);
        Task<bool> EditPatientEnrollmentDetails(ServicePlanRequest servicePlanRequest);
        Task<bool> EditScheduleStickerTrackerDetails(ServicePlanRequest servicePlanRequest);

    }
}