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
                    retDashBoardDetails.TodayPatientStatusDetails = await GetTodayPatientDetails(companyId);

                    retDashBoardDetails.AllUserTypeDetails = await GetUserTypeDetails(companyId);

                    retDashBoardDetails.PatientStatusDetails = await GetPatientDetails(companyId);

                    retDashBoardDetails.ReceptionStatusDetails = await GetReceptionDetails(companyId);

                    retDashBoardDetails.DoctorStatusDetails = await GetDoctorDetails(companyId, drNurseCallFieldAllocationRepository);

                    retDashBoardDetails.NurseStatusDetails = await GetNurseDetails(companyId, drNurseCallFieldAllocationRepository);

                    retDashBoardDetails.TeamStatusDetailsList = await GetTeamDetails(companyId, drNurseCallFieldAllocationRepository);
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retDashBoardDetails;
        }

        async Task<TodayPatientStatusDetails> GetTodayPatientDetails(string companyId)
        {
            TodayPatientStatusDetails retTodayPatientStatusDetails = new TodayPatientStatusDetails();
            try
            {
                string todayDate = DateTime.Today.ToString("yyyy-MM-dd 00:00:00.0");

                using (var connection = _appDbContext.Connection)
                {
                    var cond = $" where created_on = '" + todayDate + "'";
                    if (!string.IsNullOrEmpty(companyId))
                        cond += " and company_id = '" + companyId + "'";
                    var sqlSelQuery = $"select * from HC_Staff_Patient.patient_obj" + cond;
                    var sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retTodayPatientStatusDetails.TodayPatientRegNumber = sqlSelResult.Count();

                    cond = $" where pa.patient_id = sc.patient_id" +
                           $" and pa.status = 'Active'" +
                           $" and pa.reception_status != 'closed'" +
                           $" and ca.call_scheduled_date = '" + todayDate + "'" +
                           $" and ca.scheduled_id = sc.scheduled_id";
                    if (!string.IsNullOrEmpty(companyId))
                        cond += " and pa.company_id = '" + companyId + "'";
                    sqlSelQuery = $"select * from  HC_Staff_Patient.patient_obj pa, " +
                                  $"HC_Treatment.scheduled_obj sc, " +
                                  $"HC_Treatment.call_obj ca" + cond;
                    sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retTodayPatientStatusDetails.TodayScheduledPatientNumber = sqlSelResult.Count();

                    cond = $" where pa.patient_id = sc.patient_id" + 
                           $" and pa.status = 'Active'" + 
                           $" and pa.reception_status != 'closed'" +
                           $" and sc.4day_pcr_test_date = '" + todayDate + "'" +
                           $" or sc.8day_pcr_test_date = '" + todayDate + "'";
                    if (!string.IsNullOrEmpty(companyId))
                        cond += " and pa.company_id = '" + companyId + "'";
                    sqlSelQuery = $"select * from  HC_Staff_Patient.patient_obj pa, " +
                                  $"HC_Treatment.scheduled_obj sc" + cond;
                    sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retTodayPatientStatusDetails.TodayScheduledPatientNumber += sqlSelResult.Count();
                }
            }
            catch(Exception Err)
            {
                string error = Err.Message.ToString();
            }
            return retTodayPatientStatusDetails;
        }//

        async Task<AllUserTypeDetails> GetUserTypeDetails(string companyId)
        {
            AllUserTypeDetails retAllUserTypeDetails = new AllUserTypeDetails();
            try
            {
                var tableName = "HC_Authentication.user_obj";
                var whereCond = "";

                string todayDate = DateTime.Today.ToString("yyyy-MM-dd 00:00:00.0");

                if (!string.IsNullOrEmpty(companyId))
                    whereCond = " where company_id = '" + companyId + "'";

                using (var connection = _appDbContext.Connection)
                {
                    var cond = $" where status = 'Active' ";
                    if (!string.IsNullOrEmpty(companyId))
                        cond += " and company_id = '" + companyId + "'";
                    var sqlSelQuery = $"select * from " + tableName + cond;
                    var sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retAllUserTypeDetails.TotalUserTypeMemberNumber = sqlSelResult.Count();

                    cond = $" where user_type = 2 and status = 'Active'";
                    if (!string.IsNullOrEmpty(companyId))
                        cond += " and company_id = '" + companyId + "'";
                    sqlSelQuery = $"select * from " + tableName + cond;
                    sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retAllUserTypeDetails.TotalAdminUserNumber = sqlSelResult.Count();

                    cond = $" where user_type = 3 and status = 'Active'";
                    if (!string.IsNullOrEmpty(companyId))
                        cond += " and company_id = '" + companyId + "'";
                    sqlSelQuery = $"select * from " + tableName + cond;
                    sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retAllUserTypeDetails.TotalDoctorUserNumber = sqlSelResult.Count();

                    cond = $" where user_type = 4 and status = 'Active'";
                    if (!string.IsNullOrEmpty(companyId))
                        cond += " and company_id = '" + companyId + "'";
                    sqlSelQuery = $"select * from " + tableName + cond;
                    sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retAllUserTypeDetails.TotalManagerUserNumber = sqlSelResult.Count();

                    cond = $" where user_type = 5 and status = 'Active'";
                    if (!string.IsNullOrEmpty(companyId))
                        cond += " and company_id = '" + companyId + "'";
                    sqlSelQuery = $"select * from " + tableName + cond;
                    sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retAllUserTypeDetails.TotalNurseUserNumber = sqlSelResult.Count();

                    cond = $" where user_type = 6 and status = 'Active'";
                    if (!string.IsNullOrEmpty(companyId))
                        cond += " and company_id = '" + companyId + "'";
                    sqlSelQuery = $"select * from " + tableName + cond;
                    sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retAllUserTypeDetails.TotalReceptionistUserNumber = sqlSelResult.Count();

                    cond = $" where user_type = 7 and status = 'Active'";
                    if (!string.IsNullOrEmpty(companyId))
                        cond += " and company_id = '" + companyId + "'";
                    sqlSelQuery = $"select * from " + tableName + cond;
                    sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retAllUserTypeDetails.TotalTeamUserNumber = sqlSelResult.Count();

                    sqlSelQuery = $"select no_of_team from HC_Master_Details.company_obj" + whereCond;
                    var sqlResult = await connection.QueryAsync<int>(sqlSelQuery);
                    retAllUserTypeDetails.TotalTeamNumber = sqlResult.FirstOrDefault();
                }
            }
            catch(Exception Err)
            {
                string error = Err.Message.ToString();
            }
            return retAllUserTypeDetails;
        }

        async Task<PatientStatusDetails> GetPatientDetails(string companyId)
        {
            PatientStatusDetails retPatientStatusDetails = new PatientStatusDetails();
            try
            {
                var whereCond = " where status = 'Active'";

                string todayDate = DateTime.Today.ToString("yyyy-MM-dd 00:00:00.0");

                if (!string.IsNullOrEmpty(companyId))
                    whereCond += " and company_id = '" + companyId + "'";

                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelQuery = $"select * from HC_Staff_Patient.patient_obj" + whereCond;
                    var sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retPatientStatusDetails.TotalPatientNumber = sqlSelResult.Count();

                    sqlSelQuery = $"select sum(enrolled_count) from HC_Staff_Patient.patient_obj" + whereCond;
                    var sqlResult = await connection.QueryAsync<int>(sqlSelQuery);
                    retPatientStatusDetails.TotalEnrolledPatientNumber = sqlResult.FirstOrDefault();

                    var cond = $" where discharge_date < '" + todayDate + "'" + 
                            $" and pa.reception_status != 'closed'" +
                            $" and pa.status = 'Active'" +
                            $" and pa.patient_id = sc.patient_id";
                    if (!string.IsNullOrEmpty(companyId))
                        cond += " and pa.company_id = '" + companyId + "'";
                    sqlSelQuery = $"select * from HC_Treatment.scheduled_obj sc, " + 
                                    $"HC_Staff_Patient.patient_obj pa " + cond;
                    sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retPatientStatusDetails.DischargePatientNumber = sqlSelResult.Count();

                    retPatientStatusDetails.ActivePatientNumber = retPatientStatusDetails.TotalPatientNumber - retPatientStatusDetails.DischargePatientNumber;

                    cond = $" where discharge_date >= '" + todayDate + "'" + 
                            $" and pa.reception_status != 'closed'" +
                            $" and pa.status = 'Active'" +
                            $" and pa.patient_id = sc.patient_id";
                    if (!string.IsNullOrEmpty(companyId))
                        cond += " and pa.company_id = '" + companyId + "'";
                    sqlSelQuery = $"select * from HC_Treatment.scheduled_obj sc, " + 
                                    $"HC_Staff_Patient.patient_obj pa " + cond;
                    sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retPatientStatusDetails.CurrentPatientNumber = sqlSelResult.Count();
                }
            }
            catch(Exception Err)
            {
                string error = Err.Message.ToString();
            }
            return retPatientStatusDetails;
        }//

        async Task<ReceptionStatusDetails> GetReceptionDetails(string companyId)
        {
            ReceptionStatusDetails retReceptionStatusDetails = new ReceptionStatusDetails();
            try
            {
                var tableName = $"HC_Staff_Patient.patient_obj";
                var whereCond = " where status = 'Active'";

                string todayDate = DateTime.Today.ToString("yyyy-MM-dd 00:00:00.0");

                if (!string.IsNullOrEmpty(companyId))
                    whereCond = " and company_id = '" + companyId + "'";

                using (var connection = _appDbContext.Connection)//
                {
                    var cond = $" where modified_on = '" + todayDate + "'" +
                               $" and status = 'Active'";
                    if (!string.IsNullOrEmpty(companyId))
                        cond += " and company_id = '" + companyId + "'";

                    var sqlSelQuery = $"select * from " + tableName + cond;
                    var sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retReceptionStatusDetails.ReceptionTotalCount = sqlSelResult.Count();

                    cond = $" where modified_on = '" + todayDate + "'" +
                           $" and reception_status = 'completed'" +
                           $" and status = 'Active'";
                    if (!string.IsNullOrEmpty(companyId))
                        cond += " and company_id = '" + companyId + "'";
                    sqlSelQuery = $"select * from " + tableName + cond;
                    sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retReceptionStatusDetails.ReceptionCompletedCount = sqlSelResult.Count();

                    cond = $" where modified_on = '" + todayDate + "'" +
                           $" and reception_status = 'pending'" +
                           $" and status = 'Active'";
                    if (!string.IsNullOrEmpty(companyId))
                        cond += " and company_id = '" + companyId + "'";
                    sqlSelQuery = $"select * from " + tableName + cond;
                    sqlSelResult = await connection.QueryAsync(sqlSelQuery);
                    retReceptionStatusDetails.ReceptionCompletedCount = sqlSelResult.Count();
                }
            }
            catch(Exception Err)
            {
                string error = Err.Message.ToString();
            }
            return retReceptionStatusDetails;
        }

        async Task<DoctorStatusDetails> GetDoctorDetails(string companyId, IDrNurseCallFieldAllocationRepository drNurseCallFieldAllocationRepository)
        {
            DoctorStatusDetails retDoctorStatusDetails = new DoctorStatusDetails();
            try
            {
                List<DrNurseCallDetails> tmpDrNurseCallDetails = await drNurseCallFieldAllocationRepository.GetDrNurseCallDetails(companyId, "", "DrCall", DateTime.Today, DateTime.Today, "all", "all", "all");
                retDoctorStatusDetails.DoctorCallTotalCount = tmpDrNurseCallDetails.Count();

                tmpDrNurseCallDetails = await drNurseCallFieldAllocationRepository.GetDrNurseCallDetails(companyId, "", "DrCall", DateTime.Today, DateTime.Today, "called", "all", "all");
                retDoctorStatusDetails.DoctorCalledCount = tmpDrNurseCallDetails.Count();

                tmpDrNurseCallDetails = await drNurseCallFieldAllocationRepository.GetDrNurseCallDetails(companyId, "", "DrCall", DateTime.Today, DateTime.Today, "pending", "all", "all");
                retDoctorStatusDetails.DoctorPendingCount = tmpDrNurseCallDetails.Count();
            }
            catch(Exception Err)
            {
                string error = Err.Message.ToString();
            }
            return retDoctorStatusDetails;
        }

        async Task<NurseStatusDetails> GetNurseDetails(string companyId, IDrNurseCallFieldAllocationRepository drNurseCallFieldAllocationRepository)
        {
            NurseStatusDetails retNurseStatusDetails = new NurseStatusDetails();
            try
            {
                List<DrNurseCallDetails> tmpDrNurseCallDetails = await drNurseCallFieldAllocationRepository.GetDrNurseCallDetails(companyId, "", "NurseCall", DateTime.Today, DateTime.Today, "all", "all", "all");
                retNurseStatusDetails.NurseCallTotalCount = tmpDrNurseCallDetails.Count();

                tmpDrNurseCallDetails = await drNurseCallFieldAllocationRepository.GetDrNurseCallDetails(companyId, "", "NurseCall", DateTime.Today, DateTime.Today, "called", "all", "all");
                retNurseStatusDetails.NurseCalledCount = tmpDrNurseCallDetails.Count();

                tmpDrNurseCallDetails = await drNurseCallFieldAllocationRepository.GetDrNurseCallDetails(companyId, "", "NurseCall", DateTime.Today, DateTime.Today, "pending", "all", "all");
                retNurseStatusDetails.NursePendingCount = tmpDrNurseCallDetails.Count();
            }
            catch(Exception Err)
            {
                string error = Err.Message.ToString();
            }
            return retNurseStatusDetails;
        }

