using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core.Dto.UseCaseRequests
{
    public class AvailabilityRequest : IUseCaseRequest<AvailabilityResponse>
    {
        public string Id { get; }
        public string Name { get; }

        public AvailabilityRequest(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}