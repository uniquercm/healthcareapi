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
    internal sealed class PatientStaffRepository : IPatientStaffRepository
    {
        private readonly AppDbContext _appDbContext;
        public PatientStaffRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<PatientStaffDetails>> GetPatientStaffDetails(string patientStaffId, string patientId, string staffId)
        {
            List<PatientStaffDetails> retPatientStaffDetailsList = new List<PatientStaffDetails>();
            try
            {
                var tableName = $"HC_Staff_Patient.patient_staff_xw psx, " +
                                $"HC_Staff_Patient.patient_obj p, HC_Staff_Patient.staff_obj s";

                var ColumAssign = $"psx.patient_staff_id as PatientStaffId, " +
                                  $"psx.staff_id as StaffId, s.staff_name as StaffName, " +
                                  $"psx.patient_id as PatientId, p.patient_name as PatientName, " +
                                  $"psx.no_of_adults as AdultsCount, psx.no_of_childrens as ChildrensCount, " +
                                  $"psx.scheduled_from_date as ScheduledFromDate, " +
                                  $"psx.scheduled_to_date as ScheduledToDate, " +
                                  $"psx.called_date as CalledDate, psx.call_status as CallStatus, " +
                                  $"psx.remarks as Remarks, psx.emr_done as EMRDone, " +
                                  $"psx.visit_status as VisitStatus, psx.visited_date as VisitedDate, " + 
                                  $"psx.reallocated_team_name as ReallocatedTeamName, " +
                                  $"psx.created_by as CreatedBy, psx.modified_by as ModifiedBy";

                var whereCond = " where psx.patient_id = p.patient_id and psx.staff_id = s.staff_id";

                if (!string.IsNullOrEmpty(patientStaffId))
                    whereCond += " and psx.patient_staff_id = '" + patientStaffId + "'";

                if (!string.IsNullOrEmpty(patientId))
                    whereCond += " and psx.patient_id = '" + patientId + "'";

                if (!string.IsNullOrEmpty(staffId))
                    whereCond += " and psx.staff_id = '" + staffId + "'";

                var sqlSelQuery = $"select " + ColumAssign + " from " + tableName + whereCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelResult = await connection.QueryAsync<PatientStaffDetails>(sqlSelQuery);
                    retPatientStaffDetailsList = sqlSelResult.ToList();
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retPatientStaffDetailsList;
        }

        public async Task<bool> CreatePatientStaff(PatientStaffRequest patientStaffRequest)
        {
            try
            {
                var uuid = GenerateUUID();
                bool sqlResult = true;
                patientStaffRequest.PatientStaffId = uuid;

                var tableName = $"HC_Staff_Patient.patient_staff_xw";

                var colName = $"patient_staff_id, staff_id, patient_id, no_of_adults, " +
                              $"no_of_childrens, scheduled_from_date, scheduled_to_date, " +
                              $"called_date, call_status, remarks, emr_done, " +
                              $"visit_status, visited_date, reallocated_team_name, " +
                              $"created_by, created_on";

                var colValueName = $"@PatientStaffId, @StaffId, @PatientId, @AdultsCount, " +
                                   $"@ChildrensCount, @ScheduledFromDate, @ScheduledToDate, " +
                                   $"@CalledDate, @CallStatus, @Remarks, @EMRDone, " +
                                   $"@VisitStatus, @VisitedDate, @ReallocatedTeamName, " +
                                   $"@CreatedBy, @CreatedOn";

                var sqlInsQuery = $"INSERT INTO "+ tableName + "( " + colName + " )" +
                                    $"VALUES ( " + colValueName + " )";

                object colValueParam = new
                {
                    PatientStaffId = patientStaffRequest.PatientStaffId,
                    StaffId = patientStaffRequest.StaffId,
                    PatientId = patientStaffRequest.PatientId,
                    AdultsCount = patientStaffRequest.AdultsCount,
                    ChildrensCount = patientStaffRequest.ChildrensCount,
                    ScheduledFromDate = patientStaffRequest.ScheduledFromDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    ScheduledToDate = patientStaffRequest.ScheduledToDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    CalledDate = patientStaffRequest.CalledDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    CallStatus = patientStaffRequest.CallStatus,
                    Remarks = patientStaffRequest.Remarks,
                    EMRDone = patientStaffRequest.EMRDone,
                    VisitStatus = patientStaffRequest.VisitStatus,
                    VisitedDate = patientStaffRequest.VisitedDate.ToString("yyyy-MM-dd 00:00:00.0"),
                    ReallocatedTeamName = patientStaffRequest.ReallocatedTeamName,
                    CreatedBy = patientStaffRequest.CreatedBy,
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
        public async Task<bool> EditPatientStaff(PatientStaffRequest patientStaffRequest)
        {
            try
            {
                bool sqlResult = true;
                var tableName = $"HC_Staff_Patient.patient_staff_xw";

                var colName = $"patient_staff_id = @PatientStaffId, staff_id = @StaffId, " +
                              $"patient_id = @PatientId, no_of_adults = @AdultsCount, " +
                              $"no_of_childrens = @ChildrensCount, scheduled_from_date = @ScheduledFromDate, " +
                              $"scheduled_to_date = @ScheduledToDate, " +
                              $"called_date = @CalledDate, call_status = @CallStatus, " +
                              $"remarks = @Remarks, emr_done = @EMRDone, " +
                              $"visit_status = @VisitStatus, visited_date = @VisitedDate, " + 
                              $"reallocated_team_name = @ReallocatedTeamName, " +
                              $"modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                var whereCond = $" where patient_staff_id = @PatientStaffId";
                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                object colValueParam = new
                {
                    PatientStaffId = patientStaffRequest.PatientStaffId,
                    StaffId = patientStaffRequest.StaffId,
                    PatientId = patientStaffRequest.PatientId,
                    AdultsCount = patientStaffRequest.AdultsCount,
                    ChildrensCount = patientStaffRequest.ChildrensCount,
                    ScheduledFromDate = patientStaffRequest.ScheduledFromDate,
                    ScheduledToDate = patientStaffRequest.ScheduledToDate,
                    CalledDate = patientStaffRequest.CalledDate,
                    CallStatus = patientStaffRequest.CallStatus,
                    Remarks = patientStaffRequest.Remarks,
                    EMRDone = patientStaffRequest.EMRDone,
                    VisitStatus = patientStaffRequest.VisitStatus,
                    VisitedDate = patientStaffRequest.VisitedDate,
                    ReallocatedTeamName = patientStaffRequest.ReallocatedTeamName,
                    ModifiedBy = patientStaffRequest.ModifiedBy,
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