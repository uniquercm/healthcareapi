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
                getDetailsResponse = new GetDetailsResponse(await _drNurseCallFieldAllocationRepository.GetDrNurseCallDetails(request.CompanyId, "DrCall", request.ScheduledFromDate, request.ScheduledToDate, request.CallStatus), true, "Data Fetched Successfully");
            else if(request.LableName == "NurseCall")
                getDetailsResponse = new GetDetailsResponse(await _drNurseCallFieldAllocationRepository.GetDrNurseCallDetails(request.CompanyId, "NurseCall", request.ScheduledFromDate, request.ScheduledToDate, request.CallStatus), true, "Data Fetched Successfully");
            else if(request.LableName == "TeamCall")
                getDetailsResponse = new GetDetailsResponse(await _drNurseCallFieldAllocationRepository.GetTeamFieldAllowCallDetails(request.CompanyId, "TeamCall", request.ScheduledFromDate, request.ScheduledToDate, request.CallStatus), true, "Data Fetched Successfully");
            else
                getDetailsResponse = new GetDetailsResponse(await _drNurseCallFieldAllocationRepository.GetFieldAllowCallDetails(request.CompanyId, request.ScheduledFromDate, request.ScheduledToDate, request.CallStatus), true, "Data Fetched Successfully");

            outputPort.Handle(getDetailsResponse);
            return true;
        }
    }
}