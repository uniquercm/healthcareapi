using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class AvailabilityRequest : IUseCaseRequest<AvailabilityResponse>
    {
        public string Id { get; }
        public string Name { get; }

        public string PatientId { get; }
        public string CompanyId { get; }
        public string CRMNumber { get; }

        public AvailabilityRequest(string id, string name)
        {
            Id = id;
            Name = name;
        }
        public AvailabilityRequest(string crmNumber, string patientId, string companyId)
        {
            CRMNumber = crmNumber;
            PatientId = patientId;
            CompanyId = companyId;
        }
    }
}