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

        public async Task<List<MasterDetails>> GetMasterDetails(string companyId)
        {
            List<MasterDetails> retMasterDetailsList = new List<MasterDetails>();
            try
            {
                var tableName = $"HC_Master_Details.master_obj ma, " +
                                $"HC_Master_Details.company_obj ca";

                var ColumAssign = $"ma.master_id as MasterId, ma.team_name as TeamName, " +
                                  $"ma.company_id as CompanyId, ca.company_name as CompanyName, " +
                                  $"ma.no_of_team as NumberOfTeam, ma.quarantine_no_of_day as QuarantineDay, " +
                                  $"ma.isolation_no_of_day as IsolationDay";// +
                                  //$", ma.pcr_day as PCRDay";

                var whereCond = " where ma.company_id = ca.company_id";
                if (!string.IsNullOrEmpty(companyId))
                    whereCond = " and company_id = '" + companyId + "'";

                var sqlSelQuery = $"select " + ColumAssign + " from " + tableName + whereCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelResult = await connection.QueryAsync<MasterDetails>(sqlSelQuery);
                    retMasterDetailsList = sqlSelResult.ToList();
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retMasterDetailsList;
        }

        public async Task<bool> CreateCompanyMasterDetails(MasterRequest masterRequest)
        {
            try
            {
                bool sqlResult = true;

                var tableName = $"HC_Master_Details.master_obj";
                var colName = $"company_id, no_of_team, team_name";
                var colValueName = $"@CompanyId, @NumberOfTeam, @TeamName";

                var sqlInsQuery = $"INSERT INTO "+ tableName + "( " + colName + " )" +
                                    $"VALUES ( " + colValueName + " )";

                object colValueParam = new
                {
                    CompanyId = masterRequest.CompanyId,
                    NumberOfTeam = masterRequest.NumberOfTeam,
                    TeamName = masterRequest.TeamName
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

        public async Task<bool> EditCompanyMasterDetails(MasterRequest masterRequest)
        {
            try
            {
                bool sqlResult = true;
                var tableName = $"HC_Master_Details.master_obj";
                var colName = $"company_id = @CompanyId, no_of_team = @NumberOfTeam, team_name = @TeamName";

                var whereCond = $" where master_id = @MasterId";
                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                object colValueParam = new
                {
                    MasterId = masterRequest.MasterId,
                    CompanyId = masterRequest.CompanyId,
                    NumberOfTeam = masterRequest.NumberOfTeam,
                    TeamName = masterRequest.TeamName
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

                var ColumAssign = $"company_id as CompanyId, company_name as CompanyName, " +
                                  $"no_of_team as NumberOfTeam, team_name as TeamName, " +
                                  $"address as Address, status as Status, " +
                                  $"created_by as CreatedBy, modified_by as ModifiedBy";

                var whereCond = " where status = 'Active'";

                if (!string.IsNullOrEmpty(companyId))
                    whereCond += " and company_id = '" + companyId + "'";

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

                var tableName = $"HC_Master_Details.company_obj";

                var colName = $"company_id, company_name, " +
                              $"no_of_team, team_name, address, " +
                              $"created_by, created_on";

                var colValueName = $"@CompanyId, @CompanyName, " +
                                   $"@NumberOfTeam, @TeamName, @Address, " +
                                   $"@CreatedBy, @CreatedOn";

                var sqlInsQuery = $"INSERT INTO "+ tableName + "( " + colName + " )" +
                                    $"VALUES ( " + colValueName + " )";

                object colValueParam = new
                {
                    CompanyId = companyRequest.CompanyId,
                    CompanyName = companyRequest.CompanyName,
                    NumberOfTeam = companyRequest.NumberOfTeam,
                    TeamName = companyRequest.TeamName,
                    Address = companyRequest.Address,
                    CreatedBy = companyRequest.CreatedBy,
                    CreatedOn = DateTime.Today.ToString("yyyy-MM-dd 00:00:00.0")
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

                var colName = $"company_id = @CompanyId, company_name = @CompanyName, " +
                              $"no_of_team = @NumberOfTeam, team_name = @TeamName, " +
                              $"address = @Address, " +
                              $"modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                var whereCond = $" where company_id = @CompanyId";

                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                object colValueParam = new
                {
                    CompanyId = companyRequest.CompanyId,
                    CompanyName = companyRequest.CompanyName,
                    NumberOfTeam = companyRequest.NumberOfTeam,
                    TeamName = companyRequest.TeamName,
                    Address = companyRequest.Address,
                    ModifiedBy = companyRequest.ModifiedBy,
                    ModifiedOn = DateTime.Today.ToString("yyyy-MM-dd 00:00:00.0")
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

        /*public async Task<bool> DeleteCompany(DeleteRequest request)
        {
            try
            {
                var tableName = $"HC_Master_Details.company_obj co, " +
                                $"HC_Staff_Patient.patient_obj pa, " +
                                $"HC_Treatment.scheduled_obj sc, " +
                                $"HC_Treatment.call_obj ca";

                var colName = $"co.status = @Status, co.modified_by = @ModifiedBy, co.modified_on = @ModifiedOn, " +
                              $"pa.status = @Status, pa.modified_by = @ModifiedBy, pa.modified_on = @ModifiedOn, " +
                              $"sc.status = @Status, sc.modified_by = @ModifiedBy, sc.modified_on = @ModifiedOn, " +
                              $"ca.status = @Status, ca.modified_by = @ModifiedBy, ca.modified_on = @ModifiedOn";

                var whereCond = $" where co.company_id = @CompanyId" +
                                $" and co.company_id = pa.company_id" +
                                $" and pa.patient_id = sc.patient_id" +
                                $" and sc.scheduled_id = ca.scheduled_id";

                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                object colValueParam = new
                {
                    CompanyId = request.Id,
                    Status = Status.InActive,
                    ModifiedBy = request.DeletedBy,
                    ModifiedOn = DateTime.UtcNow
                };
                using (var connection = _appDbContext.Connection)
                {
                    var sqlResult = Convert.ToBoolean(await connection.ExecuteAsync(sqlUpdateQuery, colValueParam));
                    //if(sqlResult)
                        //sqlResult = Convert.ToBoolean(await DeleteSchedule(request));
                    return sqlResult;
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
                return false;
            }
        }*/
        public async Task<bool> DeleteCompany(DeleteRequest request)
        {
            try
            {
                var tableName = $"HC_Master_Details.company_obj";
                var colName = $"status = @Status, modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                var whereCond = $" where company_id = @CompanyId";
                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                object colValueParam = new
                {
                    CompanyId = request.CompanyId,
                    Status = Status.InActive,
                    ModifiedBy = request.DeletedBy,
                    ModifiedOn = DateTime.UtcNow
                };
                using (var connection = _appDbContext.Connection)
                {
                    var sqlResult = Convert.ToBoolean(await connection.ExecuteAsync(sqlUpdateQuery, colValueParam));
                    if(sqlResult)
                        sqlResult = Convert.ToBoolean(await DeletePatient(request));
                    return sqlResult;
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
                return false;
            }
        }
        async Task<bool> DeletePatient(DeleteRequest request)
        {
            try
            {
                var tableName = $"HC_Staff_Patient.patient_obj";
                var colName = $"status = @Status, modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                var whereCond = $" where patient_id = @PatientId";
                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                object colValueParam = new
                {
                    PatientId = request.Id,
                    Status = Status.InActive,
                    ModifiedBy = request.DeletedBy,
                    ModifiedOn = DateTime.UtcNow
                };
                using (var connection = _appDbContext.Connection)
                {
                    var sqlResult = Convert.ToBoolean(await connection.ExecuteAsync(sqlUpdateQuery, colValueParam));
                    //if(sqlResult)
                        //sqlResult = Convert.ToBoolean(await DeleteSchedule(request));
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