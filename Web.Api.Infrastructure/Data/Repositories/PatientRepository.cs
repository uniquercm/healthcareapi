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
        public async Task<List<PatientDetails>> GetPatientDetails(string companyId, string patientId, string gMapLinkSatus, DateTime assignedFromDate, DateTime assignedToDate, string searchStatus)
        {
            List<PatientDetails> retPatientDetailsList = new List<PatientDetails>();
            try
            {
                var tableName = $"HC_Staff_Patient.patient_obj p, HC_Master_Details.company_obj co, " +
                                $"HC_Master_Details.city_obj ci, HC_Master_Details.nationality_obj n, " +
                                $"HC_Master_Details.request_crm_obj rc";

                var ColumAssign = $"p.patient_id as PatientId, p.patient_name as PatientName, " +
                              $"p.company_id as CompanyId, co.company_name as CompanyName, " +
                              $"p.request_id as RequestId, rc.request_crm_name as RequestCrmName, " +
                              $"p.crm_no as CRMNo, p.eid_no as EIDNo, " +
                              $"p.date_of_birth as DateOfBirth, " +
                              $"p.age as Age, p.sex as Sex, p.address as Address, " +
                              $"p.assigned_date as AssignedDate, " +
                              $"p.landmark as LandMark, p.area as Area, " +
                              $"p.city_id as CityId, ci.city_name as CityName, " +
                              $"p.nationality_id as NationalityId, n.country_name as NationalityName, " +
                              $"p.mobile_no as MobileNo, p.google_map_link as GoogleMapLink, " +
                              $"p.no_of_adults as AdultsCount, p.no_of_childrens as ChildrensCount, p.pcr_count as PCRCount, " +
                              $"p.enrolled_count as EnrolledCount, p.enrolled_details as EnrolledDetails, " +
                              $"p.sticker_application as StickerApplication, " +
                              $"p.tracker_application as TrackerApplication, " +
                              $"p.sticker_removal as StickerRemoval, " +
                              $"p.tracker_removal as TrackerRemoval, " +
                              $"p.pcr_count as PCRCount, " +
                              $"p.reception_date as RecptionCallDate, p.reception_status as RecptionCallStatus, " +
                              $"p.reception_remarks as RecptionCallRemarks, " +
                              $"p.status as Status, " +
                              $"p.created_by as CreatedBy, p.created_on as CreatedOn, " +
                              $"p.modified_by as ModifiedBy";

                var whereCond = " where p.company_id = co.company_id"  +
                                $" and p.city_id = ci.city_id" +
                                $" and p.nationality_id = n.nationality_id" +
                                $" and p.status = 'Active'" +
                                $" and p.request_id = rc.request_crm_id";

                if (!string.IsNullOrEmpty(companyId))
                    whereCond += " and p.company_id = '" + companyId + "'";

                if (!string.IsNullOrEmpty(patientId))
                    whereCond += " and p.patient_id = '" + patientId + "'";

                if (!string.IsNullOrEmpty(gMapLinkSatus))
                {
                    if(gMapLinkSatus.Equals("no"))
                        whereCond += " and p.google_map_link = ''";
                    else if(gMapLinkSatus.Equals("yes"))
                        whereCond += " and p.google_map_link != ''";
                }

                if (!string.IsNullOrEmpty(searchStatus))
                {
                    if(searchStatus.Equals("completed"))
                        whereCond += " and p.reception_status = 'completed'";
                    else if(searchStatus.Equals("pending"))
                        whereCond += " and p.reception_status = 'pending'";
                }

                string timeMin = " 00:00:00.0";
                string fromDate = assignedFromDate.Date.ToString("yyyy-MM-dd");
                string toDate = assignedToDate.Date.ToString("yyyy-MM-dd");
                /*if(fromDate == "0001-01-01" && toDate == "0001-01-01")
                {
                    assignedFromDate = DateTime.Today;
                    fromDate = assignedFromDate.Date.ToString("yyyy-MM-dd");
                }*/
                if(fromDate != "0001-01-01" || toDate != "0001-01-01")
                {
                    if(fromDate == "0001-01-01")
                        fromDate = toDate;
                    
                    if(toDate == "0001-01-01")
                        toDate = fromDate;
                }

                if(fromDate != "0001-01-01")
                    whereCond += $" and p.assigned_date between '" + fromDate + timeMin + "' and '" + toDate + timeMin + "'";
                    //whereCond += $" and p.assigned_date <= '" + fromDate + timeMin + "'" +
                                // $" and p.assigned_date >= '" + toDate + timeMin + "'";

                var orderCond = $" order by p.created_on DESC ";

                var sqlSelQuery = $"select " + ColumAssign + " from " + tableName + whereCond + orderCond;
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

                        if (!string.IsNullOrEmpty(searchStatus))
                        {
                            if(searchStatus.Equals("scheduled"))
                            {
                                if(scheduleDrCallIdList.Count > 0)
                                    retPatientDetailsList.Add(singlePatienDetails);
                            }
                            else if(searchStatus.Equals("notscheduled"))
                            {
                                if(scheduleDrCallIdList.Count == 0)
                                    retPatientDetailsList.Add(singlePatienDetails);
                            }
                            else
                                retPatientDetailsList.Add(singlePatienDetails);
                        }
                    }
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retPatientDetailsList;
        }
        public async Task<bool> CheckCRMNumberAvailability(string crmNumber, string companyId, string patientId)
        {
            using (var connection = _appDbContext.Connection)
            {
                var tableName = $"HC_Staff_Patient.patient_obj";
                var whereCond = $" where status = '" + Status.Active.ToString() + "'";

                if(!string.IsNullOrEmpty(crmNumber))
                    whereCond += $" and crm_no = @CRMNo";

                if(!string.IsNullOrEmpty(companyId))
                    whereCond += $" and company_id = @CompanyId";

                if(!string.IsNullOrEmpty(patientId))
                    whereCond += $" and patient_id != @PatientId";

                object colValueParam = new
                {
                    CRMNo = crmNumber.Trim(),
                    CompanyId = companyId,
                    PatientId = patientId
                };
                var sqlSelQuery = $"select * from " + tableName + whereCond;
                var sqlResult = await connection.QueryAsync<string>(sqlSelQuery, colValueParam);
                return sqlResult.ToList().Any();
            }
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
                              $"p.nationality_id as NationalityId, n.country_name as NationalityName, " +
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
        public async Task<bool> CreateFilePatient(FilePatientRequest filePatientRequest)
        {
            //try
            //{
            bool sqlResult = true;
            filePatientRequest.CreatedPatientIdList = new List<string>();
            filePatientRequest.DuplicatedPatientRequestList = new List<PatientRequest>();
            filePatientRequest.ErroredPatientRequestList = new List<PatientRequest>();
            foreach(PatientRequest singlePatientRequest in filePatientRequest.PatientRequestList)
            {
                try
                {
                    if(! await CheckCRMNumberAvailability(singlePatientRequest.CRMNo, singlePatientRequest.CompanyId, ""))
                    {
                        if(!String.IsNullOrEmpty(singlePatientRequest.NationalityName))
                            singlePatientRequest.NationalityId = await GetNationalityId(singlePatientRequest.NationalityName);
                        else
                            singlePatientRequest.NationalityId = 0;
                        if(!String.IsNullOrEmpty(singlePatientRequest.RequestCrmName))
                            singlePatientRequest.RequestId = await GetRequestId(singlePatientRequest.RequestCrmName);
                        else
                            singlePatientRequest.RequestId = 0;
                        sqlResult = await CreatePatient(singlePatientRequest);
                        if(sqlResult)
                            filePatientRequest.CreatedPatientIdList.Add(singlePatientRequest.PatientId);
                        else
                            filePatientRequest.ErroredPatientRequestList.Add(singlePatientRequest);
                    }
                    else
                        filePatientRequest.DuplicatedPatientRequestList.Add(singlePatientRequest);
                }
                catch (Exception Err)
                {
                    var Error = Err.Message.ToString();
                    singlePatientRequest.ErrorMsg = Err.Message.ToString();
                    filePatientRequest.ErroredPatientRequestList.Add(singlePatientRequest);
                }
            }
            /*}
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
                return false;
            }*/
            return sqlResult;
        }

        async Task<int> GetNationalityId(string nationalityName)
        {
            int retVal = 0;
            try
            {
                var tableName = $"HC_Master_Details.nationality_obj";

                var whereCond = $" where country_name = '" + nationalityName + "'";

                var sqlSelQuery = $"select country_name from " + tableName + whereCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelResult = await connection.QueryAsync<int>(sqlSelQuery);
                    retVal = sqlSelResult.FirstOrDefault();
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
                retVal = 0;
            }
            return retVal;
        }

        async Task<int> GetRequestId(string requestCrmName)
        {
            int retVal = 0;
            try
            {
                var tableName = $"HC_Master_Details.request_crm_obj";

                var whereCond = $" where request_crm_name = '" + requestCrmName + "'";

                var sqlSelQuery = $"select request_crm_id from " + tableName + whereCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelResult = await connection.QueryAsync<int>(sqlSelQuery);
                    retVal = sqlSelResult.FirstOrDefault();
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
                retVal = 0;
            }
            return retVal;
        }

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
                              $"assigned_date, " +
                              $"mobile_no, google_map_link, no_of_adults, no_of_childrens, " +
                              $"enrolled_count, enrolled_details, " +
                              $"pcr_count, sticker_application, tracker_application, " +
                              $"sticker_removal, tracker_removal, " +
                              $"reception_date, reception_status, reception_remarks, " +
                              $"created_by, created_on";

                var colValueName = $"@PatientId, @PatientName, @CompanyId, @RequestId, @CRMNo, @EIDNo, " +
                                   $"@DateOfBirth, @Age, @Sex, @Address, @LandMark, @Area, @CityId, @NationalityId, " +
                                   $"@AssignedDate, " +
                                   $"@MobileNo, @GoogleMapLink, @AdultsCount, @ChildrensCount, " +
                                   $"@EnrolledCount, @EnrolledDetails, " +
                                   $"@PCRCount, @StickerApplication, @TrackerApplication, " +
                                   $"@StickerRemoval, @TrackerRemoval, " +
                                   $"@RecptionCallDate, @RecptionCallStatus, @RecptionCallRemarks, " +
                                   $"@CreatedBy, @CreatedOn";

                var sqlInsQuery = $"INSERT INTO "+ tableName + "( " + colName + " )" +
                                    $"VALUES ( " + colValueName + " )";

                string dateOfBirth = "";
                dateOfBirth = patientRequest.DateOfBirth.ToString("yyyy-MM-dd");
                if(dateOfBirth == "0001-01-01")
                    dateOfBirth = "";

                string recptionCallDate = "";
                /*recptionCallDate = patientRequest.RecptionCallDate.ToString("yyyy-MM-dd");
                if(recptionCallDate == "0001-01-01")
                    recptionCallDate = "";*/

                string assignedDate = "";
                assignedDate = patientRequest.AssignedDate.ToString("yyyy-MM-dd");
                if(assignedDate == "0001-01-01")
                    assignedDate = "";
                else
                    assignedDate = patientRequest.AssignedDate.ToString("yyyy-MM-dd 00:00:00.0");

                if(String.IsNullOrEmpty(patientRequest.GoogleMapLink))
                    patientRequest.GoogleMapLink = "";

                var crmNo = "";
                if(!String.IsNullOrEmpty(patientRequest.CRMNo))
                    crmNo = patientRequest.CRMNo.Trim();

                object colValueParam = new
                {
                    PatientId = patientRequest.PatientId,
                    PatientName = patientRequest.PatientName,
                    CompanyId = patientRequest.CompanyId,
                    RequestId = patientRequest.RequestId,
                    CRMNo = crmNo,
                    EIDNo = patientRequest.EIDNo,
                    DateOfBirth = dateOfBirth,//patientRequest.DateOfBirth.ToString("yyyy-MM-dd 00:00:00.0"),
                    Age = patientRequest.Age,
                    Sex = patientRequest.Sex,
                    Address = patientRequest.Address,
                    LandMark = patientRequest.LandMark,
                    Area = patientRequest.Area,
                    CityId = patientRequest.CityId,
                    NationalityId = patientRequest.NationalityId,
                    AssignedDate = assignedDate,
                    MobileNo = patientRequest.MobileNo,
                    GoogleMapLink = patientRequest.GoogleMapLink,
                    AdultsCount = patientRequest.AdultsCount,
                    ChildrensCount = patientRequest.ChildrensCount,
                    EnrolledCount = patientRequest.EnrolledCount,
                    EnrolledDetails = patientRequest.EnrolledDetails,
                    PCRCount = patientRequest.AdultsCount,
                    StickerApplication = patientRequest.StickerApplication,
                    TrackerApplication = patientRequest.AdultsCount,
                    StickerRemoval = patientRequest.StickerRemoval,
                    TrackerRemoval = patientRequest.AdultsCount,
                    RecptionCallDate = recptionCallDate,
                    RecptionCallStatus = patientRequest.RecptionCallStatus,
                    RecptionCallRemarks = patientRequest.RecptionCallRemarks,
                    CreatedBy = patientRequest.CreatedBy,
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
                patientRequest.ErrorMsg = Err.Message.ToString();
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
            {//reception_date
                bool sqlResult = true;
                var tableName = $"HC_Staff_Patient.patient_obj";

                var colName = $"patient_id = @PatientId, patient_name = @PatientName, company_id = @CompanyId, " +
                              $"request_id = @RequestId, crm_no = @CRMNo, eid_no = @EIDNo, " +
                              $"date_of_birth = @DateOfBirth, age = @Age, sex = @Sex, address = @Address, " +
                              $"assigned_date = @AssignedDate, " +
                              $"landmark = @LandMark, area = @Area, city_id = @CityId, nationality_id = @NationalityId, " +
                              $"mobile_no = @MobileNo, google_map_link = @GoogleMapLink, " +
                              $"no_of_adults = @AdultsCount, no_of_childrens = @ChildrensCount, " +
                              $"enrolled_count = @EnrolledCount, enrolled_details = @EnrolledDetails, " +
                              $"pcr_count = @PCRCount, " +
                              $"sticker_application = @StickerApplication, tracker_application = @TrackerApplication, " +
                              $"sticker_removal = @StickerRemoval, tracker_removal = @TrackerRemoval, " +
                              $"reception_date = @RecptionCallDate, reception_status = @RecptionCallStatus, " +
                              $"reception_remarks = @RecptionCallRemarks, " +
                              $"modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                var whereCond = $" where patient_id = @PatientId";
                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                string dateOfBirth = "";
                dateOfBirth = patientRequest.DateOfBirth.ToString("yyyy-MM-dd");
                if(dateOfBirth == "0001-01-01")
                    dateOfBirth = "";

                string recptionCallDate = patientRequest.RecptionCallDate.ToString("yyyy-MM-dd");
                if(patientRequest.IsReception && recptionCallDate == "0001-01-01")
                {
                    if(patientRequest.RecptionCallStatus.ToLower() == "completed")
                        recptionCallDate = DateTime.Today.ToString("yyyy-MM-dd 00:00:00.0");
                }

                if(recptionCallDate == "0001-01-01")
                    recptionCallDate = "";

                string assignedDate = "";
                assignedDate = patientRequest.AssignedDate.ToString("yyyy-MM-dd");
                if(assignedDate == "0001-01-01")
                    assignedDate = "";
                else
                    assignedDate = patientRequest.AssignedDate.ToString("yyyy-MM-dd 00:00:00.0");

                if(String.IsNullOrEmpty(patientRequest.GoogleMapLink))
                    patientRequest.GoogleMapLink = "";

                var crmNo = "";
                if(!String.IsNullOrEmpty(patientRequest.CRMNo))
                    crmNo = patientRequest.CRMNo.Trim();

                object colValueParam = new
                {
                    PatientId = patientRequest.PatientId,
                    PatientName = patientRequest.PatientName,
                    CompanyId = patientRequest.CompanyId,
                    RequestId = patientRequest.RequestId,
                    CRMNo = patientRequest.CRMNo,
                    EIDNo = patientRequest.EIDNo,
                    DateOfBirth = patientRequest.DateOfBirth.ToString("yyyy-MM-dd 00:00:00.0"),
                    Age = patientRequest.Age,
                    Sex = patientRequest.Sex,
                    Address = patientRequest.Address,
                    LandMark = patientRequest.LandMark,
                    Area = patientRequest.Area,
                    CityId = patientRequest.CityId,
                    NationalityId = patientRequest.NationalityId,
                    AssignedDate = assignedDate,
                    MobileNo = patientRequest.MobileNo,
                    GoogleMapLink = patientRequest.GoogleMapLink,
                    AdultsCount = patientRequest.AdultsCount,
                    ChildrensCount = patientRequest.ChildrensCount,
                    EnrolledCount = patientRequest.EnrolledCount,
                    EnrolledDetails = patientRequest.EnrolledDetails,
                    StickerApplication = patientRequest.StickerApplication,
                    TrackerApplication = patientRequest.TrackerApplication,
                    PCRCount = patientRequest.PCRCount,
                    StickerRemoval = patientRequest.StickerRemoval,
                    TrackerRemoval = patientRequest.TrackerRemoval,
                    RecptionCallDate = recptionCallDate,
                    RecptionCallStatus = patientRequest.RecptionCallStatus,
                    RecptionCallRemarks = patientRequest.RecptionCallRemarks,
                    ModifiedBy = patientRequest.ModifiedBy,
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

        public async Task<bool> DeletePatient(DeleteRequest request)
        {
            try
            {
                var tableName = $"HC_Staff_Patient.patient_obj";
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
                    var sqlResult = Convert.ToBoolean(await connection.ExecuteAsync(sqlUpdateQuery, colValueParam));
                    if(sqlResult)
                        sqlResult = Convert.ToBoolean(await DeleteSchedule(request));
                    return sqlResult;
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
                return false;
            }
        }

        async Task<bool> DeleteSchedule(DeleteRequest request)
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
        }

    }
}