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
    internal sealed class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext _appDbContext;
        public PatientRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<PatientDetails>> GetPatientDetails(string companyId, string patientId)
        {
            List<PatientDetails> retPatientDetailsList = new List<PatientDetails>();
            try
            {
                var tableName = $"HC_Staff_Patient.patient_obj p, HC_Master_Details.company_obj co, " +
                                $"HC_Master_Details.city_obj ci, HC_Master_Details.nationality_obj n";

                var ColumAssign = $"p.patient_id as PatientId, p.patient_name as PatientName, " +
                              $"p.company_id as CompanyId, co.company_name as CompanyName, " +
                              $"p.request_id as RequestId, p.crm_no as CRMNo, p.eid_no as EIDNo, " +
                              $"p.date_of_birth as DateOfBirth, " +
                              $"p.age as Age, p.sex as Sex, p.address as Address, " +
                              $"p.landmark as LandMark, p.area as Area, " +
                              $"p.city_id as CityId, ci.city_name as CityName, " +
                              $"p.nationality_id as NationalityId, n.nationality_name as NationalityName, " +
                              $"p.mobile_no as MobileNo, p.google_map_link as GoogleMapLink, p.sticker_application as StickerApplication, " +
                              $"p.sticker_removal as StickerRemoval, " +
                              $"p.created_by as CreatedBy, p.modified_by as ModifiedBy";

                var whereCond = " where p.company_id = co.company_id"  +
                                $" and p.city_id = ci.city_id" +
                                $" and p.nationality_id = n.nationality_id";

                if (!string.IsNullOrEmpty(companyId))
                    whereCond += " and p.company_id = '" + companyId + "'";

                if (!string.IsNullOrEmpty(patientId))
                    whereCond += " and p.patient_id = '" + patientId + "'";

                var sqlSelQuery = $"select " + ColumAssign + " from " + tableName + whereCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelResult = await connection.QueryAsync<PatientDetails>(sqlSelQuery);
                    foreach(PatientDetails singlePatienDetails in sqlSelResult.ToList())
                    {
                        List<KeyValuePair<string,string>> scheduleDrCallIdList = new List<KeyValuePair<string, string>>();
                        tableName = $"HC_Treatment.scheduled_obj sc";

                        ColumAssign = $"sc.scheduled_id as `Key`, sc.2day_call_id as `Value` " ;

                        whereCond = " where sc.patient_id = '" + singlePatienDetails.PatientId + "'";
                        var sqlQuery = $"select " + ColumAssign + " from " + tableName + whereCond;

                        Dictionary<string, string> data = connection.Query<KeyValuePair<string, string>>(sqlQuery).ToDictionary(pair => pair.Key, pair => pair.Value);
                        scheduleDrCallIdList = data.ToList();
                        if(scheduleDrCallIdList.Count > 0)
                        {
                            singlePatienDetails.ScheduledId = scheduleDrCallIdList[0].Key;
                            singlePatienDetails.DrCallId = scheduleDrCallIdList[0].Value;
                        }
                        else
                        {
                            singlePatienDetails.ScheduledId = "";
                            singlePatienDetails.DrCallId = "";
                        }
                        retPatientDetailsList.Add(singlePatienDetails);
                    }
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retPatientDetailsList;
        }

/*
        public async Task<List<PatientDetails>> GetPatientDetails(string companyId, string patientId)
        {
            List<PatientDetails> retPatientDetailsList = new List<PatientDetails>();
            try
            {
                var tableName = $"HC_Staff_Patient.patient_obj p, HC_Master_Details.company_obj co, " +
                                $"HC_Master_Details.city_obj ci, HC_Master_Details.nationality_obj n";

                var ColumAssign = $"p.patient_id as PatientId, p.patient_name as PatientName, " +
                              $"p.company_id as CompanyId, co.company_name as CompanyName, " +
                              $"p.request_id as RequestId, p.crm_no as CRMNo, p.eid_no as EIDNo, " +
                              $"p.date_of_birth as DateOfBirth, " +
                              $"p.age as Age, p.sex as Sex, p.address as Address, " +
                              $"p.landmark as LandMark, p.area as Area, " +
                              $"p.city_id as CityId, ci.city_name as CityName, " +
                              $"p.nationality_id as NationalityId, n.nationality_name as NationalityName, " +
                              $"p.mobile_no as MobileNo, p.google_map_link as GoogleMapLink, p.sticker_application as StickerApplication, " +
                              $"p.sticker_removal as StickerRemoval, " +
                              $"p.created_by as CreatedBy, p.modified_by as ModifiedBy";

                var whereCond = " where p.company_id = co.company_id"  +
                                $" and p.city_id = ci.city_id" +
                                $" and p.nationality_id = n.nationality_id";

                if (!string.IsNullOrEmpty(companyId))
                    whereCond += " and p.company_id = '" + companyId + "'";

                if (!string.IsNullOrEmpty(patientId))
                    whereCond += " and p.patient_id = '" + patientId + "'";

                var sqlSelQuery = $"select " + ColumAssign + " from " + tableName + whereCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelResult = await connection.QueryAsync<PatientDetails>(sqlSelQuery);
                    foreach(PatientDetails singlePatienDetails in sqlSelResult.ToList())
                    {
                        List<KeyValuePair<string,string>> scheduleDrCallIdList = new List<KeyValuePair<string, string>>();
                        tableName = $"HC_Staff_Patient.patient_staff_xw psx, " +
                                    $"HC_Treatment.scheduled_obj sc";

                        ColumAssign = $"sc.scheduled_id as `Key`, sc.2day_call_id as `Value` " ;

                        whereCond = " where psx.patient_id = '" + singlePatienDetails.PatientId + "'" +
                                    $" and psx.patient_staff_id = sc.patient_staff_id";
                        var sqlQuery = $"select " + ColumAssign + " from " + tableName + whereCond;

                        Dictionary<string, string> data = connection.Query<KeyValuePair<string, string>>(sqlQuery).ToDictionary(pair => pair.Key, pair => pair.Value);
                        scheduleDrCallIdList = data.ToList();
                        if(scheduleDrCallIdList.Count > 0)
                        {
                            singlePatienDetails.ScheduledId = scheduleDrCallIdList[0].Key;
                            singlePatienDetails.DrCallId = scheduleDrCallIdList[0].Value;
                        }
                        else
                        {
                            singlePatienDetails.ScheduledId = "";
                            singlePatienDetails.DrCallId = "";
                        }
                        retPatientDetailsList.Add(singlePatienDetails);
                    }
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retPatientDetailsList;
        }
*/
        public async Task<bool> CreatePatient(PatientRequest patientRequest)
        {
            try
            {
                var uuid = GenerateUUID();
                bool sqlResult = true;
                patientRequest.PatientId = uuid;

                var tableName = $"HC_Staff_Patient.patient_obj";

                var colName = $"patient_id, patient_name, company_id, request_id, crm_no, eid_no, " +
                              $"date_of_birth, age, sex, address, landmark, area, city_id, nationality_id, " +
                              $"mobile_no, google_map_link, sticker_application, sticker_removal, " +
                              $"created_by, created_on";

                var colValueName = $"@PatientId, @PatientName, @CompanyId, @RequestId, @CRMNo, @EIDNo, " +
                                   $"@DateOfBirth, @Age, @Sex, @Address, @LandMark, @Area, @CityId, @NationalityId, " +
                                   $"@MobileNo, @GoogleMapLink, @StickerApplication, @StickerRemoval, " +
                                   $"@CreatedBy, @CreatedOn";

                var sqlInsQuery = $"INSERT INTO "+ tableName + "( " + colName + " )" +
                                    $"VALUES ( " + colValueName + " )";

                object colValueParam = new
                {
                    PatientId = patientRequest.PatientId,
                    PatientName = patientRequest.PatientName,
                    CompanyId = patientRequest.CompanyId,
                    RequestId = patientRequest.RequestId,
                    CRMNo = patientRequest.CRMNo,
                    EIDNo = patientRequest.EIDNo,
                    DateOfBirth = patientRequest.DateOfBirth,
                    Age = patientRequest.Age,
                    Sex = patientRequest.Sex,
                    Address = patientRequest.Address,
                    LandMark = patientRequest.LandMark,
                    Area = patientRequest.Area,
                    CityId = patientRequest.CityId,
                    NationalityId = patientRequest.NationalityId,
                    MobileNo = patientRequest.MobileNo,
                    GoogleMapLink = patientRequest.GoogleMapLink,
                    StickerApplication = patientRequest.StickerApplication,
                    StickerRemoval = patientRequest.StickerRemoval,
                    CreatedBy = patientRequest.CreatedBy,
                    CreatedOn = DateTime.UtcNow
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
        public async Task<bool> EditPatient(PatientRequest patientRequest)
        {
            try
            {
                bool sqlResult = true;
                var tableName = $"HC_Staff_Patient.patient_obj";

                var colName = $"patient_id = @PatientId, patient_name = @PatientName, company_id = @CompanyId, " +
                              $"request_id = @RequestId, crm_no = @CRMNo, eid_no = @EIDNo, " +
                              $"date_of_birth = @DateOfBirth, age = @Age, sex = @Sex, address = @Address, " +
                              $"landmark = @LandMark, area = @Area, city_id = @CityId, nationality_id = @NationalityId, " +
                              $"mobile_no = @MobileNo, google_map_link = @GoogleMapLink, sticker_application = @StickerApplication, " +
                              $"sticker_removal = @StickerRemoval, modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                var whereCond = $" where patient_id = @PatientId";
                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                object colValueParam = new
                {
                    PatientId = patientRequest.PatientId,
                    PatientName = patientRequest.PatientName,
                    CompanyId = patientRequest.CompanyId,
                    RequestId = patientRequest.RequestId,
                    CRMNo = patientRequest.CRMNo,
                    EIDNo = patientRequest.EIDNo,
                    DateOfBirth = patientRequest.DateOfBirth,
                    Age = patientRequest.Age,
                    Sex = patientRequest.Sex,
                    Address = patientRequest.Address,
                    LandMark = patientRequest.LandMark,
                    Area = patientRequest.Area,
                    CityId = patientRequest.CityId,
                    NationalityId = patientRequest.NationalityId,
                    MobileNo = patientRequest.MobileNo,
                    GoogleMapLink = patientRequest.GoogleMapLink,
                    StickerApplication = patientRequest.StickerApplication,
                    StickerRemoval = patientRequest.StickerRemoval,
                    ModifiedBy = patientRequest.ModifiedBy,
                    ModifiedOn = DateTime.UtcNow
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

        public async Task<List<DrNurseCallDetails>> GetDrNurseCallDetails(string companyId, string callName, DateTime scheduledFromDate, DateTime scheduledToDate)
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
                                  $"p.mobile_no as MobileNo";

                var whereCond = $" where p.company_id = co.company_id" +
                                $" and p.patient_id = sc.patient_id" +
                                $" and ca.scheduled_id = sc.scheduled_id";

                string fromDate = scheduledFromDate.Date.ToString("dd-MM-yyyy");
                if(fromDate == "01-01-0001")
                {
                    scheduledFromDate = DateTime.Today;//.AddDays(1);
                    fromDate = scheduledFromDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                //scheduledToDate = DateTime.Today.AddDays(1);
                string toDate = scheduledToDate.Date.ToString("dd-MM-yyyy");
                if(toDate != "01-01-0001")
                {
                    if(fromDate != "01-01-0001")
                        whereCond += $" and ca.call_scheduled_date >= '" + fromDate + "'";

                    toDate = scheduledToDate.ToString("yyyy-MM-dd 00:00:00.0");
                    whereCond += $" and ca.call_scheduled_date <= '" + toDate + "'";

                    if(callName != "DrCall")
                    {
                        whereCond += $" and sc.4day_pcr_test_date >= '" + fromDate + "'" +
                                     $" or sc.8day_pcr_test_date >= '" + fromDate + "'" +
                                     $" and sc.4day_pcr_test_date <= '" + toDate + "'" +
                                     $" or sc.8day_pcr_test_date <= '" + toDate + "'";
                    }
                }
                else
                {
                    if(fromDate != "01-01-0001")
                        whereCond += $" and ca.call_scheduled_date = '" + fromDate + "'";

                    if(callName != "DrCall")
                    {
                        whereCond += $" and sc.4day_pcr_test_date = '" + fromDate + "'" +
                                    $" or sc.8day_pcr_test_date = '" + fromDate + "'";
                    }
                }

                if(callName == "DrCall")
                    whereCond += $" and sc.2day_call_id = ca.call_id";
                else if(callName == "NurseCall")
                    whereCond += $" and sc.2day_call_id <> ca.call_id";

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

    }
}