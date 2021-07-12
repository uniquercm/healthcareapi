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
    internal sealed class ScheduledRepository : IScheduledRepository
    {
        private readonly AppDbContext _appDbContext;
        public ScheduledRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // public async Task<List<ScheduledDetails>> GetScheduledDetails(string companyId, string scheduledId, string patientId, bool isFieldAllocation, IPatientRepository patientRepository, DateTime scheduledFromDate, DateTime scheduledToDate, string searchAllowTeamType, string serviceName, string serviceStatus)
        // {
        //     List<ScheduledDetails> retScheduledDetailsList = new List<ScheduledDetails>();
        //     try
        //     {
        //         var tableName = $"HC_Staff_Patient.patient_obj p, " +
        //                         $"HC_Treatment.scheduled_obj sc";

        //         var ColumAssign = $"sc.scheduled_id as ScheduledId, sc.patient_staff_id as PatientStaffId, " +
        //                           $"sc.patient_id as PatientId, p.patient_name as PatientName, p.age as Age, " +
        //                           $"sc.initial_pcr_test_date as PCRTestDate, sc.initial_pcr_test_result as PCRResult, " +
        //                           $"sc.have_vaccine as HaveVaccine, " +

        //                           $"sc.allocated_team_name as AllocatedTeamName, sc.reallocated_team_name as ReAllocatedTeamName, " +

        //                           $"sc.treatment_type as TreatmentType, " +

        //                           $"sc.treatment_from_date as TreatmentFromDate, sc.treatment_to_date as TreatmentToDate, " +

        //                           $"sc.discharge_date as DischargeDate, sc.discharge_status as DischargeStatus, " +
        //                           $"sc.discharge_remarks as DischargeRemarks, " +

        //                           $"sc.tracker_schedule_date as TrackerScheduleDate, sc.tracker_applied_date as TrackerAppliedDate, " +
        //                           $"sc.sticker_schedule_date as StickerScheduleDate, sc.sticker_removed_date as StickerRemovedDate, " +
        //                           $"sc.tracker_replace_date as TrackerReplacedDate, sc.tracker_replace_no as TrackerReplaceNumber, " +
        //                           $"sc.sticker_tracker_no as StickerTrackerNumber, sc.sticker_tracker_result as StickerTrackerResult, " +
                                  
        //                           $"sc.4day_pcr_test_date as PCR4DayTestDate, sc.4day_pcr_test_sample_date as PCR4DaySampleDate, sc.4day_pcr_test_result as PCR4DayResult, " +
        //                           $"sc.8day_pcr_test_date as PCR8DayTestDate, sc.8day_pcr_test_sample_date as PCR8DaySampleDate, sc.8day_pcr_test_result as PCR8DayResult, " +

        //                           $"sc.2day_call_id as Day2CallId, sc.3day_call_id as Day3CallId, " +
        //                           $"sc.4day_call_id as Day4CallId, sc.5day_call_id as Day5CallId, " +
        //                           $"sc.6day_call_id as Day6CallId, sc.7day_call_id as Day7CallId, " +
        //                           $"sc.9day_call_id as Day9CallId, " +

        //                           $"sc.have_treatement_extract as ExtractTreatementDate, " +
        //                           $"sc.status as Status, " +
        //                           $"sc.created_by as CreatedBy, sc.modified_by as ModifiedBy";

        //         var whereCond = $" where sc.patient_id = p.patient_id" +
        //                         $" and sc.status = 'Active'" +
        //                         $" and p.status = 'Active'";

        //         if (!string.IsNullOrEmpty(companyId))
        //             whereCond += " and p.company_id = '" + companyId + "'";

        //         if (!string.IsNullOrEmpty(scheduledId))
        //             whereCond += " and sc.scheduled_id = '" + scheduledId + "'";

        //         if (!string.IsNullOrEmpty(patientId))
        //             whereCond += " and sc.patient_id = '" + patientId + "'";
                
        //         if(searchAllowTeamType.ToLower().Equals("allowed"))
        //             whereCond += " and ((sc.allocated_team_name != null or sc.reallocated_team_name != null) " + 
        //                          " or (sc.allocated_team_name != '' or sc.reallocated_team_name != '')) ";
        //         else if(searchAllowTeamType.ToLower().Equals("notallowed"))
        //             whereCond += " and ((sc.allocated_team_name = null and sc.reallocated_team_name = null) " + 
        //                          " or (sc.allocated_team_name = '' and sc.reallocated_team_name = '')) ";

        //         string fromDate = scheduledFromDate.Date.ToString("yyyy-MM-dd");
        //         /*if(fromDate == "0001-01-01")
        //         {
        //             scheduledFromDate = DateTime.Today;
        //             //scheduledFromDate = DateTime.Today.AddDays(1);
        //             fromDate = scheduledFromDate.ToString("yyyy-MM-dd 00:00:00.0");
        //         }*/
        //         string toDate = scheduledToDate.Date.ToString("yyyy-MM-dd");
        //         if(fromDate != "0001-01-01" || toDate != "0001-01-01")
        //         {
        //             if(fromDate == "0001-01-01")
        //                 fromDate = toDate;
                    
        //             if(toDate == "0001-01-01")
        //                 toDate = fromDate;

        //             //if(fromDate != "0001-01-01")
        //                 //whereCond += $" and sc.treatment_from_date <= '" + fromDate + "'" +
        //                              //$" and sc.treatment_to_date > '" + toDate + "'";
        //         }

        //         string timeMin = " 00:00:00.0";
        //         if(!String.IsNullOrEmpty(serviceName) && fromDate != "0001-01-01")
        //         {//all, tracker, sticker, 4pcr, 8pcr, discharge
        //             if(serviceName.Equals("tracker"))
        //                 whereCond += $" and sc.tracker_schedule_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "'";
        //             else if(serviceName.Equals("sticker"))
        //                 whereCond += $" and sc.sticker_schedule_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "'";
        //             else if(serviceName.Equals("4pcr"))
        //                 whereCond += $" and sc.4day_pcr_test_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "'";
        //             else if(serviceName.Equals("8pcr"))
        //                 whereCond += $" and sc.8day_pcr_test_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "'";
        //             else if(serviceName.Equals("discharge"))
        //                 whereCond += $" and sc.discharge_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "'";
        //             else
        //                 whereCond += $" and ((sc.treatment_from_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "')" +
        //                              $" or (sc.treatment_to_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "'))";
        //                 /*whereCond += $" and sc.treatment_from_date >= '" + fromDate + timeMin + "'" +
        //                              $" and sc.treatment_to_date <= '" + toDate + timeMin + "'";*/
        //         }
        //         else if(fromDate != "0001-01-01")
        //             whereCond += $" and ((sc.treatment_from_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "')" +
        //                              $" or (sc.treatment_to_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "'))";
        //             /*whereCond += $" and sc.treatment_from_date >= '" + fromDate + timeMin + "'" +
        //                            $" and sc.treatment_to_date <= '" + toDate + timeMin + "'";*/

        //         if(!String.IsNullOrEmpty(serviceStatus)  && serviceStatus != "all")
        //         {//all, tracker, sticker, 4pcr, 8pcr, discharge
        //             if(serviceName.Equals("tracker") || serviceName.Equals("sticker"))
        //                 whereCond += " and sc.sticker_tracker_result = '" + serviceStatus + "'";
        //             else if(serviceName.Equals("4pcr"))
        //                 whereCond += " and sc.4day_pcr_test_result = '" + serviceStatus + "'";
        //             else if(serviceName.Equals("8pcr"))
        //                 whereCond += " and sc.8day_pcr_test_result = '" + serviceStatus + "'";
        //             else if(serviceName.Equals("discharge"))
        //                 whereCond += " and p.discharge_status = '" + serviceStatus + "'";
        //         }

        //         var orderCond = $" order by sc.created_on DESC ";

        //         var sqlSelQuery = $"select " + ColumAssign + " from " + tableName + whereCond + orderCond;
        //         using (var connection = _appDbContext.Connection)
        //         {
        //             var sqlSelResult = await connection.QueryAsync<ScheduledDetails>(sqlSelQuery);
        //             retScheduledDetailsList = sqlSelResult.ToList();
        //             foreach(ScheduledDetails singleScheduledDetails in retScheduledDetailsList)
        //             {
        //                 List<CallDetails> callDetailsList = await GetCallDetails(singleScheduledDetails.Day2CallId, singleScheduledDetails.ScheduledId);
        //                 if(callDetailsList.Count > 0)
        //                     singleScheduledDetails.Day2CallDetails = callDetailsList[0];

        //                 callDetailsList = await GetCallDetails(singleScheduledDetails.Day3CallId, singleScheduledDetails.ScheduledId);
        //                 if(callDetailsList.Count > 0)
        //                     singleScheduledDetails.Day3CallDetails = callDetailsList[0];

        //                 callDetailsList = await GetCallDetails(singleScheduledDetails.Day4CallId, singleScheduledDetails.ScheduledId);
        //                 if(callDetailsList.Count > 0)
        //                     singleScheduledDetails.Day4CallDetails = callDetailsList[0];

        //                 callDetailsList = await GetCallDetails(singleScheduledDetails.Day5CallId, singleScheduledDetails.ScheduledId);
        //                 if(callDetailsList.Count > 0)
        //                     singleScheduledDetails.Day5CallDetails = callDetailsList[0];

        //                 callDetailsList = await GetCallDetails(singleScheduledDetails.Day6CallId, singleScheduledDetails.ScheduledId);
        //                 if(callDetailsList.Count > 0)
        //                     singleScheduledDetails.Day6CallDetails = callDetailsList[0];

        //                 callDetailsList = await GetCallDetails(singleScheduledDetails.Day7CallId, singleScheduledDetails.ScheduledId);
        //                 if(callDetailsList.Count > 0)
        //                     singleScheduledDetails.Day7CallDetails = callDetailsList[0];

        //                 callDetailsList = await GetCallDetails(singleScheduledDetails.Day9CallId, singleScheduledDetails.ScheduledId);
        //                 if(callDetailsList.Count > 0)
        //                     singleScheduledDetails.Day9CallDetails = callDetailsList[0];

        //                 singleScheduledDetails.PatientInformation = new PatientDetails();
        //                 if(isFieldAllocation)
        //                 {
        //                     List<PatientDetails> patientDetailsList = new List<PatientDetails>();
        //                     patientDetailsList = await patientRepository.GetPatientDetails(companyId, singleScheduledDetails.PatientId, "all");
        //                     if(patientDetailsList.Count > 0)
        //                         singleScheduledDetails.PatientInformation = patientDetailsList[0];
        //                 }
        //             }
        //         }
        //     }
        //     catch (Exception Err)
        //     {
        //         var Error = Err.Message.ToString();
        //     }
        //     return retScheduledDetailsList;
        // }

        public async Task<List<ScheduledDetails>> GetScheduledDetails(string companyId, string scheduledId, string patientId, bool isFieldAllocation, IPatientRepository patientRepository, DateTime scheduledFromDate, DateTime scheduledToDate, string searchAllowTeamType, string serviceName, string serviceStatus, bool isTeam)
        {
            List<ScheduledDetails> retScheduledDetailsList = new List<ScheduledDetails>();
            try
            {
                var tableName = $"HC_Staff_Patient.patient_obj p, " +
                                $"HC_Treatment.scheduled_obj sc, " +
                                $"HC_Master_Details.request_crm_obj rc";

                var ColumAssign = $"sc.scheduled_id as ScheduledId, sc.patient_staff_id as PatientStaffId, " +
                                  $"sc.patient_id as PatientId, p.patient_name as PatientName, p.age as Age, " +
                                  $"p.request_id as RequestId, rc.request_crm_name as RequestCrmName, " +
                                  $"sc.initial_pcr_test_date as PCRTestDate, sc.initial_pcr_test_result as PCRResult, " +
                                  $"sc.have_vaccine as HaveVaccine, " +

                                  $"sc.allocated_team_name as AllocatedTeamName, sc.reallocated_team_name as ReAllocatedTeamName, " +
                                  $"sc.team_allocated_date as AllocatedDate, sc.team_reallocated_date as ReAllocatedDate, " +

                                  $"sc.treatment_type as TreatmentType, " +

                                  $"sc.treatment_from_date as TreatmentFromDate, sc.treatment_to_date as TreatmentToDate, " +

                                  $"sc.discharge_date as DischargeDate, sc.discharge_status as DischargeStatus, " +
                                  $"sc.discharge_remarks as DischargeRemarks, " +
                                  $"sc.discharge_team_user_name as DischargeTeamUserName, sc.discharge_team_status as DischargeTeamStatus, " +
                                  $"sc.discharge_team_remark as DischargeTeamRemark, sc.discharge_team_date as DischargeTeamStatusDate, " +


                                  $"sc.tracker_schedule_date as TrackerScheduleDate, sc.tracker_applied_date as TrackerAppliedDate, " +
                                  $"sc.tracker_team_user_name as TrackerTeamUserName, sc.tracker_team_status as TrackerTeamStatus, " +
                                  $"sc.tracker_team_remark as TrackerTeamRemark, sc.tracker_team_date as TrackerTeamStatusDate, " +

                                  $"sc.sticker_schedule_date as StickerScheduleDate, sc.sticker_removed_date as StickerRemovedDate, " +
                                  $"sc.sticker_team_user_name as StickerTeamUserName, sc.sticker_team_status as StickerTeamStatus, " +
                                  $"sc.sticker_team_remark as StickerTeamRemark, sc.sticker_team_date as StickerTeamStatusDate, " +

                                  $"sc.tracker_replace_date as TrackerReplacedDate, sc.tracker_replace_no as TrackerReplaceNumber, " +
                                  $"sc.tracker_replace_team_user_name as TrackerReplaceTeamUserName, sc.tracker_replace_team_status as TrackerReplaceTeamStatus, " +
                                  $"sc.tracker_replace_team_remark as TrackerReplaceTeamRemark, sc.tracker_replace_team_date as TrackerReplaceTeamStatusDate, " +

                                  $"sc.sticker_tracker_no as StickerTrackerNumber, sc.sticker_tracker_result as StickerTrackerResult, " +

                                  $"sc.4day_pcr_test_date as PCR4DayTestDate, sc.4day_pcr_test_sample_date as PCR4DaySampleDate, sc.4day_pcr_test_result as PCR4DayResult, " +
                                  $"sc.4day_pcr_team_user_name as PCR4DayTeamUserName, sc.4day_pcr_team_status as PCR4DayTeamStatus, " +
                                  $"sc.4day_pcr_team_remark as PCR4DayTeamRemark, sc.4day_pcr_team_date as PCR4DayTeamStatusDate, " +

                                  $"sc.6day_pcr_test_date as PCR6DayTestDate, sc.6day_pcr_test_sample_date as PCR6DaySampleDate, sc.6day_pcr_test_result as PCR6DayResult, " +
                                  $"sc.6day_pcr_team_user_name as PCR6DayTeamUserName, sc.6day_pcr_team_status as PCR6DayTeamStatus, " +
                                  $"sc.6day_pcr_team_remark as PCR6DayTeamRemark, sc.6day_pcr_team_date as PCR6DayTeamStatusDate, " +

                                  $"sc.8day_pcr_test_date as PCR8DayTestDate, sc.8day_pcr_test_sample_date as PCR8DaySampleDate, sc.8day_pcr_test_result as PCR8DayResult, " +
                                  $"sc.8day_pcr_team_user_name as PCR8DayTeamUserName, sc.8day_pcr_team_status as PCR8DayTeamStatus, " +
                                  $"sc.8day_pcr_team_remark as PCR8DayTeamRemark, sc.8day_pcr_team_date as PCR8DayTeamStatusDate, " +

                                  $"sc.11day_pcr_test_date as PCR11DayTestDate, sc.11day_pcr_test_sample_date as PCR11DaySampleDate, sc.11day_pcr_test_result as PCR11DayResult, " +
                                  $"sc.11day_pcr_team_user_name as PCR11DayTeamUserName, sc.11day_pcr_team_status as PCR11DayTeamStatus, " +
                                  $"sc.11day_pcr_team_remark as PCR11DayTeamRemark, sc.11day_pcr_team_date as PCR11DayTeamStatusDate, " +


                                  $"sc.2day_call_id as Day2CallId, sc.3day_call_id as Day3CallId, " +
                                  $"sc.4day_call_id as Day4CallId, sc.5day_call_id as Day5CallId, " +
                                  $"sc.6day_call_id as Day6CallId, sc.7day_call_id as Day7CallId, " +
                                  $"sc.9day_call_id as Day9CallId, " +

                                  $"sc.have_treatement_extract as ExtractTreatementDate, " +
                                  $"sc.status as Status, " +
                                  $"sc.created_by as CreatedBy, sc.modified_by as ModifiedBy";

                var whereCond = $" where sc.patient_id = p.patient_id" +
                                $" and sc.status = 'Active'" +
                                $" and p.status = 'Active'"+
                                $" and p.request_id = rc.request_crm_id";

                if (!string.IsNullOrEmpty(companyId))
                    whereCond += " and p.company_id = '" + companyId + "'";

                if (!string.IsNullOrEmpty(scheduledId))
                    whereCond += " and sc.scheduled_id = '" + scheduledId + "'";

                if (!string.IsNullOrEmpty(patientId))
                    whereCond += " and sc.patient_id = '" + patientId + "'";
                
                if(searchAllowTeamType.ToLower().Equals("allowed"))
                    whereCond += " and ((sc.allocated_team_name != null or sc.reallocated_team_name != null) " + 
                                 " or (sc.allocated_team_name != '' or sc.reallocated_team_name != '')) ";
                else if(searchAllowTeamType.ToLower().Equals("notallowed"))
                    whereCond += " and ((sc.allocated_team_name = null and sc.reallocated_team_name = null) " + 
                                 " or (sc.allocated_team_name = '' and sc.reallocated_team_name = '')) ";

                string fromDate = scheduledFromDate.Date.ToString("yyyy-MM-dd");
                string toDate = scheduledToDate.Date.ToString("yyyy-MM-dd");
                if(isFieldAllocation)
                {
                    if (!isTeam)
                    {
                        if(fromDate == "0001-01-01" && toDate == "0001-01-01")
                        {
                            scheduledFromDate = DateTime.Today;
                            fromDate = scheduledFromDate.Date.ToString("yyyy-MM-dd");
                        }
                    }
                }
                if(fromDate != "0001-01-01" || toDate != "0001-01-01")
                {
                    if(fromDate == "0001-01-01")
                        fromDate = toDate;
                    
                    if(toDate == "0001-01-01")
                        toDate = fromDate;
                }

                string timeMin = " 00:00:00.0";
                if(!String.IsNullOrEmpty(serviceName) && fromDate != "0001-01-01")
                {//all, tracker, sticker, 4pcr, 8pcr, discharge
                    if(serviceName.Equals("tracker"))
                        whereCond += $" and sc.tracker_schedule_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "'";
                    else if(serviceName.Equals("sticker"))
                        whereCond += $" and sc.sticker_schedule_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "'";
                    else if(serviceName.Equals("4pcr"))
                        whereCond += $" and sc.4day_pcr_test_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "'";
                    else if(serviceName.Equals("6pcr"))
                        whereCond += $" and sc.6day_pcr_test_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "'";
                    else if(serviceName.Equals("8pcr") || serviceName.Equals("eight"))
                        whereCond += $" and sc.8day_pcr_test_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "'";
                    else if(serviceName.Equals("11pcr"))
                        whereCond += $" and sc.11day_pcr_test_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "'";
                    else if(serviceName.Equals("discharge"))
                        whereCond += $" and sc.discharge_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "'";
                    else
                    {
                        //whereCond += $" and (sc.treatment_from_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "')" +
                                     //$" and (sc.treatment_to_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "')";

                        whereCond += $" and (sc.tracker_schedule_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "'";
                        //whereCond += $" or sc.4day_pcr_test_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "'";
                        whereCond += $" or sc.6day_pcr_test_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "'";
                        whereCond += $" or sc.8day_pcr_test_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "'";
                        whereCond += $" or sc.11day_pcr_test_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "'";
                        whereCond += $" or sc.sticker_schedule_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "'";
                        whereCond += $" or sc.discharge_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "')";

                        /*whereCond += $" and sc.treatment_from_date >= '" + fromDate + timeMin + "'" +
                                     $" and sc.treatment_to_date <= '" + toDate + timeMin + "'";*/
                    }
                }
                else if(fromDate != "0001-01-01")
                {
                    //whereCond += $" and (sc.treatment_from_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "')" +
                                     //$" and (sc.treatment_to_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "')";

                    whereCond += $" and (sc.tracker_schedule_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "'";
                    //whereCond += $" or sc.4day_pcr_test_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "'";
                    whereCond += $" or sc.6day_pcr_test_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "'";
                    whereCond += $" or sc.8day_pcr_test_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "'";
                    whereCond += $" or sc.11day_pcr_test_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "'";
                    whereCond += $" or sc.sticker_schedule_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "'";
                    whereCond += $" or sc.discharge_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "')";

                    /*whereCond += $" and sc.treatment_from_date >= '" + fromDate + timeMin + "'" +
                                   $" and sc.treatment_to_date <= '" + toDate + timeMin + "'";*/
                }

                if(!String.IsNullOrEmpty(serviceStatus)  && serviceStatus != "all")
                {//all, tracker, sticker, 4pcr, 8pcr, discharge
                    if(serviceName.Equals("tracker") || serviceName.Equals("sticker"))
                        whereCond += " and sc.sticker_tracker_result = '" + serviceStatus + "'";
                    else if(serviceName.Equals("4pcr"))
                        whereCond += " and sc.4day_pcr_test_result = '" + serviceStatus + "'";
                    else if(serviceName.Equals("6pcr"))
                        whereCond += " and sc.6day_pcr_test_result = '" + serviceStatus + "'";
                    else if(serviceName.Equals("8pcr") || serviceName.Equals("eight"))
                        whereCond += " and sc.8day_pcr_test_result = '" + serviceStatus + "'";
                    else if(serviceName.Equals("11pcr"))
                        whereCond += " and sc.11day_pcr_test_result = '" + serviceStatus + "'";
                    else if(serviceName.Equals("discharge"))
                        whereCond += " and p.discharge_status = '" + serviceStatus + "'";
                }

                var orderCond = $" order by sc.created_on DESC ";

                var sqlSelQuery = $"select " + ColumAssign + " from " + tableName + whereCond + orderCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelResult = await connection.QueryAsync<ScheduledDetails>(sqlSelQuery);
                    retScheduledDetailsList = sqlSelResult.ToList();
                    foreach(ScheduledDetails singleScheduledDetails in retScheduledDetailsList)
                    {
                        List<CallDetails> callDetailsList = await GetCallDetails(singleScheduledDetails.Day2CallId, singleScheduledDetails.ScheduledId);
                        if(callDetailsList.Count > 0)
                            singleScheduledDetails.Day2CallDetails = callDetailsList[0];

                        callDetailsList = await GetCallDetails(singleScheduledDetails.Day3CallId, singleScheduledDetails.ScheduledId);
                        if(callDetailsList.Count > 0)
                            singleScheduledDetails.Day3CallDetails = callDetailsList[0];

                        callDetailsList = await GetCallDetails(singleScheduledDetails.Day4CallId, singleScheduledDetails.ScheduledId);
                        if(callDetailsList.Count > 0)
                            singleScheduledDetails.Day4CallDetails = callDetailsList[0];

                        callDetailsList = await GetCallDetails(singleScheduledDetails.Day5CallId, singleScheduledDetails.ScheduledId);
                        if(callDetailsList.Count > 0)
                            singleScheduledDetails.Day5CallDetails = callDetailsList[0];

                        callDetailsList = await GetCallDetails(singleScheduledDetails.Day6CallId, singleScheduledDetails.ScheduledId);
                        if(callDetailsList.Count > 0)
                            singleScheduledDetails.Day6CallDetails = callDetailsList[0];

                        callDetailsList = await GetCallDetails(singleScheduledDetails.Day7CallId, singleScheduledDetails.ScheduledId);
                        if(callDetailsList.Count > 0)
                            singleScheduledDetails.Day7CallDetails = callDetailsList[0];

                        callDetailsList = await GetCallDetails(singleScheduledDetails.Day9CallId, singleScheduledDetails.ScheduledId);
                        if(callDetailsList.Count > 0)
                            singleScheduledDetails.Day9CallDetails = callDetailsList[0];

                        singleScheduledDetails.PatientInformation = new PatientDetails();
                        if(isFieldAllocation)
                        {
                            List<PatientDetails> patientDetailsList = new List<PatientDetails>();
                            patientDetailsList = await patientRepository.GetPatientDetails(companyId, singleScheduledDetails.PatientId, "all", new DateTime(01,01,01), new DateTime(01,01,01), "all");
                            if(patientDetailsList.Count > 0)
                                singleScheduledDetails.PatientInformation = patientDetailsList[0];
                        }
                    }
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retScheduledDetailsList;
        }

/*
        public async Task<List<ScheduledDetails>> GetScheduledDetails(string companyId, string scheduledId, string patientStaffId, bool isFieldAllocation, IPatientRepository patientRepository)
        {
            List<ScheduledDetails> retScheduledDetailsList = new List<ScheduledDetails>();
            try
            {
                var tableName = $"HC_Staff_Patient.patient_obj p, " +
                                $"HC_Staff_Patient.staff_obj s, " +
                                $"HC_Treatment.scheduled_obj sc, " +
                                $"HC_Treatment.call_obj c, " +
                                $"HC_Staff_Patient.patient_staff_xw psx";

                var ColumAssign = $"sc.scheduled_id as ScheduledId, sc.patient_staff_id as PatientStaffId, " +
                                  $"psx.patient_id as PatientId, p.patient_name as PatientName," +
                                  $"psx.staff_id as StaffId, s.staff_name as StaffName," +
                                  $"sc.initial_pcr_test_date as PCRTestDate, sc.initial_pcr_test_result as PCRResult, " +
                                  $"sc.have_vaccine as HaveVaccine, " +
                                  $"sc.allocated_team_name as AllocatedTeamName, sc.reallocated_team_name as ReAllocatedTeamName, " +
                                  $"sc.discharge_date as DischargeDate, sc.treatment_type as TreatmentType, " +
                                  $"sc.treatment_from_date as TreatmentFromDate, sc.treatment_to_date as TreatmentToDate, " +
                                  $"sc.4day_pcr_test_date as PCR4DayTestDate, sc.4day_pcr_test_sample_date as PCR4DaySampleDate, sc.4day_pcr_test_result as PCR4DayResult, " +
                                  $"sc.8day_pcr_test_date as PCR8DayTestDate, sc.8day_pcr_test_sample_date as PCR8DaySampleDate, sc.8day_pcr_test_result as PCR8DayResult, " +
                                  $"sc.2day_call_id as Day2CallId, sc.3day_call_id as Day3CallId, sc.5day_call_id as Day5CallId, " +
                                  $"sc.6day_call_id as Day6CallId, sc.7day_call_id as Day7CallId, sc.9day_call_id as Day9CallId, " +
                                  $"sc.created_by as CreatedBy, sc.modified_by as ModifiedBy";

                var whereCond = $" where p.company_id = s.company_id"+
                                $" and sc.patient_staff_id = psx.patient_staff_id" +
                                $" and psx.patient_id = p.patient_id" +
                                $" and psx.staff_id = s.staff_id";
                                //$"";

                if (!string.IsNullOrEmpty(companyId))
                    whereCond += " and p.company_id = '" + companyId + "'";

                if (!string.IsNullOrEmpty(scheduledId))
                    whereCond += " and sc.scheduled_id = '" + scheduledId + "'";

                if (!string.IsNullOrEmpty(patientStaffId))
                    whereCond += " and sc.patient_staff_id = '" + patientStaffId + "'";

                var sqlSelQuery = $"select " + ColumAssign + " from " + tableName + whereCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelResult = await connection.QueryAsync<ScheduledDetails>(sqlSelQuery);
                    retScheduledDetailsList = sqlSelResult.ToList();
                    foreach(ScheduledDetails singleScheduledDetails in retScheduledDetailsList)
                    {
                        List<CallDetails> callDetailsList = await GetCallDetails(singleScheduledDetails.Day2CallId, singleScheduledDetails.ScheduledId);
                        if(callDetailsList.Count > 0)
                            singleScheduledDetails.Day2CallDetails = callDetailsList[0];

                        callDetailsList = await GetCallDetails(singleScheduledDetails.Day3CallId, singleScheduledDetails.ScheduledId);
                        if(callDetailsList.Count > 0)
                            singleScheduledDetails.Day3CallDetails = callDetailsList[0];

                        callDetailsList = await GetCallDetails(singleScheduledDetails.Day5CallId, singleScheduledDetails.ScheduledId);
                        if(callDetailsList.Count > 0)
                            singleScheduledDetails.Day5CallDetails = callDetailsList[0];

                        callDetailsList = await GetCallDetails(singleScheduledDetails.Day6CallId, singleScheduledDetails.ScheduledId);
                        if(callDetailsList.Count > 0)
                            singleScheduledDetails.Day6CallDetails = callDetailsList[0];

                        callDetailsList = await GetCallDetails(singleScheduledDetails.Day7CallId, singleScheduledDetails.ScheduledId);
                        if(callDetailsList.Count > 0)
                            singleScheduledDetails.Day7CallDetails = callDetailsList[0];

                        callDetailsList = await GetCallDetails(singleScheduledDetails.Day9CallId, singleScheduledDetails.ScheduledId);
                        if(callDetailsList.Count > 0)
                            singleScheduledDetails.Day9CallDetails = callDetailsList[0];

                        singleScheduledDetails.PatientInformation = new PatientDetails();
                        if(isFieldAllocation)
                        {
                            List<PatientDetails> patientDetailsList = new List<PatientDetails>();
                            patientDetailsList = await patientRepository.GetPatientDetails(companyId, singleScheduledDetails.PatientId);
                            if(patientDetailsList.Count > 0)
                                singleScheduledDetails.PatientInformation = patientDetailsList[0];
                        }
                    }
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retScheduledDetailsList;
        }
*/
        public async Task<List<CallDetails>> GetCallDetails(string callId, string scheduledId)
        {
            List<CallDetails> retCallDetailsList = new List<CallDetails>();
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
                    retCallDetailsList = sqlSelResult.ToList();
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retCallDetailsList;
        }

        public async Task<bool> CreateScheduled(ScheduledRequest scheduledRequest)
        {
            CompanyDetails retCompanyDetails = new CompanyDetails();
            try
            {
                var uuid = GenerateUUID();
                bool sqlResult = true;
                scheduledRequest.ScheduledId = uuid;

                var tableName = $"HC_Treatment.scheduled_obj";

                var colName = $"scheduled_id, patient_staff_id, patient_id, initial_pcr_test_date, initial_pcr_test_result, " +
                              $"have_vaccine, allocated_team_name, reallocated_team_name, " +
                              $"team_allocated_date, team_reallocated_date, " +
                              $"discharge_date, treatment_type, treatment_from_date, treatment_to_date, " +

                              $"tracker_schedule_date, sticker_schedule_date, " +

                              $"4day_pcr_test_date, 4day_pcr_test_result, " +
                              //$"4day_pcr_test_sample_date, " +
                              $"6day_pcr_test_date, 6day_pcr_test_result, " +
                              //$"6day_pcr_test_sample_date, " +
                              $"8day_pcr_test_date, 8day_pcr_test_result, " +
                              //$"8day_pcr_test_sample_date, " +
                              $"11day_pcr_test_date, 11day_pcr_test_result, " +
                              //$"11day_pcr_test_sample_date, " +

                              $"2day_call_id, 3day_call_id, 4day_call_id, " +
                              $"5day_call_id, 6day_call_id, 7day_call_id, " +
                              $"9day_call_id, have_treatement_extract, created_by, created_on";

                var colValueName = $"@ScheduledId, @PatientStaffId, @PatientId, @PCRTestDate, @PCRResult, " +
                                   $"@HaveVaccine, @AllocatedTeamName, @ReAllocatedTeamName, " +
                                   $"@AllocatedDate, @ReAllocatedDate, " +
                                   $"@DischargeDate, @TreatmentType, " +
                                   $"@TreatmentFromDate, @TreatmentToDate, " +
                                   $"@TrackerScheduleDate, @StickerScheduleDate, " +
                                   $"@PCR4DayTestDate, @PCR4DayResult, " + 
                                   //$"@PCR4DaySampleDate, " +
                                   $"@PCR6DayTestDate, @PCR6DayResult, " +
                                   //$"@PCR6DaySampleDate, " +
                                   $"@PCR8DayTestDate, @PCR8DayResult, " +
                                   //$"@PCR8DaySampleDate, " +
                                   $"@PCR11DayTestDate, @PCR11DayResult, " +
                                   //$"@PCR11DaySampleDate, " +
                                   $"@Day2CallId, @Day3CallId, " +
                                   $"@Day4CallId, @Day5CallId, " +
                                   $"@Day6CallId, @Day7CallId, " +
                                   $"@Day9CallId, " +
                                   $"@ExtractTreatementDate, " +
                                   $"@CreatedBy, @CreatedOn";

                var sqlInsQuery = $"INSERT INTO "+ tableName + "( " + colName + " )" +
                                    $"VALUES ( " + colValueName + " )";

                CallRequest callRequest = new CallRequest();
                callRequest.ScheduledId = scheduledRequest.ScheduledId;
                callRequest.EMRDone = "no";
                callRequest.CreatedBy = scheduledRequest.CreatedBy;

                callRequest.CallScheduledDate = scheduledRequest.TreatmentFromDate.AddDays(1);
                if(await CreateCall(callRequest))
                    scheduledRequest.Day2CallId = callRequest.CallId;
                else
                    scheduledRequest.Day2CallId = "";

                if(scheduledRequest.RequestId == 1)//HQP
                {
                    if(scheduledRequest.HaveVaccine.Equals("yes"))
                    {
                        scheduledRequest.PCR6DayTestDate = scheduledRequest.TreatmentFromDate.AddDays(5);
                        scheduledRequest.DischargeDate = scheduledRequest.TreatmentFromDate.AddDays(6);
                    }
                    else
                    {
                        scheduledRequest.PCR11DayTestDate = scheduledRequest.TreatmentFromDate.AddDays(10);
                        scheduledRequest.DischargeDate = scheduledRequest.TreatmentFromDate.AddDays(11);
                    }

                    scheduledRequest.Day3CallId = "";
                    scheduledRequest.Day4CallId = "";
                    scheduledRequest.Day5CallId = "";
                    scheduledRequest.Day6CallId = "";
                    scheduledRequest.Day7CallId = "";
                    scheduledRequest.Day9CallId = "";

                    scheduledRequest.TrackerScheduleDate = scheduledRequest.TreatmentFromDate;
                }
                else if(scheduledRequest.RequestId == 2)//HIP
                {
                    if(!String.IsNullOrEmpty(scheduledRequest.TreatmentType) && 
                        scheduledRequest.TreatmentType.ToLower() == "isolation")
                    {
                        callRequest.CallScheduledDate = scheduledRequest.TreatmentFromDate.AddDays(2);
                        if(await CreateCall(callRequest))
                            scheduledRequest.Day3CallId = callRequest.CallId;
                        else
                            scheduledRequest.Day3CallId = "";

                        callRequest.CallScheduledDate = scheduledRequest.TreatmentFromDate.AddDays(3);
                        if(await CreateCall(callRequest))
                            scheduledRequest.Day4CallId = callRequest.CallId;
                        else
                            scheduledRequest.Day4CallId = "";

                        callRequest.CallScheduledDate = scheduledRequest.TreatmentFromDate.AddDays(4);
                        if(await CreateCall(callRequest))
                            scheduledRequest.Day5CallId = callRequest.CallId;
                        else
                            scheduledRequest.Day5CallId = "";

                        callRequest.CallScheduledDate = scheduledRequest.TreatmentFromDate.AddDays(5);
                        if(await CreateCall(callRequest))
                            scheduledRequest.Day6CallId = callRequest.CallId;
                        else
                            scheduledRequest.Day6CallId = "";

                        callRequest.CallScheduledDate = scheduledRequest.TreatmentFromDate.AddDays(6);
                        if(await CreateCall(callRequest))
                            scheduledRequest.Day7CallId = callRequest.CallId;
                        else
                            scheduledRequest.Day7CallId = "";

                        callRequest.CallScheduledDate = scheduledRequest.TreatmentFromDate.AddDays(8);
                        if(await CreateCall(callRequest))
                            scheduledRequest.Day9CallId = callRequest.CallId;
                        else
                            scheduledRequest.Day9CallId = "";

                        scheduledRequest.DischargeDate = scheduledRequest.TreatmentToDate;
                    }
                    else
                    {
                        scheduledRequest.Day3CallId = "";
                        scheduledRequest.Day4CallId = "";
                        scheduledRequest.Day5CallId = "";
                        scheduledRequest.Day6CallId = "";
                        scheduledRequest.Day7CallId = "";
                        scheduledRequest.Day9CallId = "";

                        scheduledRequest.DischargeDate = scheduledRequest.TreatmentFromDate.AddDays(3);
                    }

                    scheduledRequest.PCR4DayTestDate = scheduledRequest.TreatmentFromDate.AddDays(3);
                    scheduledRequest.PCR8DayTestDate = scheduledRequest.TreatmentFromDate.AddDays(7);

                    scheduledRequest.TrackerScheduleDate = scheduledRequest.TreatmentFromDate.AddDays(1);
                }
                else
                    return false;

                scheduledRequest.StickerScheduleDate = scheduledRequest.DischargeDate;

                string pcrTestDate;
                if(scheduledRequest.PCRTestDate == null)
                    pcrTestDate = "";
                else
                {
                    pcrTestDate = scheduledRequest.PCRTestDate.ToString("yyyy-MM-dd");
                    if( pcrTestDate == "0001-01-01")
                        pcrTestDate = "";
                    else
                        pcrTestDate = scheduledRequest.PCRTestDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                string dischargeDate;
                if(scheduledRequest.DischargeDate == null)
                    dischargeDate = "";
                else
                {
                    dischargeDate = scheduledRequest.DischargeDate.ToString("yyyy-MM-dd");
                    if( dischargeDate == "0001-01-01")
                        dischargeDate = "";
                    else
                        dischargeDate = scheduledRequest.DischargeDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                string treatmentFromDate;
                if(scheduledRequest.TreatmentFromDate == null)
                    treatmentFromDate = "";
                else
                {
                    treatmentFromDate = scheduledRequest.TreatmentFromDate.ToString("yyyy-MM-dd");
                    if( treatmentFromDate == "0001-01-01")
                        treatmentFromDate = "";
                    else
                        treatmentFromDate = scheduledRequest.TreatmentFromDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                string treatmentToDate;
                if(scheduledRequest.TreatmentToDate == null)
                    treatmentToDate = "";
                else
                {
                    treatmentToDate = scheduledRequest.TreatmentToDate.ToString("yyyy-MM-dd");
                    if( treatmentToDate == "0001-01-01")
                        treatmentToDate = "";
                    else
                        treatmentToDate = scheduledRequest.TreatmentToDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                string trackerDate;
                if(scheduledRequest.TrackerScheduleDate == null)
                    trackerDate = "";
                else
                {
                    trackerDate = scheduledRequest.TrackerScheduleDate.ToString("yyyy-MM-dd");
                    if( trackerDate == "0001-01-01")
                        trackerDate = "";
                    else
                        trackerDate = scheduledRequest.TrackerScheduleDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                string stickerDate;
                if(scheduledRequest.StickerScheduleDate == null)
                    stickerDate = "";
                else
                {
                    stickerDate = scheduledRequest.StickerScheduleDate.ToString("yyyy-MM-dd");
                    if( stickerDate == "0001-01-01")
                        stickerDate = "";
                    else
                        stickerDate = scheduledRequest.StickerScheduleDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                string pcr4DayTestDate;
                if(scheduledRequest.PCR4DayTestDate == null)
                    pcr4DayTestDate = "";
                else
                {
                    pcr4DayTestDate = scheduledRequest.PCR4DayTestDate.ToString("yyyy-MM-dd");
                    if( pcr4DayTestDate == "0001-01-01")
                        pcr4DayTestDate = "";
                    else
                        pcr4DayTestDate = scheduledRequest.PCR4DayTestDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                /*string pcr4DaySampleDate;
                if(scheduledRequest.PCR4DaySampleDate == null)
                    pcr4DaySampleDate = "";
                else
                {
                    pcr4DaySampleDate = scheduledRequest.PCR4DaySampleDate.ToString("yyyy-MM-dd");
                    if( pcr4DaySampleDate == "0001-01-01")
                        pcr4DaySampleDate = "";
                    else
                        pcr4DaySampleDate = scheduledRequest.PCR4DaySampleDate.ToString("yyyy-MM-dd 00:00:00.0");
                }*/

                string pcr6DayTestDate;
                if(scheduledRequest.PCR6DayTestDate == null)
                    pcr6DayTestDate = "";
                else
                {
                    pcr6DayTestDate = scheduledRequest.PCR6DayTestDate.ToString("yyyy-MM-dd");
                    if( pcr6DayTestDate == "0001-01-01")
                        pcr6DayTestDate = "";
                    else
                        pcr6DayTestDate = scheduledRequest.PCR6DayTestDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                /*string pcr6DaySampleDate;
                if(scheduledRequest.PCR6DaySampleDate == null)
                    pcr6DaySampleDate = "";
                else
                {
                    pcr6DaySampleDate = scheduledRequest.PCR6DaySampleDate.ToString("yyyy-MM-dd");
                    if( pcr6DaySampleDate == "0001-01-01")
                        pcr6DaySampleDate = "";
                    else
                        pcr6DaySampleDate = scheduledRequest.PCR6DaySampleDate.ToString("yyyy-MM-dd 00:00:00.0");
                }*/

                string pcr8DayTestDate;
                if(scheduledRequest.PCR8DayTestDate == null)
                    pcr8DayTestDate = "";
                else
                {
                    pcr8DayTestDate = scheduledRequest.PCR8DayTestDate.ToString("yyyy-MM-dd");
                    if( pcr8DayTestDate == "0001-01-01")
                        pcr8DayTestDate = "";
                    else
                        pcr8DayTestDate = scheduledRequest.PCR8DayTestDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                /*string pcr8DaySampleDate;
                if(scheduledRequest.PCR8DaySampleDate == null)
                    pcr8DaySampleDate = "";
                else
                {
                    pcr8DaySampleDate = scheduledRequest.PCR8DaySampleDate.ToString("yyyy-MM-dd");
                    if( pcr8DaySampleDate == "0001-01-01")
                        pcr8DaySampleDate = "";
                    else
                        pcr8DaySampleDate = scheduledRequest.PCR8DaySampleDate.ToString("yyyy-MM-dd 00:00:00.0");
                }*/

                string pcr11DayTestDate;
                if(scheduledRequest.PCR11DayTestDate == null)
                    pcr11DayTestDate = "";
                else
                {
                    pcr11DayTestDate = scheduledRequest.PCR11DayTestDate.ToString("yyyy-MM-dd");
                    if( pcr11DayTestDate == "0001-01-01")
                        pcr11DayTestDate = "";
                    else
                        pcr11DayTestDate = scheduledRequest.PCR11DayTestDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                /*string pcr11DaySampleDate;
                if(scheduledRequest.PCR11DaySampleDate == null)
                    pcr11DaySampleDate = "";
                else
                {
                    pcr11DaySampleDate = scheduledRequest.PCR11DaySampleDate.ToString("yyyy-MM-dd");
                    if( pcr11DaySampleDate == "0001-01-01")
                        pcr11DaySampleDate = "";
                    else
                        pcr11DaySampleDate = scheduledRequest.PCR11DaySampleDate.ToString("yyyy-MM-dd 00:00:00.0");
                }*/

                string allocatedDate;
                if(scheduledRequest.AllocatedDate == null)
                    allocatedDate = "";
                else
                {
                    allocatedDate = scheduledRequest.AllocatedDate.ToString("yyyy-MM-dd");
                    if( allocatedDate == "0001-01-01")
                        allocatedDate = "";
                    else
                        allocatedDate = scheduledRequest.AllocatedDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                string reAllocatedDate;
                if(scheduledRequest.ReAllocatedDate == null)
                    reAllocatedDate = "";
                else
                {
                    reAllocatedDate = scheduledRequest.ReAllocatedDate.ToString("yyyy-MM-dd");
                    if( reAllocatedDate == "0001-01-01")
                        reAllocatedDate = "";
                    else
                        reAllocatedDate = scheduledRequest.ReAllocatedDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                object colValueParam = new
                {
                    ScheduledId = scheduledRequest.ScheduledId,
                    PatientStaffId = scheduledRequest.PatientStaffId,
                    PatientId = scheduledRequest.PatientId,
                    PCRTestDate = pcrTestDate,//scheduledRequest.PCRTestDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    PCRResult = scheduledRequest.PCRResult,
                    HaveVaccine = scheduledRequest.HaveVaccine,
                    AllocatedTeamName = scheduledRequest.AllocatedTeamName,
                    AllocatedDate = allocatedDate,
                    ReAllocatedTeamName = scheduledRequest.ReAllocatedTeamName,
                    ReAllocatedDate = reAllocatedDate,
                    DischargeDate = dischargeDate,//scheduledRequest.DischargeDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    TrackerScheduleDate = trackerDate,//scheduledRequest.TrackerScheduleDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    StickerScheduleDate = stickerDate,//scheduledRequest.StickerScheduleDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    TreatmentType = scheduledRequest.TreatmentType,
                    TreatmentFromDate = treatmentFromDate,//scheduledRequest.TreatmentFromDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    TreatmentToDate = treatmentToDate,//scheduledRequest.TreatmentToDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    PCR4DayTestDate = pcr4DayTestDate,//scheduledRequest.PCR4DayTestDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    //PCR4DaySampleDate = pcr4DaySampleDate,//scheduledRequest.PCR4DaySampleDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    PCR4DayResult = scheduledRequest.PCR4DayResult,
                    PCR6DayTestDate = pcr6DayTestDate,//scheduledRequest.PCR6DayTestDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    //PCR6DaySampleDate = pcr6DaySampleDate,//scheduledRequest.PCR6DaySampleDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    PCR6DayResult = scheduledRequest.PCR6DayResult,
                    PCR8DayTestDate = pcr8DayTestDate,//scheduledRequest.PCR8DayTestDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    //PCR8DaySampleDate = pcr8DaySampleDate,//scheduledRequest.PCR8DaySampleDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    PCR8DayResult = scheduledRequest.PCR8DayResult,
                    PCR11DayTestDate = pcr11DayTestDate,//scheduledRequest.PCR11DayTestDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    //PCR11DaySampleDate = pcr11DaySampleDate,//scheduledRequest.PCR11DaySampleDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    PCR11DayResult = scheduledRequest.PCR11DayResult,
                    Day2CallId = scheduledRequest.Day2CallId,
                    Day3CallId = scheduledRequest.Day3CallId,
                    Day4CallId = scheduledRequest.Day4CallId,
                    Day5CallId = scheduledRequest.Day5CallId,
                    Day6CallId = scheduledRequest.Day6CallId,
                    Day7CallId = scheduledRequest.Day7CallId,
                    Day9CallId = scheduledRequest.Day9CallId,
                    ExtractTreatementDate = "no",
                    CreatedBy = scheduledRequest.CreatedBy,
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
        public async Task<bool> CreateCall(CallRequest callRequest)
        {
            try
            {
                var uuid = GenerateUUID();
                bool sqlResult = true;
                callRequest.CallId = uuid;

                var tableName = $"HC_Treatment.call_obj";

                var colName = $"call_id, scheduled_id, call_scheduled_date, " +//called_date, " +
                              $"call_status, remarks, emr_done, is_pcr, created_by, created_on";

                var colValueName = $"@CallId, @ScheduledId, @CallScheduledDate, " +//@CalledDate, " +
                                   $"@CallStatus, @Remarks, @EMRDone, @IsPCRCall, " +
                                   $"@CreatedBy, @CreatedOn";

                var sqlInsQuery = $"INSERT INTO "+ tableName + "( " + colName + " )" +
                                    $"VALUES ( " + colValueName + " )";

                string callScheduledDate = callRequest.CallScheduledDate.ToString("yyyy-MM-dd");
                if( callScheduledDate == "0001-01-01")
                    callScheduledDate = "";
                else
                    callScheduledDate = callRequest.CallScheduledDate.ToString("yyyy-MM-dd 00:00:00.0");

                string calledDate = callRequest.CalledDate.ToString("yyyy-MM-dd");
                if( calledDate == "0001-01-01")
                    calledDate = "";
                else
                    callScheduledDate = callRequest.CalledDate.ToString("yyyy-MM-dd 00:00:00.0");

                object colValueParam = new
                {
                    CallId = callRequest.CallId,
                    ScheduledId = callRequest.ScheduledId,
                    CallScheduledDate = callScheduledDate,//callRequest.CallScheduledDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    //CalledDate = calledDate,//callRequest.CalledDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    CallStatus = callRequest.CallStatus,
                    Remarks = callRequest.Remarks,
                    EMRDone = callRequest.EMRDone,
                    IsPCRCall = callRequest.IsPCRCall,
                    CreatedBy = callRequest.CreatedBy,
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

        public async Task<bool> EditScheduled(ScheduledRequest scheduledRequest)
        {
            try
            {
                bool sqlResult = true;
                var tableName = $"HC_Treatment.scheduled_obj";

                var colName = $"scheduled_id = @ScheduledId, patient_staff_id = @PatientStaffId, " +
                              $"patient_id = @PatientId, " +
                              $"initial_pcr_test_date = @PCRTestDate, initial_pcr_test_result = @PCRResult, " +
                              $"have_vaccine = @HaveVaccine, " +
                              //$"allocated_team_name = @AllocatedTeamName, reallocated_team_name = @ReAllocatedTeamName, " +
                              $"discharge_date = @DischargeDate, treatment_type = @TreatmentType, " +
                              $"treatment_from_date = @TreatmentFromDate, treatment_to_date = @TreatmentToDate, " +

                              $"tracker_schedule_date = @TrackerScheduleDate, sticker_schedule_date = @StickerScheduleDate, " +

                              $"4day_pcr_test_date = @PCR4DayTestDate, " +
                              //$"4day_pcr_test_sample_date = @PCR4DaySampleDate, 4day_pcr_test_result = @PCR4DayResult, " +
                              $"6day_pcr_test_date = @PCR6DayTestDate, " +
                              //$"6day_pcr_test_sample_date = @PCR6DaySampleDate, 6day_pcr_test_result = @PCR6DayResult, " +
                              $"8day_pcr_test_date = @PCR8DayTestDate, " +
                              //$"8day_pcr_test_sample_date = @PCR8DaySampleDate, 8day_pcr_test_result = @PCR8DayResult, " +
                              $"11day_pcr_test_date = @PCR11DayTestDate, " +
                              //$"11day_pcr_test_sample_date = @PCR11DaySampleDate, 11day_pcr_test_result = @PCR11DayResult, " +

                              $"modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                var whereCond = $" where scheduled_id = @ScheduledId";
                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                CallRequest callRequest = new CallRequest();
                callRequest.ModifiedBy = scheduledRequest.ModifiedBy;

                callRequest.CallId = scheduledRequest.Day2CallId;
                callRequest.CallScheduledDate = scheduledRequest.TreatmentFromDate.AddDays(1);
                await EditCall(callRequest);

                if(scheduledRequest.RequestId == 1)//HQP
                {
                    if(scheduledRequest.HaveVaccine.Equals("yes"))
                    {
                        scheduledRequest.PCR6DayTestDate = scheduledRequest.TreatmentFromDate.AddDays(5);
                        scheduledRequest.DischargeDate = scheduledRequest.TreatmentFromDate.AddDays(6);
                    }
                    else
                    {
                        scheduledRequest.PCR11DayTestDate = scheduledRequest.TreatmentFromDate.AddDays(10);
                        scheduledRequest.DischargeDate = scheduledRequest.TreatmentFromDate.AddDays(11);
                    }

                    scheduledRequest.Day3CallId = "";
                    scheduledRequest.Day4CallId = "";
                    scheduledRequest.Day5CallId = "";
                    scheduledRequest.Day6CallId = "";
                    scheduledRequest.Day7CallId = "";
                    scheduledRequest.Day9CallId = "";

                    scheduledRequest.TrackerScheduleDate = scheduledRequest.TreatmentFromDate;
                }
                else if(scheduledRequest.RequestId == 2)//HIP
                {
                    if(!String.IsNullOrEmpty(scheduledRequest.TreatmentType) && 
                    scheduledRequest.TreatmentType.ToLower() == "isolation")
                    {
                        callRequest.CallId = scheduledRequest.Day3CallId;
                        callRequest.CallScheduledDate = scheduledRequest.TreatmentFromDate.AddDays(2);
                        await EditCall(callRequest);

                        callRequest.CallId = scheduledRequest.Day4CallId;
                        callRequest.CallScheduledDate = scheduledRequest.TreatmentFromDate.AddDays(3);
                        await EditCall(callRequest);

                        callRequest.CallId = scheduledRequest.Day5CallId;
                        callRequest.CallScheduledDate = scheduledRequest.TreatmentFromDate.AddDays(4);
                        await EditCall(callRequest);

                        callRequest.CallId = scheduledRequest.Day6CallId;
                        callRequest.CallScheduledDate = scheduledRequest.TreatmentFromDate.AddDays(5);
                        await EditCall(callRequest);

                        callRequest.CallId = scheduledRequest.Day7CallId;
                        callRequest.CallScheduledDate = scheduledRequest.TreatmentFromDate.AddDays(6);
                        await EditCall(callRequest);

                        callRequest.CallId = scheduledRequest.Day9CallId;
                        callRequest.CallScheduledDate = scheduledRequest.TreatmentFromDate.AddDays(8);
                        await EditCall(callRequest);

                        scheduledRequest.DischargeDate = scheduledRequest.TreatmentToDate;
                    }
                    else
                    {
                        scheduledRequest.Day3CallId = "";
                        scheduledRequest.Day4CallId = "";
                        scheduledRequest.Day5CallId = "";
                        scheduledRequest.Day6CallId = "";
                        scheduledRequest.Day7CallId = "";
                        scheduledRequest.Day9CallId = "";

                        scheduledRequest.DischargeDate = scheduledRequest.TreatmentFromDate.AddDays(3);
                    }

                    scheduledRequest.PCR4DayTestDate = scheduledRequest.TreatmentFromDate.AddDays(3);
                    scheduledRequest.PCR8DayTestDate = scheduledRequest.TreatmentFromDate.AddDays(7);

                    scheduledRequest.TrackerScheduleDate = scheduledRequest.TreatmentFromDate.AddDays(1);
                }
                else
                    return false;

                scheduledRequest.StickerScheduleDate = scheduledRequest.DischargeDate;

                string pcrTestDate;
                if(scheduledRequest.PCRTestDate == null)
                    pcrTestDate = "";
                else
                {
                    pcrTestDate = scheduledRequest.PCRTestDate.ToString("yyyy-MM-dd");
                    if( pcrTestDate == "0001-01-01")
                        pcrTestDate = "";
                    else
                        pcrTestDate = scheduledRequest.PCRTestDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                string dischargeDate;
                if(scheduledRequest.DischargeDate == null)
                    dischargeDate = "";
                else
                {
                    dischargeDate = scheduledRequest.DischargeDate.ToString("yyyy-MM-dd");
                    if( dischargeDate == "0001-01-01")
                        dischargeDate = "";
                    else
                        dischargeDate = scheduledRequest.DischargeDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                string treatmentFromDate;
                if(scheduledRequest.TreatmentFromDate == null)
                    treatmentFromDate = "";
                else
                {
                    treatmentFromDate = scheduledRequest.TreatmentFromDate.ToString("yyyy-MM-dd");
                    if( treatmentFromDate == "0001-01-01")
                        treatmentFromDate = "";
                    else
                        treatmentFromDate = scheduledRequest.TreatmentFromDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                string treatmentToDate;
                if(scheduledRequest.TreatmentToDate == null)
                    treatmentToDate = "";
                else
                {
                    treatmentToDate = scheduledRequest.TreatmentToDate.ToString("yyyy-MM-dd");
                    if( treatmentToDate == "0001-01-01")
                        treatmentToDate = "";
                    else
                        treatmentToDate = scheduledRequest.TreatmentToDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                string trackerDate;
                if(scheduledRequest.TrackerScheduleDate == null)
                    trackerDate = "";
                else
                {
                    trackerDate = scheduledRequest.TrackerScheduleDate.ToString("yyyy-MM-dd");
                    if( trackerDate == "0001-01-01")
                        trackerDate = "";
                    else
                        trackerDate = scheduledRequest.TrackerScheduleDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                string stickerDate;
                if(scheduledRequest.StickerScheduleDate == null)
                    stickerDate = "";
                else
                {
                    stickerDate = scheduledRequest.StickerScheduleDate.ToString("yyyy-MM-dd");
                    if( stickerDate == "0001-01-01")
                        stickerDate = "";
                    else
                        stickerDate = scheduledRequest.StickerScheduleDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                string pcr4DayTestDate;
                if(scheduledRequest.PCR4DayTestDate == null)
                    pcr4DayTestDate = "";
                else
                {
                    pcr4DayTestDate = scheduledRequest.PCR4DayTestDate.ToString("yyyy-MM-dd");
                    if( pcr4DayTestDate == "0001-01-01")
                        pcr4DayTestDate = "";
                    else
                        pcr4DayTestDate = scheduledRequest.PCR4DayTestDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                string pcr6DayTestDate;
                if(scheduledRequest.PCR6DayTestDate == null)
                    pcr6DayTestDate = "";
                else
                {
                    pcr6DayTestDate = scheduledRequest.PCR6DayTestDate.ToString("yyyy-MM-dd");
                    if( pcr6DayTestDate == "0001-01-01")
                        pcr6DayTestDate = "";
                    else
                        pcr6DayTestDate = scheduledRequest.PCR6DayTestDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                string pcr8DayTestDate;
                if(scheduledRequest.PCR8DayTestDate == null)
                    pcr8DayTestDate = "";
                else
                {
                    pcr8DayTestDate = scheduledRequest.PCR8DayTestDate.ToString("yyyy-MM-dd");
                    if( pcr8DayTestDate == "0001-01-01")
                        pcr8DayTestDate = "";
                    else
                        pcr8DayTestDate = scheduledRequest.PCR8DayTestDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                string pcr11DayTestDate;
                if(scheduledRequest.PCR11DayTestDate == null)
                    pcr11DayTestDate = "";
                else
                {
                    pcr11DayTestDate = scheduledRequest.PCR11DayTestDate.ToString("yyyy-MM-dd");
                    if( pcr11DayTestDate == "0001-01-01")
                        pcr11DayTestDate = "";
                    else
                        pcr11DayTestDate = scheduledRequest.PCR11DayTestDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                object colValueParam = new
                {
                    ScheduledId = scheduledRequest.ScheduledId,
                    PatientStaffId = scheduledRequest.PatientStaffId,
                    PatientId = scheduledRequest.PatientId,
                    PCRTestDate = pcrTestDate,//scheduledRequest.PCRTestDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    PCRResult = scheduledRequest.PCRResult,
                    HaveVaccine = scheduledRequest.HaveVaccine,
                    //AllocatedTeamName = scheduledRequest.AllocatedTeamName,
                    //ReAllocatedTeamName = scheduledRequest.ReAllocatedTeamName,
                    DischargeDate = dischargeDate,//scheduledRequest.DischargeDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    TrackerScheduleDate = trackerDate,//scheduledRequest.TrackerScheduleDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    StickerScheduleDate = stickerDate,//scheduledRequest.StickerScheduleDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    TreatmentType = scheduledRequest.TreatmentType,
                    TreatmentFromDate = treatmentFromDate,//scheduledRequest.TreatmentFromDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    TreatmentToDate = treatmentToDate,//scheduledRequest.TreatmentToDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    PCR4DayTestDate = pcr4DayTestDate,//scheduledRequest.PCR4DayTestDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    //PCR4DaySampleDate = pcr4DaySampleDate,//scheduledRequest.PCR4DaySampleDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    //PCR4DayResult = scheduledRequest.PCR4DayResult,
                    PCR6DayTestDate = pcr6DayTestDate,//scheduledRequest.PCR6DayTestDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    //PCR6DaySampleDate = pcr6DaySampleDate,//scheduledRequest.PCR6DaySampleDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    //PCR6DayResult = scheduledRequest.PCR6DayResult,
                    PCR8DayTestDate = pcr8DayTestDate,//scheduledRequest.PCR8DayTestDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    //PCR8DaySampleDate = pcr8DaySampleDate,//scheduledRequest.PCR8DaySampleDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    //PCR8DayResult = scheduledRequest.PCR8DayResult,
                    PCR11DayTestDate = pcr11DayTestDate,//scheduledRequest.PCR11DayTestDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    //PCR11DaySampleDate = pcr11DaySampleDate,//scheduledRequest.PCR11DaySampleDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    //PCR11DayResult = scheduledRequest.PCR11DayResult,
                    ModifiedBy = scheduledRequest.ModifiedBy,
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
        public async Task<bool> EditCall(CallRequest callRequest)
        {
            try
            {
                bool sqlResult = true;
                var tableName = $"HC_Treatment.call_obj";

                var colName = $"call_id = @CallId, " +
                              //$"scheduled_id = @ScheduledId, " +
                              $"call_scheduled_date = @CallScheduledDate, " +
                              $"called_date = @CalledDate, call_status = @CallStatus, " +
                              $"remarks = @Remarks, emr_done = @EMRDone, is_pcr = @IsPCRCall," +
                              $"modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                var whereCond = $" where call_id = @CallId";
                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                string callScheduledDate;
                if(callRequest.CallScheduledDate == null)
                    callScheduledDate = "";
                else
                {
                    callScheduledDate = callRequest.CallScheduledDate.ToString("yyyy-MM-dd");
                    if( callScheduledDate == "0001-01-01")
                        callScheduledDate = "";
                    else
                        callScheduledDate = callRequest.CallScheduledDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

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
                    CallId = callRequest.CallId,
                    ScheduledId = callRequest.ScheduledId,
                    CallScheduledDate = callScheduledDate,//callRequest.CallScheduledDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    CalledDate = calledDate,//callRequest.CalledDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    CallStatus = callRequest.CallStatus,
                    Remarks = callRequest.Remarks,
                    EMRDone = callRequest.EMRDone,
                    IsPCRCall = callRequest.IsPCRCall,
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

        public async Task<bool> EditFieldAllocation(FieldAllocationRequest fieldAllocationRequest)
        {
            try
            {
                bool sqlResult = true;
                foreach(FieldAllocationDetails singleFieldAllocationDetails in fieldAllocationRequest.FieldAllocationDetailsList)
                {
                    var tableName = $"HC_Treatment.scheduled_obj";

                    var colName = $"scheduled_id = @ScheduledId, " +
                                $"allocated_team_name = @AllocatedTeamName, reallocated_team_name = @ReAllocatedTeamName, " +
                                $"team_allocated_date = @AllocatedDate, team_reallocated_date = @ReAllocatedDate, " +
                                $"modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                    var whereCond = $" where scheduled_id = @ScheduledId";
                    var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                    string allocatedDate;
                    /*if(singleFieldAllocationDetails.AllocatedDate == null)
                        allocatedDate = "";
                    else
                    {
                        allocatedDate = singleFieldAllocationDetails.AllocatedDate.ToString("yyyy-MM-dd");
                        if( allocatedDate == "0001-01-01")
                            allocatedDate = "";
                        else
                            allocatedDate = singleFieldAllocationDetails.AllocatedDate.ToString("yyyy-MM-dd 00:00:00.0");
                    }*/
                    if(String.IsNullOrEmpty(singleFieldAllocationDetails.AllocatedTeamName))
                        allocatedDate = "";
                    else
                        allocatedDate = DateTime.Today.ToString("yyyy-MM-dd 00:00:00.0");

                    string reAllocatedDate;
                    /*if(singleFieldAllocationDetails.ReAllocatedDate == null)
                        reAllocatedDate = "";
                    else
                    {
                        reAllocatedDate = singleFieldAllocationDetails.ReAllocatedDate.ToString("yyyy-MM-dd");
                        if( reAllocatedDate == "0001-01-01")
                            reAllocatedDate = "";
                        else
                            reAllocatedDate = singleFieldAllocationDetails.ReAllocatedDate.ToString("yyyy-MM-dd 00:00:00.0");
                    }*/
                    if(String.IsNullOrEmpty(singleFieldAllocationDetails.ReAllocatedTeamName))
                        reAllocatedDate = "";
                    else
                        reAllocatedDate = DateTime.Today.ToString("yyyy-MM-dd 00:00:00.0");

                    object colValueParam = new
                    {
                        ScheduledId = singleFieldAllocationDetails.ScheduledId,
                        AllocatedTeamName = singleFieldAllocationDetails.AllocatedTeamName,
                        ReAllocatedTeamName = singleFieldAllocationDetails.ReAllocatedTeamName,
                        AllocatedDate = allocatedDate,
                        ReAllocatedDate = reAllocatedDate,
                        ModifiedBy = singleFieldAllocationDetails.ModifiedBy,
                        ModifiedOn = DateTime.Today.ToString("yyyy-MM-dd 00:00:00.0")
                    };
                    using (var connection = _appDbContext.Connection)
                    {
                        sqlResult = Convert.ToBoolean(await connection.ExecuteAsync(sqlUpdateQuery, colValueParam));
                    }
                }
                return sqlResult;
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
                return false;
            }
        }


        /*public async Task<bool> DeleteSchedule(DeleteRequest request)
        {
            try
            {
                var tableName = $"HC_Treatment.scheduled_obj";
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
                    var sqlResult = await connection.ExecuteAsync(sqlUpdateQuery, colValueParam);
                    return true;
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
                return false;
            }
        }*/
    }
}