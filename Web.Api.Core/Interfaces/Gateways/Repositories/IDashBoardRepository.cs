using System.Threading.Tasks;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseRequests;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface IDashBoardRepository 
    {
        Task<DashBoardDetails> GetDashBoardDetails(string companyId, IDrNurseCallFieldAllocationRepository drNurseCallFieldAllocationRepository);
    }
}