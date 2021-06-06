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
    internal sealed class DrNurseCallFieldAllocationRepository : IDrNurseCallFieldAllocationRepository
    {
        private readonly AppDbContext _appDbContext;
        public DrNurseCallFieldAllocationRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<DrNurseCallDetails>> GetFieldAllowCallDetails(string companyId, DateTime scheduledFromDate, DateTime scheduledToDate, string callStatus)
        {
            List<DrNurseCallDetails> retDrNurseCallDetails = new List<DrNurseCallDetails>();
            List<DrNurseCallDetails> dayCallDetails = new List<DrNurseCallDetails>();
            try
            {
                retDrNurseCallDetails = await GetDrNurseCallDetails(companyId, "NurseCall", scheduledFromDate, scheduledToDate, callStatus);

                dayCallDetails = await GetPCRCallDetails(companyId, true, scheduledFromDate, scheduledToDate);
                if(retDrNurseCallDetails.Count > 0)
                {
                    foreach(DrNurseCallDetails singleDrNurseCallDetails in dayCallDetails)
                        retDrNurseCallDetails.Add(singleDrNurseCallDetails);
                }
                else
                    retDrNurseCallDetails = dayCallDetails;

                dayCallDetails = await GetPCRCallDetails(companyId, false, scheduledFromDate, scheduledToDate);
                if(retDrNurseCallDetails.Count > 0)
                {
                    foreach(DrNurseCallDetails singleDrNurseCallDetails in dayCallDetails)
                        retDrNurseCallDetails.Add(singleDrNurseCallDetails);
                }
                else
                    retDrNurseCallDetails = dayCallDetails;
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retDrNurseCallDetails;
        }
        public async Task<List<DrNurseCallDetails>> GetDrNurseCallDetails(string companyId, string callName, DateTime scheduledFromDate, DateTime scheduledToDate, string callStatus)
        {
            List<DrNurseCallDetails> retDrNurseCallDetails = new List<DrNurseCallDetails>();
            try
            {
                var tableName = $"HC_Staff_Patient.patient_obj p, " +
                                $"HC_Master_Details.company_obj co, " +
                                $"HC_Treatment.scheduled_obj sc, " +
                                $"HC_Treatment.call_obj ca";

                var ColumAssign = $"p.patient_id as PatientId, p.patient_name as PatientName, " +
                                  $"p.company_id as CompanyId, co.company_name as CompanyName, " +
                                  $"p.request_id as RequestId, p.crm_no as CRMNo, p.eid_no as EIDNo, " +
                                  $"p.mobile_no as MobileNo, ca.call_id as CallId, " +
                                  $"ca.called_date as CalledDate, ca.scheduled_id as ScheduledId, " +
                                  $"ca.call_scheduled_date as CallScheduledDate, " +
                                  $"ca.call_status as CallStatus, ca.remarks as Remarks, " +
                                  $"ca.emr_done as EMRDone, ca.is_pcr as IsPCRCall";

                var whereCond = $" where p.company_id = co.company_id" +
                                $" and p.patient_id = sc.patient_id" +
                                $" and ca.scheduled_id = sc.scheduled_id";

                string fromDate = scheduledFromDate.Date.ToString("dd-MM-yyyy");
                if(fromDate == "01-01-0001")
                {
                    scheduledFromDate = DateTime.Today;
                    //scheduledFromDate = DateTime.Today.AddDays(1);
                    fromDate = scheduledFromDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                //scheduledToDate = DateTime.Today.AddDays(1);
                string toDate = scheduledToDate.Date.ToString("dd-MM-yyyy");
                if(toDate != "01-01-0001")
                {
                    if(fromDate != "01-01-0001")
                        whereCond += $" and (ca.call_scheduled_date between '" + fromDate + "'";

                    toDate = scheduledToDate.ToString("yyyy-MM-dd 00:00:00.0");
                    whereCond += $" and '" + toDate + "'";

                    whereCond += $")";
                }
                else
                {
                    if(fromDate != "01-01-0001")
                        whereCond += $" and ca.call_scheduled_date = '" + fromDate + "'";
                }

                if(callName == "DrCall")
                    whereCond += $" and sc.2day_call_id = ca.call_id";
                else if(callName == "NurseCall")
                    whereCond += $" and sc.2day_call_id <> ca.call_id";

                if (!string.IsNullOrEmpty(companyId))
                    whereCond += " and p.company_id = '" + companyId + "'";

                if (!string.IsNullOrEmpty(callStatus))
                {
                    if (!callStatus.ToLower().Equals("all"))
                        whereCond += " and ca.call_status = '" + callStatus + "'";
                }

                var sqlSelQuery = $"select " + ColumAssign + " from " + tableName + whereCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelResult = await connection.QueryAsync<DrNurseCallDetails>(sqlSelQuery);
                    retDrNurseCallDetails = sqlSelResult.ToList();
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retDrNurseCallDetails;
        }
        public async Task<List<DrNurseCallDetails>> GetTeamFieldAllowCallDetails(string companyId, string teamUserName, DateTime scheduledFromDate, DateTime scheduledToDate, string callStatus)
        {
            List<DrNurseCallDetails> retDrNurseCallDetails = new List<DrNurseCallDetails>();
            try
            {
                var tableName = $"HC_Staff_Patient.patient_obj p, " +
                                $"HC_Master_Details.company_obj co, " +
                                $"HC_Treatment.scheduled_obj sc, " +
                                $"HC_Treatment.call_obj ca";

                var ColumAssign = $"p.patient_id as PatientId, p.patient_name as PatientName, " +
                                  $"p.company_id as CompanyId, co.company_name as CompanyName, " +
                                  $"p.request_id as RequestId, p.crm_no as CRMNo, p.eid_no as EIDNo, " +
                                  $"p.mobile_no as MobileNo, ca.call_id as CallId, " +
                                  $"ca.called_date as CalledDate, ca.scheduled_id as ScheduledId, " +
                                  $"ca.call_scheduled_date as CallScheduledDate, " +
                                  $"ca.call_status as CallStatus, ca.remarks as Remarks, " +
                                  $"ca.emr_done as EMRDone, ca.is_pcr as IsPCRCall";

                var whereCond = $" where p.company_id = co.company_id" +
                                $" and p.patient_id = sc.patient_id" +
                                $" and ca.scheduled_id = sc.scheduled_id";

                string fromDate = scheduledFromDate.Date.ToString("dd-MM-yyyy");
                if(fromDate == "01-01-0001")
                {
                    scheduledFromDate = DateTime.Today;
                    //scheduledFromDate = DateTime.Today.AddDays(1);
                    fromDate = scheduledFromDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                //scheduledToDate = DateTime.Today.AddDays(1);
                string toDate = scheduledToDate.Date.ToString("dd-MM-yyyy");
                if(toDate != "01-01-0001")
                {
                    if(fromDate != "01-01-0001")
                        whereCond += $" and (ca.call_scheduled_date between '" + fromDate + "'";

                    toDate = scheduledToDate.ToString("yyyy-MM-dd 00:00:00.0");
                    whereCond += $" and '" + toDate + "'";

                    whereCond += $")";
                }
                else
                {
                    if(fromDate != "01-01-0001")
                        whereCond += $" and ca.call_scheduled_date = '" + fromDate + "'";
                }

                whereCond += $" and sc.2day_call_id <> ca.call_id";

                if(!string.IsNullOrEmpty(teamUserName))
                    whereCond += " and ((sc.allocated_team_name = '" + teamUserName + "' and sc.reallocated_team_name = '')" +
                                 " or (sc.allocated_team_name != '' and sc.reallocated_team_name = '" + teamUserName + "'))";

                if (!string.IsNullOrEmpty(companyId))
                    whereCond += " and p.company_id = '" + companyId + "'";

                if (!string.IsNullOrEmpty(callStatus))
                {
                    if (!callStatus.ToLower().Equals("all"))
                        whereCond += " and ca.call_status = '" + callStatus + "'";
                }

                var sqlSelQuery = $"select " + ColumAssign + " from " + tableName + whereCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelResult = await connection.QueryAsync<DrNurseCallDetails>(sqlSelQuery);
                    retDrNurseCallDetails = sqlSelResult.ToList();
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retDrNurseCallDetails;
        }

        async Task<List<DrNurseCallDetails>> GetPCRCallDetails(string companyId, bool is4thDay, DateTime scheduledFromDate, DateTime scheduledToDate)
        {
            List<DrNurseCallDetails> retDrNurseCallDetails = new List<DrNurseCallDetails>();
            try
            {
                var tableName = $"HC_Staff_Patient.patient_obj p, " +
                                $"HC_Master_Details.company_obj co, " +
                                $"HC_Treatment.scheduled_obj sc";

                var ColumAssign = $"p.patient_id as PatientId, p.patient_name as PatientName, " +
                                  $"p.company_id as CompanyId, co.company_name as CompanyName, " +
                                  $"p.request_id as RequestId, p.crm_no as CRMNo, p.eid_no as EIDNo, " +
                                  $"p.mobile_no as MobileNo, sc.scheduled_id as ScheduledId, " +
                                  $"true as IsPCRCall, ";
                if(is4thDay)
                    ColumAssign += "'4thday' as CallId, sc.4day_pcr_test_date as CallScheduledDate, " +
                                  $"sc.4day_pcr_test_sample_date as CalledDate, "+
                                  $"sc.4day_pcr_test_result as CallStatus";
                else
                    ColumAssign += "'8thday' as CallId, sc.8day_pcr_test_date as CallScheduledDate, " +
                                  $"sc.8day_pcr_test_sample_date as CalledDate, "+
                                  $"sc.8day_pcr_test_result as CallStatus";

                var whereCond = $" where p.company_id = co.company_id" +
                                $" and p.patient_id = sc.patient_id";

                string fromDate = scheduledFromDate.Date.ToString("dd-MM-yyyy");
                if(fromDate == "01-01-0001")
                {
                    scheduledFromDate = DateTime.Today;
                    //scheduledFromDate = DateTime.Today.AddDays(1);
                    fromDate = scheduledFromDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                //scheduledToDate = DateTime.Today.AddDays(1);
                string toDate = scheduledToDate.Date.ToString("dd-MM-yyyy");
                if(toDate != "01-01-0001")
                {
                    if(is4thDay)
                        whereCond += $" and sc.4day_pcr_test_date between '" + fromDate + "' and '" + toDate + "'";
                    else
                        whereCond += $" and sc.8day_pcr_test_date between '" + fromDate + "' and '" + toDate + "'";
                }
                else if(is4thDay)
                    whereCond += $" and sc.4day_pcr_test_date = '" + fromDate + "'";
                else
                    whereCond += $" and sc.8day_pcr_test_date = '" + fromDate + "'";

                if (!string.IsNullOrEmpty(companyId))
                    whereCond += " and p.company_id = '" + companyId + "'";

                var sqlSelQuery = $"select " + ColumAssign + " from " + tableName + whereCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelResult = await connection.QueryAsync<DrNurseCallDetails>(sqlSelQuery);
                    retDrNurseCallDetails = sqlSelResult.ToList();
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retDrNurseCallDetails;
        }

        /*public async Task<bool> EditTeamFieldAllowCallDetails(CallRequest callRequest)
        {
            try
            {
                bool sqlResult = true;
                //if(callRequest)
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
                return false;
            }
        }*/
        public async Task<bool> EditPCRCall(CallRequest callRequest)
        {
            try
            {
                bool sqlResult = true;
                var tableName = $"HC_Treatment.scheduled_obj";

                var colName = $"scheduled_id = @ScheduledId, " +
                              //$"remarks = @Remarks, emr_done = @EMRDone, is_pcr = @IsPCRCall," +
                              $"modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                if(!String.IsNullOrEmpty(callRequest.CallId))
                {
                    if(callRequest.CallId.ToLower().Equals("4thday"))
                    {
                        colName += $", 4day_pcr_test_sample_date = @CalledDate, " +
                                   $"4day_pcr_test_result = @CallStatus";
                    }
                    else if(callRequest.CallId.ToLower().Equals("8thday"))
                    {
                        colName += $", 8day_pcr_test_sample_date = @CalledDate, " +
                                   $"8day_pcr_test_result = @CallStatus";
                    }
                }
                else
                    return false;

                var whereCond = $" where scheduled_id = @ScheduledId";

                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                string calledDate;
                if(callRequest.CalledDate == null)
                    calledDate = "";
                else
                {
                    calledDate = callRequest.CalledDate.ToString("yyyy-MM-dd");
                    if( calledDate == "0001-01-01")
                        calledDate = "";
                    else
                        calledDate = callRequest.CalledDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                object colValueParam = new
                {
                    ScheduledId = callRequest.ScheduledId,
                    CalledDate = calledDate,
                    CallStatus = callRequest.CallStatus,
                    //Remarks = callRequest.Remarks,
                    //EMRDone = callRequest.EMRDone,
                    //IsPCRCall = callRequest.IsPCRCall,
                    ModifiedBy = callRequest.ModifiedBy,
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

    }
}