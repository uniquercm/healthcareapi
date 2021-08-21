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

        public async Task<List<DrNurseCallDetails>> GetTeamReportDetails(string companyId, string teamUserName, DateTime scheduledFromDate, DateTime scheduledToDate, IDrNurseCallFieldAllocationRepository drNurseCallFieldAllocationRepository)
        {
            List<DrNurseCallDetails> retDrNurseCallDetails = new List<DrNurseCallDetails>();
            try
            {
                retDrNurseCallDetails = await drNurseCallFieldAllocationRepository.GetFieldAllowCallDetails(companyId, teamUserName, "all", scheduledFromDate, scheduledToDate, "all", "all", "all", "schedule", "all");
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retDrNurseCallDetails;
        }
    }
}