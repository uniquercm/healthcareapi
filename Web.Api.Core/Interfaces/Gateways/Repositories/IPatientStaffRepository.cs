using System.Threading.Tasks;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseRequests;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface IPatientStaffRepository 
    {
        Task<List<PatientStaffDetails>> GetPatientStaffDetails(string patientStaffId, string patientId, string staffId);
        Task<bool> CreatePatientStaff(PatientStaffRequest patientStaffRequest);
        string GenerateUUID();
        Task<bool> EditPatientStaff(PatientStaffRequest patientStaffRequest);
    }
}
