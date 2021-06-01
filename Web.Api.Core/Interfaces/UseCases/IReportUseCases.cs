using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;

namespace Web.Api.Core.Interfaces.UseCases
{
    public partial interface IReportUseCases : IUseCaseRequestHandler<GetDetailsRequest, GetDetailsResponse>
    {   }
    public partial interface IReportUseCases : IUseCaseRequestHandler<ReportDetails, AcknowledgementResponse>
    {   }
}