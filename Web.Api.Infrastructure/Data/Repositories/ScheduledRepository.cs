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
        public async Task<List<ScheduledDetails>> GetScheduledDetails(string companyId, string scheduledId, string patientId, bool isFieldAllocation, IPatientRepository patientRepository)
        {
            List<ScheduledDetails> retScheduledDetailsList = new List<ScheduledDetails>();
            try
            {
                var tableName = $"HC_Staff_Patient.patient_obj p, " +
                                $"HC_Treatment.scheduled_obj sc";

                var ColumAssign = $"sc.scheduled_id as ScheduledId, sc.patient_staff_id as PatientStaffId, " +
                                  $"sc.patient_id as PatientId, p.patient_name as PatientName," +
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

                var whereCond = $" where sc.patient_id = p.patient_id";
                                //$"";

                if (!string.IsNullOrEmpty(companyId))
                    whereCond += " and p.company_id = '" + companyId + "'";

                if (!string.IsNullOrEmpty(scheduledId))
                    whereCond += " and sc.scheduled_id = '" + scheduledId + "'";

                if (!string.IsNullOrEmpty(patientId))
                    whereCond += " and sc.patient_id = '" + patientId + "'";

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
                              $"discharge_date, treatment_type, treatment_from_date, treatment_to_date, " +
                              $"4day_pcr_test_date, 4day_pcr_test_sample_date, 4day_pcr_test_result, " +
                              $"8day_pcr_test_date, 8day_pcr_test_sample_date, 8day_pcr_test_result, " +
                              $"2day_call_id, 3day_call_id, 5day_call_id, 6day_call_id, 7day_call_id, " +
                              $"9day_call_id, created_by, created_on";

                var colValueName = $"@ScheduledId, @PatientStaffId, @PatientId, @PCRTestDate, @PCRResult, " +
                                   $"@HaveVaccine, @AllocatedTeamName, " +
                                   $"@ReAllocatedTeamName, @DischargeDate, @TreatmentType, " +
                                   $"@TreatmentFromDate, @TreatmentToDate, " +
                                   $"@PCR4DayTestDate, @PCR4DaySampleDate, " +
                                   $"@PCR4DayResult, @PCR8DayTestDate, " +
                                   $"@PCR8DaySampleDate, @PCR8DayResult, " +
                                   $"@Day2CallId, @Day3CallId, @Day5CallId , @Day6CallId, " +
                                   $"@Day7CallId, @Day9CallId, " +
                                   $"@CreatedBy, @CreatedOn";

                var sqlInsQuery = $"INSERT INTO "+ tableName + "( " + colName + " )" +
                                    $"VALUES ( " + colValueName + " )";

                CallRequest callRequest = new CallRequest();
                callRequest.ScheduledId = scheduledRequest.ScheduledId;
                callRequest.EMRDone = "No";
                //callRequest.CallStatus = "Pending";
                callRequest.CreatedBy = scheduledRequest.CreatedBy;

                callRequest.CallScheduledDate = scheduledRequest.TreatmentFromDate.AddDays(1);
                if(await CreateCall(callRequest))
                    scheduledRequest.Day2CallId = callRequest.CallId;
                else
                    scheduledRequest.Day2CallId = "";

                callRequest.CallScheduledDate = scheduledRequest.TreatmentFromDate.AddDays(2);
                if(await CreateCall(callRequest))
                    scheduledRequest.Day3CallId = callRequest.CallId;
                else
                    scheduledRequest.Day3CallId = "";

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

                object colValueParam = new
                {
                    ScheduledId = scheduledRequest.ScheduledId,
                    PatientStaffId = scheduledRequest.PatientStaffId,
                    PatientId = scheduledRequest.PatientId,
                    PCRTestDate = scheduledRequest.PCRTestDate,
                    PCRResult = scheduledRequest.PCRResult,
                    HaveVaccine = scheduledRequest.HaveVaccine,
                    AllocatedTeamName = scheduledRequest.AllocatedTeamName,
                    ReAllocatedTeamName = scheduledRequest.ReAllocatedTeamName,
                    DischargeDate = scheduledRequest.DischargeDate,
                    TreatmentType = scheduledRequest.TreatmentType,
                    TreatmentFromDate = scheduledRequest.TreatmentFromDate,
                    TreatmentToDate = scheduledRequest.TreatmentToDate,
                    PCR4DayTestDate = scheduledRequest.PCR4DayTestDate,
                    PCR4DaySampleDate = scheduledRequest.PCR4DaySampleDate,
                    PCR4DayResult = scheduledRequest.PCR4DayResult,
                    PCR8DayTestDate = scheduledRequest.PCR8DayTestDate,
                    PCR8DaySampleDate = scheduledRequest.PCR8DaySampleDate,
                    PCR8DayResult = scheduledRequest.PCR8DayResult,
                    Day2CallId = scheduledRequest.Day2CallId,
                    Day3CallId = scheduledRequest.Day3CallId,
                    Day5CallId = scheduledRequest.Day5CallId,
                    Day6CallId = scheduledRequest.Day6CallId,
                    Day7CallId = scheduledRequest.Day7CallId,
                    Day9CallId = scheduledRequest.Day9CallId,
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

                var colName = $"call_id, scheduled_id, call_scheduled_date, called_date, " +
                              $"call_status, remarks, emr_done, created_by, created_on";

                var colValueName = $"@CallId, @ScheduledId, @CallScheduledDate, @CalledDate, " +
                                   $"@CallStatus, @Remarks, @EMRDone, " +
                                   $"@CreatedBy, @CreatedOn";

                var sqlInsQuery = $"INSERT INTO "+ tableName + "( " + colName + " )" +
                                    $"VALUES ( " + colValueName + " )";

                object colValueParam = new
                {
                    CallId = callRequest.CallId,
                    ScheduledId = callRequest.ScheduledId,
                    CallScheduledDate = callRequest.CallScheduledDate,
                    CalledDate = callRequest.CalledDate,
                    CallStatus = callRequest.CallStatus,
                    Remarks = callRequest.Remarks,
                    EMRDone = callRequest.EMRDone,
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
                              $"4day_pcr_test_date = @PCR4DayTestDate, 4day_pcr_test_sample_date = @PCR4DaySampleDate, 4day_pcr_test_result = @PCR4DayResult, " +
                              $"8day_pcr_test_date = @PCR8DayTestDate, 8day_pcr_test_sample_date = @PCR8DaySampleDate, 8day_pcr_test_result = @PCR8DayResult, " +
                              //$"2day_call_id = @Day2CallId, 3day_call_id = @Day3CallId, 5day_call_id = @Day5CallId, " +
                              //$"6day_call_id = @Day6CallId, 7day_call_id = @Day7CallId, 9day_call_id = @Day9CallId, " +
                              $"modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                var whereCond = $" where scheduled_id = @ScheduledId";
                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                CallRequest callRequest = new CallRequest();
                callRequest.ModifiedBy = scheduledRequest.ModifiedBy;

                callRequest.CallId = scheduledRequest.Day2CallId;
                callRequest.CallScheduledDate = scheduledRequest.TreatmentFromDate.AddDays(1);
                await EditCall(callRequest);

                callRequest.CallId = scheduledRequest.Day3CallId;
                callRequest.CallScheduledDate = scheduledRequest.TreatmentFromDate.AddDays(2);
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

                object colValueParam = new
                {
                    ScheduledId = scheduledRequest.ScheduledId,
                    PatientStaffId = scheduledRequest.PatientStaffId,
                    PatientId = scheduledRequest.PatientId,
                    PCRTestDate = scheduledRequest.PCRTestDate,
                    PCRResult = scheduledRequest.PCRResult,
                    HaveVaccine = scheduledRequest.HaveVaccine,
                    //AllocatedTeamName = scheduledRequest.AllocatedTeamName,
                    //ReAllocatedTeamName = scheduledRequest.ReAllocatedTeamName,
                    DischargeDate = scheduledRequest.DischargeDate,
                    TreatmentType = scheduledRequest.TreatmentType,
                    TreatmentFromDate = scheduledRequest.TreatmentFromDate,
                    TreatmentToDate = scheduledRequest.TreatmentToDate,
                    PCR4DayTestDate = scheduledRequest.PCR4DayTestDate,
                    PCR4DaySampleDate = scheduledRequest.PCR4DaySampleDate,
                    PCR4DayResult = scheduledRequest.PCR4DayResult,
                    PCR8DayTestDate = scheduledRequest.PCR8DayTestDate,
                    PCR8DaySampleDate = scheduledRequest.PCR8DaySampleDate,
                    PCR8DayResult = scheduledRequest.PCR8DayResult,
                    /*Day2CallId = day2CallId,
                    Day3CallId = day3CallId,
                    Day5CallId = day5CallId,
                    Day6CallId = day6CallId,
                    Day7CallId = day7CallId,
                    Day9CallId = day9CallId,*/
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

                var colName = $"call_id = @CallId, scheduled_id = @ScheduledId, " +
                              $"call_scheduled_date = @CallScheduledDate, " +
                              $"called_date = @CalledDate, call_status = @CallStatus, " +
                              $"remarks = @Remarks, emr_done = @EMRDone, " +
                              $"modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                var whereCond = $" where call_id = @CallId";
                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                object colValueParam = new
                {
                    CallId = callRequest.CallId,
                    ScheduledId = callRequest.ScheduledId,
                    CallScheduledDate = callRequest.CallScheduledDate,
                    CalledDate = callRequest.CalledDate,
                    CallStatus = callRequest.CallStatus,
                    Remarks = callRequest.Remarks,
                    EMRDone = callRequest.EMRDone,
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
                var tableName = $"HC_Treatment.scheduled_obj";

                var colName = $"scheduled_id = @ScheduledId, " +
                              //$"patient_staff_id = @PatientStaffId, " +
                              //$"patient_id = @PatientId, " +
                              $"allocated_team_name = @AllocatedTeamName, reallocated_team_name = @ReAllocatedTeamName, " +
                              $"modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                var whereCond = $" where scheduled_id = @ScheduledId";
                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                object colValueParam = new
                {
                    ScheduledId = fieldAllocationRequest.ScheduledId,
                    //PatientStaffId = fieldAllocationRequest.PatientStaffId,
                    //PatientId = fieldAllocationRequest.PatientId,
                    AllocatedTeamName = fieldAllocationRequest.AllocatedTeamName,
                    ReAllocatedTeamName = fieldAllocationRequest.ReAllocatedTeamName,
                    ModifiedBy = fieldAllocationRequest.ModifiedBy,
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