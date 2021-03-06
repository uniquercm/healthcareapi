using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseRequests;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface IPatientRepository 
    {
        Task<List<PatientDetails>> GetPatientDetails(string companyId, string patientId, string gMapLinkSatus, DateTime assignedFromDate, DateTime assignedToDate, string SearchStatus, string areaNames);
        Task<bool> CreatePatient(PatientRequest patientRequest);
        Task<bool> CreateFilePatient(FilePatientRequest filePatientRequest);
        string GenerateUUID();
        Task<bool> EditPatient(PatientRequest patientRequest);
        Task<bool> DeletePatient(DeleteRequest request);
        Task<bool> CheckCRMNumberAvailability(string crmNumber, string companyId, string patientId);
    }
}
