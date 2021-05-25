using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;

namespace Web.Api.Core.Interfaces.UseCases
{
    public partial interface IPatientStaffUseCases : IUseCaseRequestHandler<GetDetailsRequest, GetDetailsResponse>
    {   }
    public partial interface IPatientStaffUseCases : IUseCaseRequestHandler<PatientStaffRequest, AcknowledgementResponse>
    {   }
}