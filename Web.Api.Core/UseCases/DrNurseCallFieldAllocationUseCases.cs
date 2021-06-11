using System.Threading.Tasks;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;

namespace Web.Api.Core.UseCases
{
    public sealed class DrNurseCallFieldAllocationUseCases : IDrNurseCallFieldAllocationUseCases
    {
        private readonly IDrNurseCallFieldAllocationRepository _drNurseCallFieldAllocationRepository;
        private readonly IScheduledRepository _scheduledRepository;
        private readonly IPatientRepository _patientRepository;
        public DrNurseCallFieldAllocationUseCases(IDrNurseCallFieldAllocationRepository drNurseCallFieldAllocationRepository, IScheduledRepository scheduledRepository, IPatientRepository patientRepository)
        {
            _drNurseCallFieldAllocationRepository = drNurseCallFieldAllocationRepository;
            _scheduledRepository = scheduledRepository;
            _patientRepository = patientRepository;
        }

        public async Task<bool> Handle(GetDetailsRequest request, IOutputPort<GetDetailsResponse> outputPort)
        {
            GetDetailsResponse getDetailsResponse;

            if(request.LableName == "DrCall")
                getDetailsResponse = new GetDetailsResponse(await _drNurseCallFieldAllocationRepository.GetDrNurseCallDetails(request.CompanyId, "", "DrCall", request.ScheduledFromDate, request.ScheduledToDate, request.CallStatus, request.ServiceName), true, "Data Fetched Successfully");
            else if(request.LableName == "NurseCall")
                getDetailsResponse = new GetDetailsResponse(await _drNurseCallFieldAllocationRepository.GetDrNurseCallDetails(request.CompanyId, "", "NurseCall", request.ScheduledFromDate, request.ScheduledToDate, request.CallStatus, request.ServiceName), true, "Data Fetched Successfully");
            else if(request.LableName == "TeamCall")
                getDetailsResponse = new GetDetailsResponse(await _drNurseCallFieldAllocationRepository.GetFieldAllowCallDetails(request.CompanyId, request.TeamUserName, "TeamCall", request.ScheduledFromDate, request.ScheduledToDate, request.CallStatus, request.ServiceName, request.ServiceStatus), true, "Data Fetched Successfully");
            else
                getDetailsResponse = new GetDetailsResponse(await _drNurseCallFieldAllocationRepository.GetFieldAllowCallDetails(request.CompanyId, request.TeamUserName, "field", request.ScheduledFromDate, request.ScheduledToDate, request.CallStatus, request.ServiceName, request.ServiceStatus), true, "Data Fetched Successfully");

            outputPort.Handle(getDetailsResponse);
            return true;
        }
        public async Task<bool> Handle(CallRequest request, IOutputPort<AcknowledgementResponse> outputPort)
        {
            AcknowledgementResponse acknowledgementResponse;

            bool isUpdated = false;

            if(request.IsPCRCall)
                isUpdated = await _drNurseCallFieldAllocationRepository.EditPCRCall(request);
            else
            {
                if(request.CallId.Equals("tracker") || request.CallId.Equals("sticker") || request.CallId.Equals("discharge")  )
                    isUpdated = await _drNurseCallFieldAllocationRepository.EditStickerTrackerDischargeCall(request);
                else 
                    isUpdated = await _scheduledRepository.EditCall(request);
            }

            if(isUpdated)
            {
                if(request.IsPCRCall)
                    acknowledgementResponse = new AcknowledgementResponse(true, "PCR Call Details Successfully Modifyed");
                else
                {
                    if(request.CallId.Equals("tracker"))
                        acknowledgementResponse = new AcknowledgementResponse(true, "Tracker Applied Details Successfully Modifyed");
                    else if(request.CallId.Equals("sticker"))
                        acknowledgementResponse = new AcknowledgementResponse(true, "Sticker Removed Details Successfully Modifyed");
                    else if(request.CallId.Equals("discharge"))
                        acknowledgementResponse = new AcknowledgementResponse(true, "Discharge Details Successfully Modifyed");
                    else
                        acknowledgementResponse = new AcknowledgementResponse(true, "Call Details Successfully Modifyed");
                }
            }
            else
                acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", "Error Occurred")}, false);

            outputPort.Handle(acknowledgementResponse);
            return true;
        }
        public async Task<bool> Handle(ServicePlanRequest request, IOutputPort<AcknowledgementResponse> outputPort)
        {
            AcknowledgementResponse acknowledgementResponse;

            if(await _drNurseCallFieldAllocationRepository.EditServicePlan(request))
                acknowledgementResponse = new AcknowledgementResponse(true, "Service Plan Details Successfully Modifyed");
            else
                acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", "Error Occurred")}, false);

            outputPort.Handle(acknowledgementResponse);
            return true;
        }
    }
}