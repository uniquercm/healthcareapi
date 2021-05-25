using System.Threading.Tasks;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseRequests;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface IStaffRepository 
    {
        Task<List<StaffDetails>> GetStaffDetails(string companyId, string staffId);
        Task<bool> CreateStaff(StaffRequest staffRequest);
        string GenerateUUID();
        Task<bool> EditStaff(StaffRequest staffRequest);
    }
}
