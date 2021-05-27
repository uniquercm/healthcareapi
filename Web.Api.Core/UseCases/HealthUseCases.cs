using System.Threading.Tasks;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.Services;
using Web.Api.Core.Interfaces.UseCases;


namespace Web.Api.Core.UseCases
{
    public sealed class HealthUseCases : IHealthUseCases
    {
        private readonly IHealthRepository _healthRepository;
       
        public HealthUseCases(IHealthRepository healthRepository)
        {
            _healthRepository = healthRepository;
        }

        public async Task<bool> Handle(PatientRequest request, IOutputPort<AcknowledgementResponse> outputPort)
        {
            AcknowledgementResponse acknowledgementResponse;
            if(!await _healthRepository.CheckDBHealth())
            {
                acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Not Healthy", "Not Healthy")}, false);
            }
            else
            {
                acknowledgementResponse = new AcknowledgementResponse(true,"Healthy");
            }
          
            outputPort.Handle(acknowledgementResponse);
            return true;
        } 

    }
}