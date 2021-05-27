using System.Threading.Tasks;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;

namespace Web.Api.Core.UseCases
{
    public sealed class MasterUseCases : IMasterUseCases
    {
        private readonly IMasterRepository _masterRepository;
        public MasterUseCases(IMasterRepository masterRepository)
        {
            _masterRepository = masterRepository;
           
        }

        public async Task<bool> Handle(GetDetailsRequest request, IOutputPort<GetDetailsResponse> outputPort)
        {
            GetDetailsResponse getDetailsResponse;

            if(request.Id == "Master")
                getDetailsResponse = new GetDetailsResponse(await _masterRepository.GetMasterDetails(), true, "Data Fetched Successfully");
            else if(request.Id == "City")
                getDetailsResponse = new GetDetailsResponse(await _masterRepository.GetCityDetails(), true, "Data Fetched Successfully");
            else if(request.Id == "RequestCRM")
                getDetailsResponse = new GetDetailsResponse(await _masterRepository.GetRequestCRMDetails(), true, "Data Fetched Successfully");
            else if(request.Id == "Nationality")
                getDetailsResponse = new GetDetailsResponse(await _masterRepository.GetNationalityDetails(), true, "Data Fetched Successfully");
            else if(request.Id == "Section")
                getDetailsResponse = new GetDetailsResponse(await _masterRepository.GetSectionDetails(), true, "Data Fetched Successfully");
            else 
                getDetailsResponse = new GetDetailsResponse(await _masterRepository.GetCompanyDetails(request.Id), true, "Data Fetched Successfully");

            outputPort.Handle(getDetailsResponse);
            return true;
        }
        public async Task<bool> Handle(CompanyRequest request, IOutputPort<AcknowledgementResponse> outputPort)
        {
            AcknowledgementResponse acknowledgementResponse;

            if(request.IsUpdate)//Edit a Company
            {
                if(await _masterRepository.EditCompany(request))
                    acknowledgementResponse = new AcknowledgementResponse(true, "Company Successfully Modifyed");
                else
                    acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", "Error Occurred")}, false);
            }
            else//Create a Company
            {
                if(await _masterRepository.CreateCompany(request))
                    acknowledgementResponse = new AcknowledgementResponse(true, "Company Created Successfully");
                else
                    acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", "Error Occurred")}, false);
            }
            outputPort.Handle(acknowledgementResponse);
            return true;
        }

    }
}