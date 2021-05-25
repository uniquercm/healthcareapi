using System.Threading.Tasks;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseRequests;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface IMasterRepository 
    {
        Task<MasterDetails> GetMasterDetails();
        Task<List<CityDetails>> GetCityDetails();
        Task<List<NationalityDetails>> GetNationalityDetails();
        Task<List<SectionDetails>> GetSectionDetails();
        Task<List<RequestCRMDetails>> GetRequestCRMDetails();
        Task<List<CompanyDetails>> GetCompanyDetails(string companyId);
        Task<bool> CreateCompany(CompanyRequest companyRequest);
        string GenerateUUID();
        Task<bool> EditCompany(CompanyRequest companyRequest);
    }
}