/*        async Task<List<TeamStatusDetails>> GetTeamDetails(string companyId, IDrNurseCallFieldAllocationRepository drNurseCallFieldAllocationRepository)
        {
            List<TeamStatusDetails> retTeamStatusDetails = new List<TeamStatusDetails>();
            try
            {
                var tableName = $"HC_Authentication.user_obj u, HC_Master_Details.company_obj co";

                var ColumAssign = $"u.company_id as CompanyId, co.company_name as CompanyName, " +
                                  $"u.full_name as TeamName, u.user_name as TeamUserName";

                var whereCond = $" where u.user_type = 7 and u.company_id = co.company_id" +
                                $" and u.status = 'Active'" +
                                $" and co.status = 'Active'";

                string todayDate = DateTime.Today.ToString("yyyy-MM-dd 00:00:00.0");

                if (!string.IsNullOrEmpty(companyId))
                    whereCond += " and u.company_id = '" + companyId + "'";

                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelQuery = $"select " + ColumAssign + " from " + tableName + whereCond;
                    var sqlSelResult = await connection.QueryAsync<TeamStatusDetails>(sqlSelQuery);

                    foreach(TeamStatusDetails singleTeamStatusDetails in sqlSelResult.ToList())
                    {//called , pending, visited, notvisited
                        List<DrNurseCallDetails> tmpDrNurseCallDetails = await drNurseCallFieldAllocationRepository.GetFieldAllowCallDetails(companyId, singleTeamStatusDetails.TeamUserName, "TeamCall", DateTime.Today, DateTime.Today, "all", "all", "all", "allocated", "all");
                        singleTeamStatusDetails.AllocatedCount = tmpDrNurseCallDetails.Count();

                        tmpDrNurseCallDetails = await drNurseCallFieldAllocationRepository.GetFieldAllowCallDetails(companyId, singleTeamStatusDetails.TeamUserName, "TeamCall", DateTime.Today, DateTime.Today, "pending", "all", "all", "schedule", "all");
                        singleTeamStatusDetails.CallStatusPendingCount = tmpDrNurseCallDetails.Count();

                        tmpDrNurseCallDetails = await drNurseCallFieldAllocationRepository.GetFieldAllowCallDetails(companyId, singleTeamStatusDetails.TeamUserName, "TeamCall", DateTime.Today, DateTime.Today, "visited", "all", "all", "schedule", "all");
                        singleTeamStatusDetails.CallStatusVisitedCount = tmpDrNurseCallDetails.Count();

                        tmpDrNurseCallDetails = await drNurseCallFieldAllocationRepository.GetFieldAllowCallDetails(companyId, singleTeamStatusDetails.TeamUserName, "TeamCall", DateTime.Today, DateTime.Today, "notvisited", "all", "all", "schedule", "all");
                        singleTeamStatusDetails.CallStatusNotVisitedCount = tmpDrNurseCallDetails.Count();

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
*/
        async Task<List<TeamStatusDetails>> GetTeamDetails(string companyId, IDrNurseCallFieldAllocationRepository drNurseCallFieldAllocationRepository)
        {
            List<TeamStatusDetails> retTeamStatusDetails = new List<TeamStatusDetails>();
            try
            {
                var tableName = $"HC_Authentication.user_obj u, HC_Master_Details.company_obj co";

                var ColumAssign = $"u.company_id as CompanyId, co.company_name as CompanyName, " +
                                  $"u.full_name as TeamName, u.user_name as TeamUserName";

                var whereCond = $" where u.user_type = 7 and u.company_id = co.company_id" +
                                $" and u.status = 'Active'" +
                                $" and co.status = 'Active'";

                string todayDate = DateTime.Today.ToString("yyyy-MM-dd 00:00:00.0");

                if (!string.IsNullOrEmpty(companyId))
                    whereCond += " and u.company_id = '" + companyId + "'";

                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelQuery = $"select " + ColumAssign + " from " + tableName + whereCond;
                    var sqlSelResult = await connection.QueryAsync<TeamStatusDetails>(sqlSelQuery);

                    foreach(TeamStatusDetails singleTeamStatusDetails in sqlSelResult.ToList())
                    {//called , pending, visited, notvisited
                        List<DrNurseCallDetails> tmpDrNurseCallDetails = await drNurseCallFieldAllocationRepository.GetDashBoardDetails(companyId, singleTeamStatusDetails.TeamUserName, "allow", "all");
                        singleTeamStatusDetails.AllocatedCount = tmpDrNurseCallDetails.Count();

                        tmpDrNurseCallDetails = await drNurseCallFieldAllocationRepository.GetDashBoardDetails(companyId, singleTeamStatusDetails.TeamUserName, "auto", "all");
                        singleTeamStatusDetails.PreviousAutomaticAllocatedCount = tmpDrNurseCallDetails.Count();

                        tmpDrNurseCallDetails = await drNurseCallFieldAllocationRepository.GetDashBoardDetails(companyId, singleTeamStatusDetails.TeamUserName, "service", "pending");
                        singleTeamStatusDetails.CallStatusPendingCount = tmpDrNurseCallDetails.Count();

                        tmpDrNurseCallDetails = await drNurseCallFieldAllocationRepository.GetDashBoardDetails(companyId, singleTeamStatusDetails.TeamUserName, "service", "visited");
                        singleTeamStatusDetails.CallStatusVisitedCount = tmpDrNurseCallDetails.Count();

                        tmpDrNurseCallDetails = await drNurseCallFieldAllocationRepository.GetDashBoardDetails(companyId, singleTeamStatusDetails.TeamUserName, "service", "notvisited");
                        singleTeamStatusDetails.CallStatusNotVisitedCount = tmpDrNurseCallDetails.Count();

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