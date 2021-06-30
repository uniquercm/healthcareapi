using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class DeleteRequest : IUseCaseRequest<AcknowledgementResponse>
    {
        public string Id { get; }
        public string CompanyId { get; }
        public string PatientId { get; }
        public string DeletedBy { get; }

        public DeleteRequest(string id, string deletedBy)
        {
            Id = id;
            DeletedBy = deletedBy;
        }
        public DeleteRequest(string companyId, string patientId, string deletedBy)
        {
            CompanyId = companyId;
            PatientId = patientId;
            DeletedBy = deletedBy;
        }
    }
}