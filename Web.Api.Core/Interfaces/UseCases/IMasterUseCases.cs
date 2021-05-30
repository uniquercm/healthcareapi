using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;

namespace Web.Api.Core.Interfaces.UseCases
{
    public partial interface IMasterUseCases : IUseCaseRequestHandler<GetDetailsRequest, GetDetailsResponse>
    {   }
    public partial interface IMasterUseCases : IUseCaseRequestHandler<MasterRequest, AcknowledgementResponse>
    {   }
    public partial interface IMasterUseCases : IUseCaseRequestHandler<CompanyRequest, AcknowledgementResponse>
    {   }
}