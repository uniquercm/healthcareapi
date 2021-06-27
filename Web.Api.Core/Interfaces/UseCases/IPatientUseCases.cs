using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;

namespace Web.Api.Core.Interfaces.UseCases
{
    public partial interface IPatientUseCases : IUseCaseRequestHandler<GetDetailsRequest, GetDetailsResponse>
    {   }
    public partial interface IPatientUseCases : IUseCaseRequestHandler<PatientRequest, AcknowledgementResponse>
    {   }
    public partial interface IPatientUseCases : IUseCaseRequestHandler<DeleteRequest, AcknowledgementResponse>
    {   }
}