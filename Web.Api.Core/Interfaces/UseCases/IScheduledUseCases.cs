using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;

namespace Web.Api.Core.Interfaces.UseCases
{
    public partial interface IScheduledUseCases : IUseCaseRequestHandler<GetDetailsRequest, GetDetailsResponse>
    {   }
    public partial interface IScheduledUseCases : IUseCaseRequestHandler<ScheduledRequest, AcknowledgementResponse>
    {   }
    public partial interface IScheduledUseCases : IUseCaseRequestHandler<CallRequest, AcknowledgementResponse>
    {   }
}