﻿using System.Threading.Tasks;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;

namespace Web.Api.Core.UseCases
{
    public sealed class ScheduledUseCases : IScheduledUseCases
    {
        private readonly IScheduledRepository _scheduledRepository;
        public ScheduledUseCases(IScheduledRepository scheduledRepository)
        {
            _scheduledRepository = scheduledRepository;
        }

        public async Task<bool> Handle(GetDetailsRequest request, IOutputPort<GetDetailsResponse> outputPort)
        {
            GetDetailsResponse getDetailsResponse;

            if(request.Id == "Call")
                getDetailsResponse = new GetDetailsResponse(await _scheduledRepository.GetCallDetails(request.Id, request.ScheduledId), true, "Data Fetched Successfully");
            else
                getDetailsResponse = new GetDetailsResponse(await _scheduledRepository.GetScheduledDetails(request.Id, request.ScheduledId, request.PatientStaffId), true, "Data Fetched Successfully");

            outputPort.Handle(getDetailsResponse);
            return true;
        }
        public async Task<bool> Handle(ScheduledRequest request, IOutputPort<AcknowledgementResponse> outputPort)
        {
            AcknowledgementResponse acknowledgementResponse;

            if(request.IsUpdate)//Edit a Scheduled
            {
                if(await _scheduledRepository.EditScheduled(request))
                    acknowledgementResponse = new AcknowledgementResponse(true, "Scheduled Successfully Modifyed");
                else
                    acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", "Error Occurred")}, false);
            }
            else//Create a Scheduled
            {
                if(await _scheduledRepository.CreateScheduled(request))
                    acknowledgementResponse = new AcknowledgementResponse(true, "Scheduled Created Successfully");
                else
                    acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", "Error Occurred")}, false);
            }
            outputPort.Handle(acknowledgementResponse);
            return true;
        }
        public async Task<bool> Handle(CallRequest request, IOutputPort<AcknowledgementResponse> outputPort)
        {
            AcknowledgementResponse acknowledgementResponse;

            if(request.IsUpdate)//Edit a Call
            {
                if(await _scheduledRepository.EditCall(request))
                    acknowledgementResponse = new AcknowledgementResponse(true, "Call Successfully Modifyed");
                else
                    acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", "Error Occurred")}, false);
            }
            else//Create a Call
            {
                if(await _scheduledRepository.CreateCall(request))
                    acknowledgementResponse = new AcknowledgementResponse(true, "Call Created Successfully");
                else
                    acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", "Error Occurred")}, false);
            }
            outputPort.Handle(acknowledgementResponse);
            return true;
        }

    }
}