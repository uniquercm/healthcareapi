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

        public async Task<DashBoardDetails> GetDashBoardDetails(string companyId, IDrNurseCallFieldAllocationRepository drNurseCallFieldAllocationRepository)
        {
            DashBoardDetails retDashBoardDetails = new DashBoardDetails();
            try
            {
                var whereCond = "";

                string todayDate = DateTime.Today.ToString("yyyy-MM-dd 00:00:00.0");

                if (!string.IsNullOrEmpty(companyId))
                    whereCond = " where company_id = '" + companyId + "'";

                using (var connection = _appDbContext.Connection)
                {
                    var cond = $" where created_on = '" + todayDate + "'";
                    if (!string.IsNullOrEmpty(companyId))
                        cond += " and company_id = '" + companyId + "'";
                    var sqlSelQuery = $"select * from HC_Staff_Patient.patient_obj" + cond;
                    var sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retDashBoardDetails.TodayPatientRegNumber = sqlSelResult.Count();

                    cond = $" where pa.patient_id = sc.patient_id" +  
                           $" and ca.call_scheduled_date = '" + todayDate + "'" +
                           $" and ca.scheduled_id = sc.scheduled_id";
                    if (!string.IsNullOrEmpty(companyId))
                        cond += " and pa.company_id = '" + companyId + "'";
                    sqlSelQuery = $"select * from  HC_Staff_Patient.patient_obj pa, " +
                                  $"HC_Treatment.scheduled_obj sc, " +
                                  $"HC_Treatment.call_obj ca" + cond;
                    sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retDashBoardDetails.TodayScheduledPatientNumber = sqlSelResult.Count();

                    cond = $" where pa.patient_id = sc.patient_id" +  
                           $" and sc.4day_pcr_test_date = '" + todayDate + "'" +
                           $" or sc.8day_pcr_test_date = '" + todayDate + "'";
                    if (!string.IsNullOrEmpty(companyId))
                        cond += " and pa.company_id = '" + companyId + "'";
                    sqlSelQuery = $"select * from  HC_Staff_Patient.patient_obj pa, " +
                                  $"HC_Treatment.scheduled_obj sc" + cond;
                    sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retDashBoardDetails.TodayScheduledPatientNumber += sqlSelResult.Count();

                    sqlSelQuery = $"select * from HC_Staff_Patient.patient_obj" + whereCond;
                    sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retDashBoardDetails.TotalPatientNumber = sqlSelResult.Count();

                    cond = $" where discharge_date < '" + todayDate + "'" + 
                           $" and pa.patient_id = sc.patient_id";
                    if (!string.IsNullOrEmpty(companyId))
                        cond += " and pa.company_id = '" + companyId + "'";
                    sqlSelQuery = $"select * from HC_Treatment.scheduled_obj sc, " + 
                                  $"HC_Staff_Patient.patient_obj pa " + cond;
                    sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retDashBoardDetails.DischargePatientNumber = sqlSelResult.Count();

                    retDashBoardDetails.ActivePatientNumber = retDashBoardDetails.TotalPatientNumber - retDashBoardDetails.DischargePatientNumber;

                    cond = $" where discharge_date >= '" + todayDate + "'" + 
                           $" and pa.patient_id = sc.patient_id";
                    if (!string.IsNullOrEmpty(companyId))
                        cond += " and pa.company_id = '" + companyId + "'";
                    sqlSelQuery = $"select * from HC_Treatment.scheduled_obj sc, " + 
                                  $"HC_Staff_Patient.patient_obj pa " + cond;
                    sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retDashBoardDetails.CurrentPatientNumber = sqlSelResult.Count();

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

                    sqlSelQuery = $"select no_of_team from HC_Master_Details.company_obj" + whereCond;
                    var sqlResult = await connection.QueryAsync<int>(sqlSelQuery);
                    retDashBoardDetails.TotalTeamNumber = sqlResult.FirstOrDefault();

                    retDashBoardDetails.TeamStatusDetailsList = await GetTeamDetails(companyId, drNurseCallFieldAllocationRepository);
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retDashBoardDetails;
        }

        async Task<List<TeamStatusDetails>> GetTeamDetails(string companyId, IDrNurseCallFieldAllocationRepository drNurseCallFieldAllocationRepository)
        {
            List<TeamStatusDetails> retTeamStatusDetails = new List<TeamStatusDetails>();
            try
            {
                var tableName = $"HC_Authentication.user_obj u, HC_Master_Details.company_obj co";

                var ColumAssign = $"u.company_id as CompanyId, co.company_name as CompanyName, " +
                                  $"u.full_name as TeamName, u.user_name as TeamUserName";

                var whereCond = $" where user_type = 7";

                string todayDate = DateTime.Today.ToString("yyyy-MM-dd 00:00:00.0");

                if (!string.IsNullOrEmpty(companyId))
                    whereCond += " and u.company_id = '" + companyId + "'";

                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelQuery = $"select " + ColumAssign + " from " + tableName + whereCond;
                    var sqlSelResult = await connection.QueryAsync<TeamStatusDetails>(sqlSelQuery);

                    foreach(TeamStatusDetails singleTeamStatusDetails in sqlSelResult.ToList())
                    {//called , pending, visited
                        List<DrNurseCallDetails> tmpDrNurseCallDetails = await drNurseCallFieldAllocationRepository.GetFieldAllowCallDetails(companyId, singleTeamStatusDetails.TeamUserName, "TeamCall", DateTime.Today, DateTime.Today, "all", "all");
                        singleTeamStatusDetails.AllocatedCount = tmpDrNurseCallDetails.Count();

                        tmpDrNurseCallDetails = await drNurseCallFieldAllocationRepository.GetFieldAllowCallDetails(companyId, singleTeamStatusDetails.TeamUserName, "TeamCall", DateTime.Today, DateTime.Today, "pending", "all");
                        singleTeamStatusDetails.CallStatusPendingCount = tmpDrNurseCallDetails.Count();

                        tmpDrNurseCallDetails = await drNurseCallFieldAllocationRepository.GetFieldAllowCallDetails(companyId, singleTeamStatusDetails.TeamUserName, "TeamCall", DateTime.Today, DateTime.Today, "called", "all");
                        singleTeamStatusDetails.CallStatusCalledCount = tmpDrNurseCallDetails.Count();

                        tmpDrNurseCallDetails = await drNurseCallFieldAllocationRepository.GetFieldAllowCallDetails(companyId, singleTeamStatusDetails.TeamUserName, "TeamCall", DateTime.Today, DateTime.Today, "visited", "all");
                        singleTeamStatusDetails.CallStatusVisitedCount = tmpDrNurseCallDetails.Count();

                        retTeamStatusDetails.Add(singleTeamStatusDetails);
                    }
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retTeamStatusDetails;
        }
    }
}