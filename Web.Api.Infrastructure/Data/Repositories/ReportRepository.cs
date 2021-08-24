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
    internal sealed class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _appDbContext;

        public ReportRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<ReportDetails>> GetReportDetails(string companyId, string patientId, string scheduledId, string extractData, string sendClaim, DateTime sendClaimOnFromDate, DateTime sendClaimOnToDate, string areaNames)
        {
            List<ReportDetails> retReportDetailsList = new List<ReportDetails>();
            try
            {
                var tableName = $"HC_Master_Details.company_obj co, " +
                                $"HC_Master_Details.request_crm_obj rc, " +
                                $"HC_Staff_Patient.patient_obj pa, " +
                                $"HC_Master_Details.city_obj ci, " +
                                $"HC_Treatment.scheduled_obj sc";

                var ColumAssign = $"sc.scheduled_id as ScheduledId, " +
                                  $"pa.patient_id as PatientId, pa.patient_name as PatientName," +
                                  $"pa.company_id as CompanyId, co.company_name as CompanyName, " +
                                  $"pa.request_id as RequestId, rc.request_crm_name as RequestCrmName, " +
                                  $"pa.crm_no as CRMNo, pa.eid_no as EIDNo, pa.mobile_no as MobileNo, " +
                                  $"pa.assigned_date as AssignedDate, pa.area as Area, " +
                                  $"pa.city_id as CityId, ci.city_name as CityName, " +
                                  $"pa.no_of_adults as AdultsCount, pa.no_of_childrens as ChildrensCount, " +
                                  $"pa.enrolled_count as EnrolledCount, pa.enrolled_details as EnrolledDetails, " +
                                  $"pa.reception_date as RecptionCallDate, pa.reception_status as RecptionCallStatus, pa.reception_remarks as RecptionCallRemarks, " +
                                  $"sc.2day_call_id as Day2CallId, " +
                                  //$"sc.4day_pcr_test_date as PCR4DayTestDate, sc.4day_pcr_test_sample_date as PCR4DaySampleDate, " +
                                  //$"sc.4day_pcr_test_result as PCR4DayResult, " +
                                  $"sc.6day_pcr_test_date as PCR6DayTestDate, sc.6day_pcr_test_sample_date as PCR6DaySampleDate, " +
                                  $"sc.6day_pcr_test_result as PCR6DayResult, " +
                                  $"sc.8day_pcr_test_date as PCR8DayTestDate, sc.8day_pcr_test_sample_date as PCR8DaySampleDate, " +
                                  $"sc.8day_pcr_test_result as PCR8DayResult, " +
                                  $"sc.9day_pcr_test_date as PCR9DayTestDate, sc.9day_pcr_test_sample_date as PCR9DaySampleDate, " +
                                  $"sc.9day_pcr_test_result as PCR9DayResult, " +
                                  $"sc.11day_pcr_test_date as PCR11DayTestDate, sc.11day_pcr_test_sample_date as PCR11DaySampleDate, " +
                                  $"sc.11day_pcr_test_result as PCR11DayResult, " +
                                  $"sc.3day_call_id as Day3CallId, sc.4day_call_id as Day4CallId, " +
                                  $"sc.5day_call_id as Day5CallId, sc.6day_call_id as Day6CallId, " +
                                  $"sc.7day_call_id as Day7CallId, sc.9day_call_id as Day9CallId, " +
                                  $"sc.discharge_date as DischargeDate, sc.discharge_status as DischargeStatus, sc.discharge_remarks as DischargeRemarks, " +
                                  $"sc.have_treatement_extract as IsExtractTreatementDate, " +
                                  $"sc.have_send_claim as IsSendClaim, sc.claim_send_date as SendingClaimDate";

                var whereCond = $" where sc.patient_id = pa.patient_id" +
                                $" and pa.company_id = co.company_id" +
                                $" and pa.city_id = ci.city_id" +
                                $" and pa.request_id = rc.request_crm_id" +
                                $" and pa.status = 'Active'" +
                                $" and sc.status = 'Active'";

                if (!string.IsNullOrEmpty(companyId))
                    whereCond += " and pa.company_id = '" + companyId + "'";

                if (!string.IsNullOrEmpty(patientId))
                    whereCond += " and pa.patient_id = '" + patientId + "'";

                if (!string.IsNullOrEmpty(scheduledId))
                    whereCond += " and sc.scheduled_id = '" + scheduledId + "'";

                if (!string.IsNullOrEmpty(sendClaim))
                {
                    if (!sendClaim.ToLower().Equals("all"))
                        whereCond += " and sc.have_send_claim = '" + sendClaim + "'";
                }

                string minits = " 00:00:00.0'";
                string sendingFromDate = "0001-01-01";
                if(sendClaimOnFromDate != null)
                    sendingFromDate = sendClaimOnFromDate.ToString("yyyy-MM-dd");
                if(sendingFromDate == "0001-01-01")
                    sendingFromDate = DateTime.Today.ToString("yyyy-MM-dd");

                string sendingToDate = "0001-01-01";
                if(sendClaimOnToDate != null)
                    sendingToDate = sendClaimOnToDate.ToString("yyyy-MM-dd");

                /*In 30-7-2021 change the sending on search to assigned date
                if(sendingFromDate != "0001-01-01" && sendingToDate != "0001-01-01")
                    whereCond += " and sc.claim_send_date between '" + sendingFromDate + minits +
                                $" and '" + sendingToDate + minits;
                else if(sendingFromDate != "0001-01-01")
                    whereCond += " and sc.claim_send_date = '" + sendingFromDate + minits;
                else if(sendingToDate != "0001-01-01")
                    whereCond += " and sc.claim_send_date = '" + sendingToDate + minits;*/
                //pa.assigned_date
                if(sendingFromDate != "0001-01-01" && sendingToDate != "0001-01-01")
                    whereCond += " and pa.assigned_date between '" + sendingFromDate + minits +
                                $" and '" + sendingToDate + minits;
                else if(sendingFromDate != "0001-01-01")
                    whereCond += " and pa.assigned_date = '" + sendingFromDate + minits;
                else if(sendingToDate != "0001-01-01")
                    whereCond += " and pa.assigned_date = '" + sendingToDate + minits;

                if (!string.IsNullOrEmpty(extractData))
                {
                    if (!extractData.ToLower().Equals("all"))
                        whereCond += " and sc.have_treatement_extract = '" + extractData + "'";
                }

                if(!String.IsNullOrEmpty(areaNames) && areaNames.ToLower() != "all")
                {
                    string[] areaArray = areaNames.Replace("[","").Replace("]","").Replace("\"","").Split(',');

                    for(int i = 0; i < areaArray.Count(); i++)
                    {
                        if(i == 0 && areaArray.Count() > 1)
                            whereCond += " and (pa.area = '" + areaArray[i] + "'";
                        else if(i == 0)
                            whereCond += " and pa.area = '" + areaArray[i] + "'";
                        else if(i == areaArray.Count()-1)
                            whereCond += " or pa.area = '" + areaArray[i] + "')";
                        else 
                            whereCond += " or pa.area = '" + areaArray[i] + "'";
                    }
                }

                var orderCond = $" order by sc.created_on DESC ";

                var sqlSelQuery = $"select " + ColumAssign + " from " + tableName + whereCond + orderCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelResult = await connection.QueryAsync<ReportDetails>(sqlSelQuery);
                    //int forcount = 0;
                    foreach(ReportDetails singleReportDetails in sqlSelResult.ToList())
                    {
                        /*if(forcount == 53)
                        {
                            var countted = "";
                        }
                        forcount += 1;*/
                        try{
                        if(!String.IsNullOrEmpty(singleReportDetails.EnrolledDetails))
                        {
                            singleReportDetails.EnrolledDetails = singleReportDetails.EnrolledDetails.Replace("[","");
                            singleReportDetails.EnrolledDetails = singleReportDetails.EnrolledDetails.Replace("]","");
                            singleReportDetails.EnrolledDetails = singleReportDetails.EnrolledDetails.Replace("\"","");
                        }

                        CallDetails callDetails = new CallDetails();
                        callDetails = await GetCallDetails(singleReportDetails.Day2CallId, singleReportDetails.ScheduledId);
                        if(callDetails == null)
                            callDetails = new CallDetails();
                        singleReportDetails.DrCallStatus = callDetails.CallStatus;
                        singleReportDetails.DrCallRemarks = callDetails.Remarks;

                        callDetails = new CallDetails();
                        callDetails = await GetCallDetails(singleReportDetails.Day3CallId, singleReportDetails.ScheduledId);
                        if(callDetails == null)
                            callDetails = new CallDetails();
                        singleReportDetails.Day3CallStatus = callDetails.CallStatus;
                        singleReportDetails.Day3CallRemarks = callDetails.Remarks;

                        callDetails = new CallDetails();
                        callDetails = await GetCallDetails(singleReportDetails.Day4CallId, singleReportDetails.ScheduledId);
                        if(callDetails == null)
                            callDetails = new CallDetails();
                        singleReportDetails.Day4CallStatus = callDetails.CallStatus;
                        singleReportDetails.Day4CallRemarks = callDetails.Remarks;

                        callDetails = new CallDetails();
                        callDetails = await GetCallDetails(singleReportDetails.Day5CallId, singleReportDetails.ScheduledId);
                        if(callDetails == null)
                            callDetails = new CallDetails();
                        singleReportDetails.Day5CallStatus = callDetails.CallStatus;
                        singleReportDetails.Day5CallRemarks = callDetails.Remarks;

                        callDetails = new CallDetails();
                        callDetails = await GetCallDetails(singleReportDetails.Day6CallId, singleReportDetails.ScheduledId);
                        if(callDetails == null)
                            callDetails = new CallDetails();
                        singleReportDetails.Day6CallStatus = callDetails.CallStatus;
                        singleReportDetails.Day6CallRemarks = callDetails.Remarks;

                        callDetails = new CallDetails();
                        callDetails = await GetCallDetails(singleReportDetails.Day7CallId, singleReportDetails.ScheduledId);
                        if(callDetails == null)
                            callDetails = new CallDetails();
                        singleReportDetails.Day7CallStatus = callDetails.CallStatus;
                        singleReportDetails.Day7CallRemarks = callDetails.Remarks;

                        callDetails = new CallDetails();
                        callDetails = await GetCallDetails(singleReportDetails.Day9CallId, singleReportDetails.ScheduledId);
                        if(callDetails == null)
                            callDetails = new CallDetails();
                        singleReportDetails.Day9CallStatus = callDetails.CallStatus;
                        singleReportDetails.Day9CallRemarks = callDetails.Remarks;

                        retReportDetailsList.Add(singleReportDetails);
                        }
                        catch(Exception Erro)
                        {
                            var error = Erro.Message.ToString();
                        }
                    }
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retReportDetailsList;
        }
        public async Task<CallDetails> GetCallDetails(string callId, string scheduledId)
        {
            CallDetails retCallDetails = new CallDetails();
            try
            {
                var tableName = $"HC_Treatment.call_obj";

                var ColumAssign = $"call_id as CallId, scheduled_id as ScheduledId, " +
                                  $"call_scheduled_date as CallScheduledDate, called_date as CalledDate, " +
                                  $"call_status as CallStatus, remarks as Remarks, " +
                                  $"emr_done as EMRDone, " +
                                  $"created_by as CreatedBy, modified_by as ModifiedBy";

                var whereCond = " where";

                if (!string.IsNullOrEmpty(callId))
                    whereCond += " call_id = '" + callId + "'";

                if (!string.IsNullOrEmpty(scheduledId))
                {
                    if (!string.IsNullOrEmpty(callId))
                        whereCond += " and scheduled_id = '" + scheduledId + "'";
                    else
                        whereCond += " scheduled_id = '" + scheduledId + "'";
                }

                var sqlSelQuery = $"select " + ColumAssign + " from " + tableName + whereCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelResult = await connection.QueryAsync<CallDetails>(sqlSelQuery);
                    retCallDetails = sqlSelResult.FirstOrDefault();
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retCallDetails;
        }
        public async Task<bool> EditReportDetails(ReportDetails reportDetails)
        {
            try
            {
                bool sqlResult = true;
                object colValueParam;

                var tableName = $"HC_Treatment.scheduled_obj";

                var colName = $"scheduled_id = @ScheduledId, " +
                              $"have_treatement_extract = @IsExtractTreatementDate, " +
                              $"have_send_claim = @IsSendClaim, claim_send_date = @SendingClaimDate, " +
                              $"modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                var whereCond = $" where scheduled_id = @ScheduledId";
                var sqlUpdateScheduleQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                string claimDate = reportDetails.SendingClaimDate.ToString("yyyy-MM-dd 00:00:00.0");
                if(claimDate == "0001-01-01")
                {
                    colValueParam = new
                    {
                        ScheduledId = reportDetails.ScheduledId,
                        PatientId = reportDetails.PatientId,
                        IsExtractTreatementDate = reportDetails.IsExtractTreatementDate,
                        IsSendClaim = reportDetails.IsSendClaim,
                        ModifiedBy = reportDetails.ModifiedBy,
                        ModifiedOn = DateTime.Today.ToString("yyyy-MM-dd 00:00:00.0")
                    };
                }
                else
                {
                    colValueParam = new
                    {
                        ScheduledId = reportDetails.ScheduledId,
                        PatientId = reportDetails.PatientId,
                        IsExtractTreatementDate = reportDetails.IsExtractTreatementDate,
                        IsSendClaim = reportDetails.IsSendClaim,
                        SendingClaimDate = claimDate,
                        ModifiedBy = reportDetails.ModifiedBy,
                        ModifiedOn = DateTime.Today.ToString("yyyy-MM-dd 00:00:00.0")
                    };
                }
                using (var connection = _appDbContext.Connection)
                {
                    sqlResult = Convert.ToBoolean(await connection.ExecuteAsync(sqlUpdateScheduleQuery, colValueParam));
                    return sqlResult;
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
                return false;
            }
        }

        public async Task<List<TeamReportDetails>> GetTeamReportDetails(string companyId, string teamUserName, DateTime scheduledFromDate, DateTime scheduledToDate)
        {
            List<TeamReportDetails> retTeamReportDetailsList = new List<TeamReportDetails>();
            List<TeamReportDetails> teamReportDetailsList = new List<TeamReportDetails>();
            try
            {
                string fromDate = scheduledFromDate.Date.ToString("yyyy-MM-dd");
                string toDate = scheduledToDate.Date.ToString("yyyy-MM-dd");
                if(fromDate == "0001-01-01")
                    fromDate = DateTime.Today.Date.ToString("yyyy-MM-dd");

                //tracker
                teamReportDetailsList = await GetServiceTeamReportDetails(companyId, teamUserName, fromDate, toDate, "tracker");
                if(retTeamReportDetailsList.Count() == 0)
                    retTeamReportDetailsList = teamReportDetailsList;
                else
                {
                    foreach(TeamReportDetails singleDrNurseCallDetails in teamReportDetailsList)
                        retTeamReportDetailsList.Add(singleDrNurseCallDetails);
                }
                //sticker
                teamReportDetailsList = await GetServiceTeamReportDetails(companyId, teamUserName, fromDate, toDate, "sticker");
                if(retTeamReportDetailsList.Count() == 0)
                    retTeamReportDetailsList = teamReportDetailsList;
                else
                {
                    foreach(TeamReportDetails singleDrNurseCallDetails in teamReportDetailsList)
                        retTeamReportDetailsList.Add(singleDrNurseCallDetails);
                }
                //replace
                teamReportDetailsList = await GetServiceTeamReportDetails(companyId, teamUserName, fromDate, toDate, "replace");
                if(retTeamReportDetailsList.Count() == 0)
                    retTeamReportDetailsList = teamReportDetailsList;
                else
                {
                    foreach(TeamReportDetails singleDrNurseCallDetails in teamReportDetailsList)
                        retTeamReportDetailsList.Add(singleDrNurseCallDetails);
                }
                //4
                /*teamReportDetailsList = await GetServiceTeamReportDetails(companyId, teamUserName, fromDate, toDate, "4");
                if(retTeamReportDetailsList.Count() == 0)
                    retTeamReportDetailsList = teamReportDetailsList;
                else
                {
                    foreach(TeamReportDetails singleDrNurseCallDetails in teamReportDetailsList)
                        retTeamReportDetailsList.Add(singleDrNurseCallDetails);
                }*/
                //6
                teamReportDetailsList = await GetServiceTeamReportDetails(companyId, teamUserName, fromDate, toDate, "6");
                if(retTeamReportDetailsList.Count() == 0)
                    retTeamReportDetailsList = teamReportDetailsList;
                else
                {
                    foreach(TeamReportDetails singleDrNurseCallDetails in teamReportDetailsList)
                        retTeamReportDetailsList.Add(singleDrNurseCallDetails);
                }
                //8
                teamReportDetailsList = await GetServiceTeamReportDetails(companyId, teamUserName, fromDate, toDate, "8");
                if(retTeamReportDetailsList.Count() == 0)
                    retTeamReportDetailsList = teamReportDetailsList;
                else
                {
                    foreach(TeamReportDetails singleDrNurseCallDetails in teamReportDetailsList)
                        retTeamReportDetailsList.Add(singleDrNurseCallDetails);
                }
                //9
                teamReportDetailsList = await GetServiceTeamReportDetails(companyId, teamUserName, fromDate, toDate, "9");
                if(retTeamReportDetailsList.Count() == 0)
                    retTeamReportDetailsList = teamReportDetailsList;
                else
                {
                    foreach(TeamReportDetails singleDrNurseCallDetails in teamReportDetailsList)
                        retTeamReportDetailsList.Add(singleDrNurseCallDetails);
                }
                //11
                teamReportDetailsList = await GetServiceTeamReportDetails(companyId, teamUserName, fromDate, toDate, "11");
                if(retTeamReportDetailsList.Count() == 0)
                    retTeamReportDetailsList = teamReportDetailsList;
                else
                {
                    foreach(TeamReportDetails singleDrNurseCallDetails in teamReportDetailsList)
                        retTeamReportDetailsList.Add(singleDrNurseCallDetails);
                }
                //discharge
                teamReportDetailsList = await GetServiceTeamReportDetails(companyId, teamUserName, fromDate, toDate, "discharge");
                if(retTeamReportDetailsList.Count() == 0)
                    retTeamReportDetailsList = teamReportDetailsList;
                else
                {
                    foreach(TeamReportDetails singleDrNurseCallDetails in teamReportDetailsList)
                        retTeamReportDetailsList.Add(singleDrNurseCallDetails);
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retTeamReportDetailsList;
        }

        async Task<List<TeamReportDetails>> GetServiceTeamReportDetails(string companyId, string teamUserName, string scheduledFromDate, string scheduledToDate, string serviceName)
        {
            List<TeamReportDetails> retTeamReportDetailsList = new List<TeamReportDetails>();
            try
            {
                if(!String.IsNullOrEmpty(serviceName))
                {
                    var dateWhereCondColumName = "";
                    var teamWhereCondColumName = "";
                    var schedateWhereCondColumName = "";
                    string minits = " 00:00:00.0";
                    var tableName = $"HC_Master_Details.company_obj co, " +
                                    $"HC_Master_Details.request_crm_obj rc, " +
                                    $"HC_Master_Details.city_obj ci, " +
                                    $"HC_Staff_Patient.patient_obj pa, " +
                                    $"HC_Treatment.scheduled_obj sc";

                    var ColumAssign = $"pa.patient_id as PatientId, pa.patient_name as PatientName," +
                                    $"pa.company_id as CompanyId, co.company_name as CompanyName, " +
                                    $"pa.request_id as RequestId, rc.request_crm_name as RequestCrmName, " +
                                    $"pa.crm_no as CRMNo, pa.eid_no as EIDNo, pa.mobile_no as MobileNo, " +
                                    $"pa.no_of_adults as AdultsCount, pa.no_of_childrens as ChildrensCount, " +
                                    $"pa.area as Area, " +
                                    $"pa.city_id as CityId, ci.city_name as CityName, " +
                                    $"sc.allocated_team_name as AllocatedTeamName, sc.team_allocated_date as AllocatedDate, " +
                                    $"sc.reallocated_team_name as ReAllocatedTeamName, sc.team_reallocated_date as ReAllocatedDate, " ;

                    var whereCond = $" where sc.patient_id = pa.patient_id" +
                                    $" and pa.company_id = co.company_id" +
                                    $" and pa.city_id = ci.city_id" +
                                    $" and pa.request_id = rc.request_crm_id" +
                                    $" and pa.status = 'Active'" +
                                    $" and sc.status = 'Active'";

                    if(serviceName.Equals("tracker"))
                    {
                        ColumAssign += $"'Tracker Application' as ServiceName, sc.tracker_schedule_date as ServiceScheduleDate, " +
                                    $"sc.tracker_team_user_name as VisitedTeamName, " +
                                    $"sc.tracker_team_status as TeamStatus, " +
                                    $"sc.tracker_team_remark as TeamRemark, " +
                                    $"sc.tracker_team_date as TeamVisitedDate";
                        dateWhereCondColumName = $"sc.tracker_team_date";
                        schedateWhereCondColumName = $"sc.tracker_schedule_date";
                        teamWhereCondColumName = $"sc.tracker_team_user_name";
                    }
                    else if(serviceName.Equals("sticker"))
                    {
                        ColumAssign += $"'Sticker Appplication' as ServiceName, sc.sticker_schedule_date as ServiceScheduleDate, " +
                                    $"sc.sticker_team_user_name as VisitedTeamName, " +
                                    $"sc.sticker_team_status as TeamStatus, " +
                                    $"sc.sticker_team_remark as TeamRemark, " +
                                    $"sc.sticker_team_date as TeamVisitedDate";
                        dateWhereCondColumName = $"sc.sticker_team_date";
                        schedateWhereCondColumName = $"sc.sticker_schedule_date";
                        teamWhereCondColumName = $"sc.sticker_team_user_name";
                    }
                    else if(serviceName.Equals("replace"))//TrackerReplace
                    {
                        ColumAssign +=  $"'Tracker/Sticker Replace' as ServiceName, " +
                                    //$"sc.tracker_replace_date as TrackerReplacedDate, " +
                                    $"sc.tracker_replace_team_user_name as VisitedTeamName, " +
                                    $"sc.tracker_replace_team_status as TeamStatus, " +
                                    $"sc.tracker_replace_team_remark as TeamRemark, " +
                                    $"sc.tracker_replace_team_date as TeamVisitedDate";
                        dateWhereCondColumName = $"sc.tracker_replace_team_date";
                        schedateWhereCondColumName = $"sc.tracker_replace_date";
                        teamWhereCondColumName = $"sc.tracker_replace_team_user_name";
                    }
                    else if(serviceName.Equals("4"))
                    {
                        ColumAssign += $"'4th Day PCR' as ServiceName, sc.4day_pcr_test_date as ServiceScheduleDate, " +
                                    $"sc.4day_pcr_team_user_name as VisitedTeamName, " +
                                    $"sc.4day_pcr_team_status as TeamStatus, " +
                                    $"sc.4day_pcr_team_remark as TeamRemark, " +
                                    $"sc.4day_pcr_team_date as TeamVisitedDate";
                        dateWhereCondColumName = $"sc.4day_pcr_team_date";
                        schedateWhereCondColumName = $"sc.4day_pcr_test_date";
                        teamWhereCondColumName = $"sc.4day_pcr_team_user_name";
                    }
                    else if(serviceName.Equals("6"))
                    {
                        ColumAssign += $"'6th Day PCR' as ServiceName, sc.6day_pcr_test_date as ServiceScheduleDate, " +
                                    $"sc.6day_pcr_team_user_name as VisitedTeamName, " +
                                    $"sc.6day_pcr_team_status as TeamStatus, " +
                                    $"sc.6day_pcr_team_remark as TeamRemark, " +
                                    $"sc.6day_pcr_team_date as TeamVisitedDate";
                        dateWhereCondColumName = $"sc.6day_pcr_team_date";
                        schedateWhereCondColumName = $"sc.6day_pcr_test_date";
                        teamWhereCondColumName = $"sc.6day_pcr_team_user_name";
                    }
                    else if(serviceName.Equals("8"))
                    {
                        ColumAssign += $"'8th Day PCR' as ServiceName, sc.8day_pcr_test_date as ServiceScheduleDate, " +
                                    $"sc.8day_pcr_team_user_name as VisitedTeamName, " +
                                    $"sc.8day_pcr_team_status as TeamStatus, " +
                                    $"sc.8day_pcr_team_remark as TeamRemark, " +
                                    $"sc.8day_pcr_team_date as TeamVisitedDate";
                        dateWhereCondColumName = $"sc.8day_pcr_team_date";
                        schedateWhereCondColumName = $"sc.8day_pcr_test_date";
                        teamWhereCondColumName = $"sc.8day_pcr_team_user_name";
                    }
                    else if(serviceName.Equals("9"))
                    {
                        ColumAssign += $"'9th Day PCR' as ServiceName, sc.9day_pcr_test_date as ServiceScheduleDate, " +
                                    $"sc.9day_pcr_team_user_name as VisitedTeamName, " +
                                    $"sc.9day_pcr_team_status as TeamStatus, " +
                                    $"sc.9day_pcr_team_remark as TeamRemark, " +
                                    $"sc.9day_pcr_team_date as TeamVisitedDate";
                        dateWhereCondColumName = $"sc.9day_pcr_team_date";
                        schedateWhereCondColumName = $"sc.9day_pcr_test_date";
                        teamWhereCondColumName = $"sc.9day_pcr_team_user_name";
                    }
                    else if(serviceName.Equals("11"))
                    {
                        ColumAssign += $"'11th Day PCR' as ServiceName, sc.11day_pcr_test_date as ServiceScheduleDate, " +
                                    $"sc.11day_pcr_team_user_name as VisitedTeamName, " +
                                    $"sc.11day_pcr_team_status as TeamStatus, " +
                                    $"sc.11day_pcr_team_remark as TeamRemark, " +
                                    $"sc.11day_pcr_team_date as TeamVisitedDate";
                        dateWhereCondColumName = $"sc.11day_pcr_team_date";
                        schedateWhereCondColumName = $"sc.11day_pcr_test_date";
                        teamWhereCondColumName = $"sc.11day_pcr_team_user_name";
                    }
                    else if(serviceName.Equals("discharge"))
                    {
                        ColumAssign += $"'Discharge' as ServiceName, sc.discharge_date as ServiceScheduleDate, " +
                                    $"sc.discharge_team_user_name as VisitedTeamName, " +
                                    $"sc.discharge_team_status as TeamStatus, " +
                                    $"sc.discharge_team_remark as TeamRemark, " +
                                    $"sc.discharge_team_date as TeamVisitedDate";
                        dateWhereCondColumName = $"sc.discharge_team_date";
                        schedateWhereCondColumName = $"sc.discharge_date";
                        teamWhereCondColumName = $"sc.discharge_team_user_name";
                    }

                    if(scheduledToDate == "0001-01-01")
                    {
                        whereCond += $" and (" + dateWhereCondColumName + " = '" + scheduledFromDate + minits + "'" +
                                     $" or " + schedateWhereCondColumName + " = '" + scheduledFromDate + minits + "')";
                    }
                    else
                    {
                        whereCond += $" and ((" +  dateWhereCondColumName + " between '" + scheduledFromDate + minits + "' and '" + scheduledToDate + minits + "')" +
                                     $" or (" +  schedateWhereCondColumName + " between '" + scheduledFromDate + minits + "' and '" + scheduledToDate + minits + "'))";
                    }

                    if (!string.IsNullOrEmpty(companyId))
                        whereCond += $" and pa.company_id = '" + companyId + "'";

                if(!string.IsNullOrEmpty(teamUserName))
                    whereCond += " and ((sc.allocated_team_name = '" + teamUserName + "' and sc.reallocated_team_name = '')" +
                                 " or (sc.allocated_team_name != '' and sc.reallocated_team_name = '" + teamUserName + "')" +
                                 " or " + teamWhereCondColumName + " = '" + teamUserName + "')";

                    var orderCond = $" order by pa.area ASC";

                    var sqlSelQuery = $"select " + ColumAssign + " from " + tableName + whereCond + orderCond;
                    using (var connection = _appDbContext.Connection)
                    {
                        var sqlSelResult = await connection.QueryAsync<TeamReportDetails>(sqlSelQuery);
                        retTeamReportDetailsList = sqlSelResult.ToList();
                    }
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retTeamReportDetailsList;
        }
    }
}