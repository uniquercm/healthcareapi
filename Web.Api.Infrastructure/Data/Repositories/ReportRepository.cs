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
        public async Task<List<ReportDetails>> GetReportDetails(string companyId, string patientId, string scheduledId)
        {
            List<ReportDetails> retReportDetailsList = new List<ReportDetails>();
            try
            {
                var tableName = $"HC_Master_Details.company_obj co, " +
                                $"HC_Master_Details.request_crm_obj rc, " +
                                $"HC_Staff_Patient.patient_obj pa, " +
                                $"HC_Treatment.scheduled_obj sc";

                var ColumAssign = $"sc.scheduled_id as ScheduledId, " +
                                  $"pa.patient_id as PatientId, pa.patient_name as PatientName," +
                                  $"pa.company_id as CompanyId, co.company_name as CompanyName, " +
                                  $"pa.request_id as RequestId, rc.request_crm_name as RequestCrmName, " +
                                  $"pa.crm_no as CRMNo, pa.eid_no as EIDNo, pa.mobile_no as MobileNo, " +
                                  //$", " +//Include Reception call Details
                                  $"sc.2day_call_id as Day2CallId, " +//Include Dr Call Details
                                  $"sc.4day_pcr_test_date as PCR4DayTestDate, sc.4day_pcr_test_result as PCR4DayResult, " +
                                  $"sc.8day_pcr_test_date as PCR8DayTestDate, sc.8day_pcr_test_result as PCR8DayResult, " +
                                  $"sc.3day_call_id as Day3CallId, sc.5day_call_id as Day5CallId, " +
                                  $"sc.6day_call_id as Day6CallId, sc.7day_call_id as Day7CallId, " +
                                  $"sc.9day_call_id as Day9CallId, " +
                                  //$"sc.discharge_date as DischargeDate, " +//Include the Discharge Status
                                  $"sc.discharge_date as DischargeDate";
                                  //$", " +//Include Extract Data
                                  //$"";//Include Send Claim Option and Send on Date

                var whereCond = $" where sc.patient_id = pa.patient_id" +
                                $" and pa.company_id = co.company_id" +
                                $" and pa.request_id = rc.request_crm_id" +
                                $"";

                if (!string.IsNullOrEmpty(companyId))
                    whereCond += " and pa.company_id = '" + companyId + "'";

                if (!string.IsNullOrEmpty(patientId))
                    whereCond += " and pa.patient_id = '" + patientId + "'";

                if (!string.IsNullOrEmpty(scheduledId))
                    whereCond += " and sc.scheduled_id = '" + scheduledId + "'";

                var sqlSelQuery = $"select " + ColumAssign + " from " + tableName + whereCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelResult = await connection.QueryAsync<ReportDetails>(sqlSelQuery);
                    retReportDetailsList = sqlSelResult.ToList();
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retReportDetailsList;
        }
    }
}