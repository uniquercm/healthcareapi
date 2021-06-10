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

        public async Task<List<DrNurseCallDetails>> GetFieldAllowCallDetails(string companyId, string teamUserName, string callName, DateTime scheduledFromDate, DateTime scheduledToDate, string callStatus, string serviceStatus)
        {
            List<DrNurseCallDetails> retDrNurseCallDetails = new List<DrNurseCallDetails>();
            List<DrNurseCallDetails> dayCallDetails = new List<DrNurseCallDetails>();
            try
            {
                if(!String.IsNullOrEmpty(serviceStatus))
                {
                    if((!callName.Equals("DrCall")) && (serviceStatus.Equals("all") || !serviceStatus.Equals("4pcr") 
                    || !serviceStatus.Equals("8pcr")))
                        retDrNurseCallDetails = await GetDrNurseCallDetails(companyId, teamUserName, "NurseCall", scheduledFromDate, scheduledToDate, callStatus, serviceStatus);

                    if(serviceStatus.Equals("4pcr") || serviceStatus.Equals("all"))
                    {
                        dayCallDetails = await GetPCRCallDetails(companyId, teamUserName, true, scheduledFromDate, scheduledToDate, serviceStatus);
                        if(retDrNurseCallDetails.Count > 0)
                        {
                            foreach(DrNurseCallDetails singleDrNurseCallDetails in dayCallDetails)
                                retDrNurseCallDetails.Add(singleDrNurseCallDetails);
                        }
                        else
                            retDrNurseCallDetails = dayCallDetails;
                    }
                /*}

                 if(!String.IsNullOrEmpty(serviceStatus))
                {*/
                    if(serviceStatus.Equals("8pcr") || serviceStatus.Equals("all"))
                    {
                        dayCallDetails = await GetPCRCallDetails(companyId, teamUserName, false, scheduledFromDate, scheduledToDate, serviceStatus);
                        if(retDrNurseCallDetails.Count > 0)
                        {
                            foreach(DrNurseCallDetails singleDrNurseCallDetails in dayCallDetails)
                                retDrNurseCallDetails.Add(singleDrNurseCallDetails);
                        }
                        else
                            retDrNurseCallDetails = dayCallDetails;
                    }
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retDrNurseCallDetails;
        }
        public async Task<List<DrNurseCallDetails>> GetDrNurseCallDetails(string companyId, string teamUserName, string callName, DateTime scheduledFromDate, DateTime scheduledToDate, string callStatus, string serviceStatus)
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
                else
                    fromDate = scheduledFromDate.ToString("yyyy-MM-dd 00:00:00.0");

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

                if(!string.IsNullOrEmpty(teamUserName) && callName != "DrCall")
                    whereCond += " and ((sc.allocated_team_name = '" + teamUserName + "' and sc.reallocated_team_name = '')" +
                                 " or (sc.allocated_team_name != '' and sc.reallocated_team_name = '" + teamUserName + "'))";

                if(!String.IsNullOrEmpty(serviceStatus) && callName != "DrCall")
                {//all, tracker, sticker, 4pcr, 8pcr, discharge
                    /*if(serviceStatus.Equals("tracker"))
                    whereCond += $" and sc.sticker_application = 'yes'";
                    else */if(serviceStatus.Equals("sticker"))
                        whereCond += $" and p.sticker_application = 'yes'";
                    else if(serviceStatus.Equals("discharge"))
                        whereCond += $" and sc.discharge_date = '" + fromDate + "'";
                }

                var orderCond = $" order by sc.created_on DESC ";

                var sqlSelQuery = $"select " + ColumAssign + " from " + tableName + whereCond + orderCond;
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
        public async Task<List<DrNurseCallDetails>> GetTeamFieldAllowCallDetails1(string companyId, string teamUserName, DateTime scheduledFromDate, DateTime scheduledToDate, string callStatus, string serviceStatus)
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
                else
                    fromDate = scheduledFromDate.ToString("yyyy-MM-dd 00:00:00.0");

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

                if (!string.IsNullOrEmpty(companyId))
                    whereCond += " and p.company_id = '" + companyId + "'";

                if (!string.IsNullOrEmpty(callStatus))
                {
                    if (!callStatus.ToLower().Equals("all"))
                        whereCond += " and ca.call_status = '" + callStatus + "'";
                }

                if(!String.IsNullOrEmpty(serviceStatus))
                {//all, tracker, sticker, 4pcr, 8pcr, discharge
                    if(serviceStatus.Equals("sticker"))
                        whereCond += $" and p.sticker_application = 'yes'";
                    else if(serviceStatus.Equals("discharge"))
                        whereCond += $" and sc.discharge_date = '" + fromDate + "'";
                }

                if(!string.IsNullOrEmpty(teamUserName))
                    whereCond += " and ((sc.allocated_team_name = '" + teamUserName + "' and sc.reallocated_team_name = '')" +
                                 " or (sc.allocated_team_name != '' and sc.reallocated_team_name = '" + teamUserName + "'))";

                var orderCond = $" order by sc.created_on DESC ";

                var sqlSelQuery = $"select " + ColumAssign + " from " + tableName + whereCond + orderCond;
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

        async Task<List<DrNurseCallDetails>> GetPCRCallDetails(string companyId, string teamUserName, bool is4thDay, DateTime scheduledFromDate, DateTime scheduledToDate, string serviceStatus)
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
                else
                    fromDate = scheduledFromDate.ToString("yyyy-MM-dd 00:00:00.0");

                //scheduledToDate = DateTime.Today.AddDays(1);
                string toDate = scheduledToDate.Date.ToString("dd-MM-yyyy");
                if(toDate != "01-01-0001")
                {
                    toDate = scheduledToDate.ToString("yyyy-MM-dd 00:00:00.0");
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

                if(!string.IsNullOrEmpty(teamUserName))
                    whereCond += " and ((sc.allocated_team_name = '" + teamUserName + "' and sc.reallocated_team_name = '')" +
                                 " or (sc.allocated_team_name != '' and sc.reallocated_team_name = '" + teamUserName + "'))";

                var orderCond = $" order by sc.created_on DESC ";

                var sqlSelQuery = $"select " + ColumAssign + " from " + tableName + whereCond + orderCond;
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

        public async Task<bool> EditServicePlan(ServicePlanRequest servicePlanRequest)
        {
            try
            {
                await EditPatientEnrollmentDetails(servicePlanRequest);
                await EditScheduleStickerTrackerDetails(servicePlanRequest);
                return true;
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
                return false;
            }
        }
        async Task<bool> EditPatientEnrollmentDetails(ServicePlanRequest servicePlanRequest)
        {
            try
            {
                bool sqlResult = true;
                var tableName = $"HC_Staff_Patient.patient_obj";

                var colName = $"enrolled_count = @EnrolledCount, enrolled_details = @EnrolledDetails, " +
                              $"sticker_application = @StickerApplication, tracker_application = @TrackerApplication, " +
                              $"sticker_removal = @StickerRemoval, tracker_removal = @TrackerRemoval, " +
                              $"modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                var whereCond = $" where patient_id = @PatientId";
                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                object colValueParam = new
                {
                    PatientId = servicePlanRequest.PatientId,
                    EnrolledCount = servicePlanRequest.EnrolledCount,
                    EnrolledDetails = servicePlanRequest.EnrolledDetails,
                    StickerApplication = servicePlanRequest.StickerApplication,
                    TrackerApplication = servicePlanRequest.TrackerApplication,
                    StickerRemoval = servicePlanRequest.StickerRemoval,
                    TrackerRemoval = servicePlanRequest.TrackerRemoval,
                    ModifiedBy = servicePlanRequest.ModifiedBy,
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

        async Task<bool> EditScheduleStickerTrackerDetails(ServicePlanRequest servicePlanRequest)
        {
            try
            {
                bool sqlResult = true;
                var tableName = $"HC_Staff_Patient.scheduled_obj";

                var colName = $"tracker_applied_date = @TrackerAppliedDate, sticker_removed_date = @StickerRemovedDate, " +
                              $"sticker_tracker_no = @StickerTrackerNumber, sticker_tracker_result = @StickerTrackerResult, " +
                              $"tracker_replace_date = @TrackerReplacedDate, tracker_replace_no = @TrackerReplaceNumber, " +
                              $"modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                var whereCond = $" where patient_id = @PatientId and scheduled_id = @ScheduledId";
                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                string appliedDate = "";
                if(servicePlanRequest.TrackerAppliedDate == null)
                    appliedDate = "";
                else
                {
                    appliedDate = servicePlanRequest.TrackerAppliedDate.ToString("yyyy-MM-dd");
                    if( appliedDate == "0001-01-01")
                        appliedDate = "";
                    else
                        appliedDate = servicePlanRequest.TrackerAppliedDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                string removedDate = "";
                if(servicePlanRequest.StickerRemovedDate == null)
                    removedDate = "";
                else
                {
                    removedDate = servicePlanRequest.StickerRemovedDate.ToString("yyyy-MM-dd");
                    if( removedDate == "0001-01-01")
                        removedDate = "";
                    else
                        removedDate = servicePlanRequest.StickerRemovedDate.ToString("yyyy-MM-dd 00:00:00.0");
                } 

                string replacedDate = "";
                if(servicePlanRequest.TrackerReplacedDate == null)
                    replacedDate = "";
                else
                {
                    replacedDate = servicePlanRequest.TrackerReplacedDate.ToString("yyyy-MM-dd");
                    if( replacedDate == "0001-01-01")
                        replacedDate = "";
                    else
                        replacedDate = servicePlanRequest.TrackerReplacedDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                object colValueParam = new
                {
                    PatientId = servicePlanRequest.PatientId,
                    ScheduledId = servicePlanRequest.ScheduledId,
                    TrackerAppliedDate = appliedDate,//
                    StickerRemovedDate = removedDate,//
                    StickerTrackerNumber = servicePlanRequest.StickerTrackerNumber,//
                    TrackerReplacedDate = replacedDate,//
                    TrackerReplaceNumber = servicePlanRequest.TrackerReplaceNumber,//
                    StickerTrackerResult = servicePlanRequest.StickerTrackerResult,//
                    ModifiedBy = servicePlanRequest.ModifiedBy,
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