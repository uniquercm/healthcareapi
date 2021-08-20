using System.Threading.Tasks;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;

namespace Web.Api.Core.UseCases
{
    public sealed class ReportUseCases : IReportUseCases
    {
        private readonly IReportRepository _reportRepository;
        public ReportUseCases(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<bool> Handle(GetDetailsRequest request, IOutputPort<GetDetailsResponse> outputPort)
        {
            GetDetailsResponse getDetailsResponse;
            getDetailsResponse = new GetDetailsResponse(await _reportRepository.GetReportDetails(request.CompanyId, request.PatientId, request.ScheduledId, request.ExtractData, request.SendClaim, request.ScheduledFromDate, request.ScheduledToDate, request.AreaNames), true, "Data Fetched Successfully");
            outputPort.Handle(getDetailsResponse);
            return true;
        }

        public async Task<bool> Handle(ReportDetails request, IOutputPort<AcknowledgementResponse> outputPort)
        {
            AcknowledgementResponse acknowledgementResponse;

            if(await _reportRepository.EditReportDetails(request))
                acknowledgementResponse = new AcknowledgementResponse(true, "Report Details Successfully Modifyed");
            else
                acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", "Error Occurred")}, false);

            outputPort.Handle(acknowledgementResponse);
            return true;
        }
    }
}