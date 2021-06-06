using System.Threading.Tasks;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;

namespace Web.Api.Core.UseCases
{
    public sealed class DashBoardUseCases : IDashBoardUseCases
    {
        private readonly IDashBoardRepository _dashBoardRepository;
        private readonly IDrNurseCallFieldAllocationRepository _drNurseCallFieldAllocationRepository;

        public DashBoardUseCases(IDashBoardRepository dashBoardRepository, IDrNurseCallFieldAllocationRepository drNurseCallFieldAllocationRepository)
        {
            _dashBoardRepository = dashBoardRepository;
            _drNurseCallFieldAllocationRepository = drNurseCallFieldAllocationRepository;
           
        }
        public async Task<bool> Handle(GetDetailsRequest request, IOutputPort<GetDetailsResponse> outputPort)
        {
            GetDetailsResponse getDetailsResponse;
            getDetailsResponse = new GetDetailsResponse(await _dashBoardRepository.GetDashBoardDetails(request.Id, _drNurseCallFieldAllocationRepository), true, "Data Fetched Successfully");
            outputPort.Handle(getDetailsResponse);
            return true;
        }
    }
}