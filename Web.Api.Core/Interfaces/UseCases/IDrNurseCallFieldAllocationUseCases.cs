using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;

namespace Web.Api.Core.Interfaces.UseCases
{
    public partial interface IDrNurseCallFieldAllocationUseCases : IUseCaseRequestHandler<GetDetailsRequest, GetDetailsResponse>
    {   }
    public partial interface IDrNurseCallFieldAllocationUseCases : IUseCaseRequestHandler<CallRequest, AcknowledgementResponse>
    {   }
    public partial interface IDrNurseCallFieldAllocationUseCases : IUseCaseRequestHandler<ServicePlanRequest, AcknowledgementResponse>
    {   }
    public partial interface IDrNurseCallFieldAllocationUseCases : IUseCaseRequestHandler<TeamVisitDetails, AcknowledgementResponse>
    {   }
}