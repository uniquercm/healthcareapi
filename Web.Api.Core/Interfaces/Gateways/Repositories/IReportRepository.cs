using System.Threading.Tasks;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseRequests;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface IReportRepository 
    {
        Task<List<ReportDetails>> GetReportDetails(string companyId, string patientId, string scheduledId);
        Task<CallDetails> GetCallDetails(string callId, string scheduledId);
        Task<bool> EditReportDetails(ReportDetails reportDetails);
    }
}