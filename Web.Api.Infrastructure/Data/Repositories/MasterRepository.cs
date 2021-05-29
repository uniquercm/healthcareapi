using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Dto.UseCaseRequests;
using System.Security.Cryptography;
using System.Text;
using Web.Api.Core.Enums;
using Dapper;

namespace Web.Api.Infrastructure.Data.Repositories
{
    internal sealed class MasterRepository : IMasterRepository
    {
        private readonly AppDbContext _appDbContext;
        public MasterRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<MasterDetails> GetMasterDetails()
        {
            MasterDetails retMasterDetails = new MasterDetails();
            try
            {
                var tableName = $"HC_Master_Details.master_obj";

                var ColumAssign = $"master_id as MasterId, team_name as TeamName, " +
                                   $"no_of_team as NumberOfTeam, quarantine_no_of_day as QuarantineDay, " +
                                   $"isolation_no_of_day as IsolationDay, pcr_day as PCRDay";

                var sqlSelQuery = $"select " + ColumAssign + " from " + tableName;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelResult = await connection.QueryAsync<MasterDetails>(sqlSelQuery);
                    retMasterDetails = sqlSelResult.FirstOrDefault();
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retMasterDetails;
        }

        public async Task<List<CityDetails>> GetCityDetails()
        {
            List<CityDetails> retCityDetailsList = new List<CityDetails>();
            try
            {
                var tableName = $"HC_Master_Details.city_obj";

                var ColumAssign = $"city_id as CityId, city_name as CityName";

                var sqlSelQuery = $"select " + ColumAssign + " from " + tableName;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelResult = await connection.QueryAsync<CityDetails>(sqlSelQuery);
                    retCityDetailsList = sqlSelResult.ToList();
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retCityDetailsList;
        }

        public async Task<List<NationalityDetails>> GetNationalityDetails()
        {
            List<NationalityDetails> retNationalityDetailsList = new List<NationalityDetails>();
            try
            {
                var tableName = $"HC_Master_Details.nationality_obj";

                var ColumAssign = $"nationality_id as NationalityId, nationality_name as NationalityName, country_name as CountryName";

                var sqlSelQuery = $"select " + ColumAssign + " from " + tableName;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelResult = await connection.QueryAsync<NationalityDetails>(sqlSelQuery);
                    retNationalityDetailsList = sqlSelResult.ToList();
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retNationalityDetailsList;
        }

        public async Task<List<SectionDetails>> GetSectionDetails()
        {
            List<SectionDetails> retSectionDetailsList = new List<SectionDetails>();
            try
            {
                var tableName = $"HC_Master_Details.section_obj";

                var ColumAssign = $"section_id as SectionId, section_name as SectionName";

                var sqlSelQuery = $"select " + ColumAssign + " from " + tableName;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelResult = await connection.QueryAsync<SectionDetails>(sqlSelQuery);
                    retSectionDetailsList = sqlSelResult.ToList();
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retSectionDetailsList;
        }

        public async Task<List<RequestCRMDetails>> GetRequestCRMDetails()
        {
            List<RequestCRMDetails> retRequestCRMDetailsList = new List<RequestCRMDetails>();
            try
            {
                var tableName = $"HC_Master_Details.request_crm_obj";

                var ColumAssign = $"request_crm_id as RequestCRMId, request_crm_name as RequestCRMName";

                var sqlSelQuery = $"select " + ColumAssign + " from " + tableName;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelResult = await connection.QueryAsync<RequestCRMDetails>(sqlSelQuery);
                    retRequestCRMDetailsList = sqlSelResult.ToList();
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retRequestCRMDetailsList;
        }

        public async Task<List<CompanyDetails>> GetCompanyDetails(string companyId)
        {
            List<CompanyDetails> retCompanyDetailsList = new List<CompanyDetails>();
            try
            {
                var tableName = $"HC_Master_Details.company_obj";

                var ColumAssign = $"company_id as CompanyId, company_name as CompanyName, address as Address, " +
                                  $"created_by as CreatedBy, modified_by as ModifiedBy";

                var whereCond = "";

                if (!string.IsNullOrEmpty(companyId))
                    whereCond += " where company_id = '" + companyId + "'";

                var sqlSelQuery = $"select " + ColumAssign + " from " + tableName + whereCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelResult = await connection.QueryAsync<CompanyDetails>(sqlSelQuery);
                    retCompanyDetailsList = sqlSelResult.ToList();
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retCompanyDetailsList;
        }

        public async Task<bool> CreateCompany(CompanyRequest companyRequest)
        {
            CompanyDetails retCompanyDetails = new CompanyDetails();
            try
            {
                var uuid = GenerateUUID();
                bool sqlResult = true;
                companyRequest.CompanyId = uuid;

                var colName = $"company_id, company_name, address, created_by, created_on";
                var colValueName = $"@CompanyId, @CompanyName, @Address, @CreatedBy, @CreatedOn";
                var tableName = $"HC_Master_Details.company_obj";

                var sqlInsQuery = $"INSERT INTO "+ tableName + "( " + colName + " )" +
                                    $"VALUES ( " + colValueName + " )";

                object colValueParam = new
                {
                    CompanyId = companyRequest.CompanyId,
                    CompanyName = companyRequest.CompanyName,
                    Address = companyRequest.Address,
                    CreatedBy = companyRequest.CreatedBy,
                    CreatedOn = DateTime.UtcNow
                };
                using (var connection = _appDbContext.Connection)
                {
                    sqlResult = Convert.ToBoolean(await connection.ExecuteAsync(sqlInsQuery, colValueParam));
                }
                return sqlResult;
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
                return false;
            }
        }
        public string GenerateUUID()
        {
            using (var connection = _appDbContext.Connection)
            {
                var sqlQuery = $"Select UUID()";
                var sql = connection.Query<string>(sqlQuery);
                return sql.FirstOrDefault();
            }
        }
        public async Task<bool> EditCompany(CompanyRequest companyRequest)
        {
            try
            {
                bool sqlResult = true;
                var tableName = $"HC_Master_Details.company_obj";
                var colName = $"company_id = @CompanyId, company_name = @CompanyName, address = @Address, " +
                              $"modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                var whereCond = $" where company_id = @UserId";
                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                object colValueParam = new
                {
                    CompanyId = companyRequest.CompanyId,
                    CompanyName = companyRequest.CompanyName,
                    Address = companyRequest.Address,
                    ModifiedBy = companyRequest.ModifiedBy,
                    ModifiedOn = DateTime.UtcNow
                };
                using (var connection = _appDbContext.Connection)
                {
                    sqlResult = Convert.ToBoolean(await connection.ExecuteAsync(sqlUpdateQuery, colValueParam));
                    return sqlResult;
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
                return false;
            }
        }

    }
}