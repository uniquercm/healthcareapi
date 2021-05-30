using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Dto.UseCaseRequests;
using Dapper;

namespace Web.Api.Infrastructure.Data.Repositories
{
    internal sealed class DashBoardRepository : IDashBoardRepository
    {
        private readonly AppDbContext _appDbContext;
        public DashBoardRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<DashBoardDetails> GetDashBoardDetails(string companyId)
        {
            DashBoardDetails retDashBoardDetails = new DashBoardDetails();
            try
            {
                var tableName = $"HC_Staff_Patient.patient_obj";

                var ColumAssign = $"*";

                var whereCond = "";

                string todayDate = DateTime.Today.ToString("yyyy-MM-dd 00:00:00.0");

                if (!string.IsNullOrEmpty(companyId))
                    whereCond += " where company_id = '" + companyId + "'";

                using (var connection = _appDbContext.Connection)
                {
                    var cond = "";
                    var sqlSelQuery = $"select * from HC_Staff_Patient.patient_obj" + whereCond;
                    var sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retDashBoardDetails.TotalPatientNumber = sqlSelResult.Count();

                    retDashBoardDetails.DischargePatientNumber = 0;

                    retDashBoardDetails.CurrentPatientNumber = 0;

                    sqlSelQuery = $"select * from HC_Authentication.user_obj" + whereCond;
                    sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retDashBoardDetails.TotalUserTypeMemberNumber = sqlSelResult.Count();

                    cond = $" where user_type = 2";
                    if (!string.IsNullOrEmpty(companyId))
                        cond += " and company_id = '" + companyId + "'";
                    sqlSelQuery = $"select * from HC_Authentication.user_obj" + cond;
                    sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retDashBoardDetails.TotalAdminUserNumber = sqlSelResult.Count();

                    cond = $" where user_type = 3";
                    if (!string.IsNullOrEmpty(companyId))
                        cond += " and company_id = '" + companyId + "'";
                    sqlSelQuery = $"select * from HC_Authentication.user_obj" + cond;
                    sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retDashBoardDetails.TotalDoctorUserNumber = sqlSelResult.Count();

                    cond = $" where user_type = 4";
                    if (!string.IsNullOrEmpty(companyId))
                        cond += " and company_id = '" + companyId + "'";
                    sqlSelQuery = $"select * from HC_Authentication.user_obj" + cond;
                    sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retDashBoardDetails.TotalManagerUserNumber = sqlSelResult.Count();

                    cond = $" where user_type = 5";
                    if (!string.IsNullOrEmpty(companyId))
                        cond += " and company_id = '" + companyId + "'";
                    sqlSelQuery = $"select * from HC_Authentication.user_obj" + cond;
                    sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retDashBoardDetails.TotalNurseUserNumber = sqlSelResult.Count();

                    cond = $" where user_type = 6";
                    if (!string.IsNullOrEmpty(companyId))
                        cond += " and company_id = '" + companyId + "'";
                    sqlSelQuery = $"select * from HC_Authentication.user_obj" + cond;
                    sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retDashBoardDetails.TotalReceptionistUserNumber = sqlSelResult.Count();

                    cond = $" where user_type = 7";
                    if (!string.IsNullOrEmpty(companyId))
                        cond += " and company_id = '" + companyId + "'";
                    sqlSelQuery = $"select * from HC_Authentication.user_obj" + cond;
                    sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retDashBoardDetails.TotalTeamUserNumber = sqlSelResult.Count();

                    cond = $" where created_on = '" + todayDate + "'";
                    if (!string.IsNullOrEmpty(companyId))
                        cond += " and company_id = '" + companyId + "'";
                    sqlSelQuery = $"select * from HC_Staff_Patient.patient_obj" + cond;
                    sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retDashBoardDetails.TodayPatientRegNumber = sqlSelResult.Count();
                }

                retDashBoardDetails.TotalTeamNumber = 10;
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retDashBoardDetails;
        }
    }
}