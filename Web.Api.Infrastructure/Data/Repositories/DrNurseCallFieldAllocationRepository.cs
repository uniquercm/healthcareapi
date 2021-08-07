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

        // public async Task<List<DrNurseCallDetails>> GetFieldAllowCallDetails(string companyId, string teamUserName, string callName, DateTime scheduledFromDate, DateTime scheduledToDate, string callStatus, string serviceName, string serviceStatus, string dateSearchType, string areaNames)
        // {
        //     List<DrNurseCallDetails> retDrNurseCallDetails = new List<DrNurseCallDetails>();
        //     List<DrNurseCallDetails> dayCallDetails = new List<DrNurseCallDetails>();
        //     try
        //     {//all, tracker, sticker, 4pcr, 8pcr, discharge
        //         if(!String.IsNullOrEmpty(serviceName))
        //         {
        //             /*comminted for 4 th PCR
        //             if(serviceName.Equals("4pcr") || serviceName.Equals("all"))
        //             {
        //                 dayCallDetails = await GetPCRCallDetails(companyId, teamUserName, "4", scheduledFromDate, scheduledToDate, callStatus, serviceStatus, dateSearchType, areaNames);
        //                 if(retDrNurseCallDetails.Count > 0)
        //                 {
        //                     foreach(DrNurseCallDetails singleDrNurseCallDetails in dayCallDetails)
        //                         retDrNurseCallDetails.Add(singleDrNurseCallDetails);
        //                 }
        //                 else
        //                     retDrNurseCallDetails = dayCallDetails;
        //             }*/

        //             string fromDate = scheduledFromDate.Date.ToString("yyyy-MM-dd");
        //             string toDate = scheduledToDate.Date.ToString("yyyy-MM-dd");
        //             if(fromDate == "0001-01-01" && toDate == "0001-01-01")
        //             {
        //                 scheduledFromDate = DateTime.Today;
        //                 fromDate = scheduledFromDate.Date.ToString("yyyy-MM-dd");
        //             }
        //             if(fromDate != "0001-01-01" || toDate != "0001-01-01")
        //             {
        //                 if(fromDate == "0001-01-01")
        //                     fromDate = toDate;
                        
        //                 if(toDate == "0001-01-01")
        //                     scheduledToDate = scheduledFromDate;
        //             }

        //             if(dateSearchType.Equals("allocated") && serviceName.Equals("all") && serviceStatus.Equals("all"))
        //                 retDrNurseCallDetails = await GetAllocatedDateDetails(companyId, teamUserName, scheduledFromDate, scheduledToDate);
        //             else
        //             {
        //                 if(serviceName.Equals("tracker") || serviceName.Equals("all"))
        //                 {
        //                     dayCallDetails = await GetTrackerStickerCallDetails(companyId, teamUserName, true, scheduledFromDate, scheduledToDate, callStatus, serviceStatus, dateSearchType, areaNames);
        //                     if(retDrNurseCallDetails.Count > 0)
        //                     {
        //                         foreach(DrNurseCallDetails singleDrNurseCallDetails in dayCallDetails)
        //                         {
        //                             retDrNurseCallDetails.Add(singleDrNurseCallDetails);
        //                         }
        //                     }
        //                     else
        //                         retDrNurseCallDetails = dayCallDetails;
        //                 }

        //                 if(serviceName.Equals("6pcr") || serviceName.Equals("all"))
        //                 {
        //                     dayCallDetails = await GetPCRCallDetails(companyId, teamUserName, "6", scheduledFromDate, scheduledToDate, callStatus, serviceStatus, dateSearchType, areaNames);
        //                     if(retDrNurseCallDetails.Count > 0)
        //                     {
        //                         foreach(DrNurseCallDetails singleDrNurseCallDetails in dayCallDetails)
        //                         {
        //                             bool isAdd = true;
        //                             foreach(DrNurseCallDetails addedListSingDrNurCallDet in retDrNurseCallDetails)
        //                             {
        //                                 if(singleDrNurseCallDetails.CRMNo.Trim().Equals(addedListSingDrNurCallDet.CRMNo.Trim()))
        //                                 {
        //                                     isAdd = false;
        //                                     break;
        //                                 }
        //                             }
        //                             if(isAdd)
        //                                 retDrNurseCallDetails.Add(singleDrNurseCallDetails);
        //                         }
        //                     }
        //                     else
        //                         retDrNurseCallDetails = dayCallDetails;
        //                 }

        //                 if(serviceName.Equals("8pcr") || serviceName.Equals("eight") || serviceName.Equals("all"))
        //                 {
        //                     dayCallDetails = await GetPCRCallDetails(companyId, teamUserName, "8", scheduledFromDate, scheduledToDate, callStatus, serviceStatus, dateSearchType, areaNames);
        //                     if(retDrNurseCallDetails.Count > 0)
        //                     {
        //                         foreach(DrNurseCallDetails singleDrNurseCallDetails in dayCallDetails)
        //                         {
        //                             bool isAdd = true;
        //                             foreach(DrNurseCallDetails addedListSingDrNurCallDet in retDrNurseCallDetails)
        //                             {
        //                                 if(singleDrNurseCallDetails.CRMNo.Trim().Equals(addedListSingDrNurCallDet.CRMNo.Trim()))
        //                                 {
        //                                     isAdd = false;
        //                                     break;
        //                                 }
        //                             }
        //                             if(isAdd)
        //                                 retDrNurseCallDetails.Add(singleDrNurseCallDetails);
        //                         }
        //                     }
        //                     else
        //                         retDrNurseCallDetails = dayCallDetails;
        //                 }

        //                 if(serviceName.Equals("11pcr") || serviceName.Equals("all"))
        //                 {
        //                     dayCallDetails = await GetPCRCallDetails(companyId, teamUserName, "11", scheduledFromDate, scheduledToDate, callStatus, serviceStatus, dateSearchType, areaNames);
        //                     if(retDrNurseCallDetails.Count > 0)
        //                     {
        //                         foreach(DrNurseCallDetails singleDrNurseCallDetails in dayCallDetails)
        //                         {
        //                             bool isAdd = true;
        //                             foreach(DrNurseCallDetails addedListSingDrNurCallDet in retDrNurseCallDetails)
        //                             {
        //                                 if(singleDrNurseCallDetails.CRMNo.Trim().Equals(addedListSingDrNurCallDet.CRMNo.Trim()))
        //                                 {
        //                                     isAdd = false;
        //                                     break;
        //                                 }
        //                             }
        //                             if(isAdd)
        //                                 retDrNurseCallDetails.Add(singleDrNurseCallDetails);
        //                         }
        //                     }
        //                     else
        //                         retDrNurseCallDetails = dayCallDetails;
        //                 }

        //                 if(serviceName.Equals("sticker") || serviceName.Equals("all"))
        //                 {
        //                     dayCallDetails = await GetTrackerStickerCallDetails(companyId, teamUserName, false, scheduledFromDate, scheduledToDate, callStatus, serviceStatus, dateSearchType, areaNames);
        //                     if(retDrNurseCallDetails.Count > 0)
        //                     {
        //                         foreach(DrNurseCallDetails singleDrNurseCallDetails in dayCallDetails)
        //                         {
        //                             bool isAdd = true;
        //                             foreach(DrNurseCallDetails addedListSingDrNurCallDet in retDrNurseCallDetails)
        //                             {
        //                                 if(singleDrNurseCallDetails.CRMNo.Trim().Equals(addedListSingDrNurCallDet.CRMNo.Trim()))
        //                                 {
        //                                     isAdd = false;
        //                                     break;
        //                                 }
        //                             }
        //                             if(isAdd)
        //                                 retDrNurseCallDetails.Add(singleDrNurseCallDetails);
        //                         }
        //                     }
        //                     else
        //                         retDrNurseCallDetails = dayCallDetails;
        //                 }

        //                 if(serviceName.Equals("discharge") || serviceName.Equals("all"))
        //                 {
        //                     dayCallDetails = await GetDischargeCallDetails(companyId, teamUserName, scheduledFromDate, scheduledToDate, callStatus, serviceStatus, dateSearchType, areaNames);
        //                     if(retDrNurseCallDetails.Count > 0)
        //                     {
        //                         foreach(DrNurseCallDetails singleDrNurseCallDetails in dayCallDetails)
        //                         {
        //                             bool isAdd = true;
        //                             foreach(DrNurseCallDetails addedListSingDrNurCallDet in retDrNurseCallDetails)
        //                             {
        //                                 if(singleDrNurseCallDetails.CRMNo.Trim().Equals(addedListSingDrNurCallDet.CRMNo.Trim()))
        //                                 {
        //                                     if(addedListSingDrNurCallDet.CallId.Equals("sticker"))
        //                                         addedListSingDrNurCallDet.ShowDischage = true;
        //                                     isAdd = false;
        //                                     break;
        //                                 }
        //                             }
        //                             if(isAdd)
        //                                 retDrNurseCallDetails.Add(singleDrNurseCallDetails);
        //                         }
        //                     }
        //                     else
        //                         retDrNurseCallDetails = dayCallDetails;
        //                 }
        //                 /*if(serviceName.Equals("team") || serviceName.Equals("all"))
        //                 {
        //                     dayCallDetails = await GetTeamFieldAllowCallDetails(companyId, teamUserName, scheduledFromDate, scheduledToDate, callStatus, serviceStatus, serviceName, areaNames);
        //                     if(retDrNurseCallDetails.Count > 0)
        //                     {
        //                         foreach(DrNurseCallDetails singleDrNurseCallDetails in dayCallDetails)
        //                             retDrNurseCallDetails.Add(singleDrNurseCallDetails);
        //                     }
        //                     else
        //                         retDrNurseCallDetails = dayCallDetails;
        //                 }*/
        //             }
        //         }
        //     }
        //     catch (Exception Err)
        //     {
        //         var Error = Err.Message.ToString();
        //     }
        //     return retDrNurseCallDetails;
        // }

        public async Task<List<DrNurseCallDetails>> GetFieldAllowCallDetails(string companyId, string teamUserName, string callName, DateTime scheduledFromDate, DateTime scheduledToDate, string callStatus, string serviceName, string serviceStatus, string dateSearchType, string areaNames)
        {
            List<DrNurseCallDetails> retDrNurseCallDetails = new List<DrNurseCallDetails>();
            List<DrNurseCallDetails> dayCallDetails = new List<DrNurseCallDetails>();
            try
            {//all, tracker, sticker, 4pcr, 8pcr, discharge
                if(!String.IsNullOrEmpty(serviceName))
                {
                    string fromDate = scheduledFromDate.Date.ToString("yyyy-MM-dd");
                    string toDate = scheduledToDate.Date.ToString("yyyy-MM-dd");
                    if(fromDate == "0001-01-01" && toDate == "0001-01-01")
                    {
                        scheduledFromDate = DateTime.Today;
                        fromDate = scheduledFromDate.Date.ToString("yyyy-MM-dd");
                    }
                    if(fromDate != "0001-01-01" || toDate != "0001-01-01")
                    {
                        if(fromDate == "0001-01-01")
                            fromDate = toDate;
                        
                        if(toDate == "0001-01-01")
                            scheduledToDate = scheduledFromDate;
                    }

                    if(dateSearchType.Equals("allocated") && serviceName.Equals("all") && serviceStatus.Equals("all"))
                        retDrNurseCallDetails = await GetAllocatedDateDetails(companyId, teamUserName, scheduledFromDate, scheduledToDate);
                    else
                    {
                        if(serviceName.Equals("tracker") || serviceName.Equals("all"))
                        {
                            dayCallDetails = await GetTrackerStickerCallDetails(companyId, teamUserName, true, scheduledFromDate, scheduledToDate, callStatus, serviceStatus, dateSearchType, areaNames);
                            foreach(DrNurseCallDetails singleDrNurseCallDetails in dayCallDetails)
                            {
                                AssignAllocatedDate(teamUserName, singleDrNurseCallDetails);
                                retDrNurseCallDetails.Add(singleDrNurseCallDetails);
                            }
                        }

                        if(serviceName.Equals("6pcr") || serviceName.Equals("all"))
                        {
                            dayCallDetails = await GetPCRCallDetails(companyId, teamUserName, "6", scheduledFromDate, scheduledToDate, callStatus, serviceStatus, dateSearchType, areaNames);
                            foreach(DrNurseCallDetails singleDrNurseCallDetails in dayCallDetails)
                            {
                                AssignAllocatedDate(teamUserName, singleDrNurseCallDetails);
                                retDrNurseCallDetails.Add(singleDrNurseCallDetails);
                            }
                        }

                        if(serviceName.Equals("8pcr") || serviceName.Equals("eight") || serviceName.Equals("all"))
                        {
                            dayCallDetails = await GetPCRCallDetails(companyId, teamUserName, "8", scheduledFromDate, scheduledToDate, callStatus, serviceStatus, dateSearchType, areaNames);
                            foreach(DrNurseCallDetails singleDrNurseCallDetails in dayCallDetails)
                            {
                                AssignAllocatedDate(teamUserName, singleDrNurseCallDetails);
                                retDrNurseCallDetails.Add(singleDrNurseCallDetails);
                            }
                        }

                        if(serviceName.Equals("11pcr") || serviceName.Equals("all"))
                        {
                            dayCallDetails = await GetPCRCallDetails(companyId, teamUserName, "11", scheduledFromDate, scheduledToDate, callStatus, serviceStatus, dateSearchType, areaNames);
                            foreach(DrNurseCallDetails singleDrNurseCallDetails in dayCallDetails)
                            {
                                AssignAllocatedDate(teamUserName, singleDrNurseCallDetails);
                                retDrNurseCallDetails.Add(singleDrNurseCallDetails);
                            }
                        }

                        if(serviceName.Equals("sticker") || serviceName.Equals("all"))
                        {
                            dayCallDetails = await GetTrackerStickerCallDetails(companyId, teamUserName, false, scheduledFromDate, scheduledToDate, callStatus, serviceStatus, dateSearchType, areaNames);
                            foreach(DrNurseCallDetails singleDrNurseCallDetails in dayCallDetails)
                            {
                                AssignAllocatedDate(teamUserName, singleDrNurseCallDetails);
                                retDrNurseCallDetails.Add(singleDrNurseCallDetails);
                            }
                        }

                        if(serviceName.Equals("discharge") || serviceName.Equals("all"))
                        {
                            dayCallDetails = await GetDischargeCallDetails(companyId, teamUserName, scheduledFromDate, scheduledToDate, callStatus, serviceStatus, dateSearchType, areaNames);
                            foreach(DrNurseCallDetails singleDrNurseCallDetails in dayCallDetails)
                            {
                                AssignAllocatedDate(teamUserName, singleDrNurseCallDetails);
                                retDrNurseCallDetails.Add(singleDrNurseCallDetails);
                            }
                        }
                    }
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retDrNurseCallDetails;
        }

        void AssignAllocatedDate(string teamUserName, DrNurseCallDetails singleDrNurseCallDetails)
        {
            try
            {
                if(!String.IsNullOrEmpty(teamUserName))
                {
                    if(!String.IsNullOrEmpty(singleDrNurseCallDetails.AllocatedTeamName))
                    {
                        if(teamUserName.ToLower().Equals(singleDrNurseCallDetails.AllocatedTeamName.ToLower()))
                            singleDrNurseCallDetails.TeamAllocatedDate = singleDrNurseCallDetails.AllocatedDate;
                    }
                    if(!String.IsNullOrEmpty(singleDrNurseCallDetails.ReAllocatedTeamName))
                    {
                        if(teamUserName.ToLower().Equals(singleDrNurseCallDetails.ReAllocatedTeamName.ToLower()))
                            singleDrNurseCallDetails.TeamAllocatedDate = singleDrNurseCallDetails.ReAllocatedDate;
                    }
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
        }

        public async Task<List<DrNurseCallDetails>> GetDrNurseCallDetails(string companyId, string teamUserName, string callName, DateTime scheduledFromDate, DateTime scheduledToDate, string callStatus, string serviceName, string areaNames)
        {
            List<DrNurseCallDetails> retDrNurseCallDetails = new List<DrNurseCallDetails>();
            try
            {
                var tableName = $"HC_Staff_Patient.patient_obj p, " +
                                $"HC_Master_Details.company_obj co, " +
                                $"HC_Treatment.scheduled_obj sc, " +
                                $"HC_Treatment.call_obj ca, " +
                                $"HC_Master_Details.city_obj ci, HC_Master_Details.nationality_obj n, " +
                                $"HC_Master_Details.request_crm_obj rc";

                var ColumAssign = $"p.patient_id as PatientId, p.patient_name as PatientName, " +
                                  $"p.company_id as CompanyId, co.company_name as CompanyName, " +
                                  $"p.request_id as RequestId, rc.request_crm_name as RequestCrmName, " +
                                  $"p.crm_no as CRMNo, p.eid_no as EIDNo, " +
                                  $"p.area as Area, p.age as Age, " +
                                  $"p.city_id as CityId, ci.city_name as CityName, " +
                                  $"p.nationality_id as NationalityId, n.country_name as NationalityName, " +
                                  $"p.mobile_no as MobileNo, ca.call_id as CallId, " +
                                  $"ca.called_date as CalledDate, ca.scheduled_id as ScheduledId, " +
                                  $"ca.call_scheduled_date as CallScheduledDate, " +
                                  $"ca.call_status as CallStatus, ca.remarks as Remarks, " +
                                  $"ca.emr_done as EMRDone, ca.is_pcr as IsPCRCall";

                var whereCond = $" where p.company_id = co.company_id" +
                                $" and p.patient_id = sc.patient_id" +
                                $" and ca.scheduled_id = sc.scheduled_id" +
                                $" and p.city_id = ci.city_id" +
                                $" and p.nationality_id = n.nationality_id" +
                                $" and p.status = 'Active'" +
                                $" and sc.status = 'Active'" +
                                $" and p.reception_status != 'closed'" +
                                $" and p.request_id = rc.request_crm_id";

                if(!String.IsNullOrEmpty(areaNames) && areaNames.ToLower() != "all")
                {
                    string[] areaArray = areaNames.Replace("[","").Replace("]","").Replace("\"","").Split(',');

                    for(int i = 0; i < areaArray.Count(); i++)
                    {
                        if(i == 0 && areaArray.Count() > 1)
                            whereCond += " and (p.area = '" + areaArray[i] + "'";
                        else if(i == 0)
                            whereCond += " and p.area = '" + areaArray[i] + "'";
                        else if(i == areaArray.Count()-1)
                            whereCond += " or p.area = '" + areaArray[i] + "')";
                        else 
                            whereCond += " or p.area = '" + areaArray[i] + "'";
                    }
                }

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

                // if(!String.IsNullOrEmpty(serviceStatus) && callName != "DrCall")
                // {//all, tracker, sticker, 4pcr, 8pcr, discharge
                //     /*if(serviceStatus.Equals("tracker"))
                //     whereCond += $" and sc.sticker_application = 'yes'";
                //     else */if(serviceStatus.Equals("sticker"))
                //         whereCond += $" and p.sticker_application = 'yes'";
                //     else if(serviceStatus.Equals("discharge"))
                //         whereCond += $" and sc.discharge_date = '" + fromDate + "'";
                // }

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
        public async Task<List<DrNurseCallDetails>> GetTeamFieldAllowCallDetails(string companyId, string teamUserName, DateTime scheduledFromDate, DateTime scheduledToDate, string callStatus, string serviceName, string areaNames)
        {
            List<DrNurseCallDetails> retDrNurseCallDetails = new List<DrNurseCallDetails>();
            try
            {
                var tableName = $"HC_Staff_Patient.patient_obj p, " +
                                $"HC_Master_Details.company_obj co, " +
                                $"HC_Treatment.scheduled_obj sc, " +
                                $"HC_Treatment.call_obj ca, " +
                                $"HC_Master_Details.city_obj ci, HC_Master_Details.nationality_obj n, " +
                                $"HC_Master_Details.request_crm_obj rc";

                var ColumAssign = $"p.patient_id as PatientId, p.patient_name as PatientName, " +
                                  $"p.company_id as CompanyId, co.company_name as CompanyName, " +
                                  $"p.request_id as RequestId, rc.request_crm_name as RequestCrmName, " +
                                  $"p.crm_no as CRMNo, p.eid_no as EIDNo, " +
                                  $"p.area as Area, p.age as Age, " +
                                  $"sc.allocated_team_name as AllocatedTeamName, sc.team_allocated_date as AllocatedDate, " +
                                  $"sc.reallocated_team_name as ReAllocatedTeamName, sc.team_reallocated_date as ReAllocatedDate, " +
                                  $"p.city_id as CityId, ci.city_name as CityName, " +
                                  $"p.nationality_id as NationalityId, n.country_name as NationalityName, " +
                                  $"p.mobile_no as MobileNo, ca.call_id as CallId, " +
                                  $"ca.called_date as CalledDate, ca.scheduled_id as ScheduledId, " +
                                  $"ca.call_scheduled_date as CallScheduledDate, " +
                                  $"ca.call_status as CallStatus, ca.remarks as Remarks, " +
                                  $"ca.emr_done as EMRDone, ca.is_pcr as IsPCRCall";

                var whereCond = $" where p.company_id = co.company_id" +
                                $" and p.patient_id = sc.patient_id" +
                                $" and ca.scheduled_id = sc.scheduled_id" +
                                $" and p.city_id = ci.city_id" +
                                $" and p.nationality_id = n.nationality_id" +
                                $" and p.status = 'Active'" +
                                $" and sc.status = 'Active'" +
                                $" and p.reception_status != 'closed'" +
                                $" and p.request_id = rc.request_crm_id";

                if(!String.IsNullOrEmpty(areaNames) && areaNames.ToLower() != "all")
                {
                    string[] areaArray = areaNames.Replace("[","").Replace("]","").Replace("\"","").Split(',');

                    for(int i = 0; i < areaArray.Count(); i++)
                    {
                        if(i == 0 && areaArray.Count() > 1)
                            whereCond += " and (p.area = '" + areaArray[i] + "'";
                        else if(i == 0)
                            whereCond += " and p.area = '" + areaArray[i] + "'";
                        else if(i == areaArray.Count()-1)
                            whereCond += " or p.area = '" + areaArray[i] + "')";
                        else 
                            whereCond += " or p.area = '" + areaArray[i] + "'";
                    }
                }

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

                if(!String.IsNullOrEmpty(serviceName))
                {//all, tracker, sticker, 4pcr, 8pcr, discharge
                    if(serviceName.Equals("sticker"))
                        whereCond += $" and p.sticker_application = 'yes'";
                    else if(serviceName.Equals("discharge"))
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
        public async Task<List<DrNurseCallDetails>> GetPCRCallDetails(string companyId, string teamUserName, string pcrDayNumber, DateTime scheduledFromDate, DateTime scheduledToDate, string callStatus, string serviceStatus, string dateSearchType, string areaNames)
        {
            List<DrNurseCallDetails> retDrNurseCallDetails = new List<DrNurseCallDetails>();
            try
            {
                var tableName = $"HC_Staff_Patient.patient_obj p, " +
                                $"HC_Master_Details.company_obj co, " +
                                $"HC_Treatment.scheduled_obj sc, " +
                                $"HC_Master_Details.city_obj ci, HC_Master_Details.nationality_obj n, " +
                                $"HC_Master_Details.request_crm_obj rc";

                var ColumAssign = $"p.patient_id as PatientId, p.patient_name as PatientName, " +
                                  $"p.company_id as CompanyId, co.company_name as CompanyName, " +
                                  $"p.request_id as RequestId, rc.request_crm_name as RequestCrmName, " +
                                  $"p.crm_no as CRMNo, p.eid_no as EIDNo, " +
                                  $"p.area as Area, p.age as Age, " +
                                  $"sc.allocated_team_name as AllocatedTeamName, sc.team_allocated_date as AllocatedDate, " +
                                  $"sc.reallocated_team_name as ReAllocatedTeamName, sc.team_reallocated_date as ReAllocatedDate, " +
                                  $"p.city_id as CityId, ci.city_name as CityName, " +
                                  $"p.nationality_id as NationalityId, n.country_name as NationalityName, " +
                                  $"p.mobile_no as MobileNo, sc.scheduled_id as ScheduledId, " +
                                  $"true as IsPCRCall, false as ShowDischage, ";

                if(pcrDayNumber.Equals("4"))
                    ColumAssign += $"'4thday' as CallId, sc.4day_pcr_test_date as CallScheduledDate, " +
                                  $"sc.4day_pcr_test_sample_date as CalledDate, " +
                                  $"sc.4day_pcr_test_result as CallStatus, " +
                                  $"sc.4day_pcr_team_user_name as TeamUserName, " +
                                  $"sc.4day_pcr_team_status as TeamStatus, " +
                                  $"sc.4day_pcr_team_remark as TeamRemark, " +
                                  $"sc.4day_pcr_team_date as TeamStatusDate";
                else if(pcrDayNumber.Equals("6"))
                    ColumAssign += $"'6thday' as CallId, sc.6day_pcr_test_date as CallScheduledDate, " +
                                  $"sc.6day_pcr_test_sample_date as CalledDate, " +
                                  $"sc.6day_pcr_test_result as CallStatus, " +
                                  $"sc.6day_pcr_team_user_name as TeamUserName, " +
                                  $"sc.6day_pcr_team_status as TeamStatus, " +
                                  $"sc.6day_pcr_team_remark as TeamRemark, " +
                                  $"sc.6day_pcr_team_date as TeamStatusDate";
                else if(pcrDayNumber.Equals("8"))
                    ColumAssign += $"'8thday' as CallId, sc.8day_pcr_test_date as CallScheduledDate, " +
                                  $"sc.8day_pcr_test_sample_date as CalledDate, " +
                                  $"sc.8day_pcr_test_result as CallStatus, " +
                                  $"sc.8day_pcr_team_user_name as TeamUserName, " +
                                  $"sc.8day_pcr_team_status as TeamStatus, " +
                                  $"sc.8day_pcr_team_remark as TeamRemark, " +
                                  $"sc.8day_pcr_team_date as TeamStatusDate";
                else
                    ColumAssign += $"'11thday' as CallId, sc.11day_pcr_test_date as CallScheduledDate, " +
                                  $"sc.11day_pcr_test_sample_date as CalledDate, " +
                                  $"sc.11day_pcr_test_result as CallStatus, " +
                                  $"sc.11day_pcr_team_user_name as TeamUserName, " +
                                  $"sc.11day_pcr_team_status as TeamStatus, " +
                                  $"sc.11day_pcr_team_remark as TeamRemark, " +
                                  $"sc.11day_pcr_team_date as TeamStatusDate";

                var whereCond = $" where p.company_id = co.company_id" +
                                $" and p.patient_id = sc.patient_id" +
                                $" and p.city_id = ci.city_id" +
                                $" and p.nationality_id = n.nationality_id" +
                                $" and p.status = 'Active'" +
                                $" and sc.status = 'Active'" +
                                $" and p.reception_status != 'closed'" +
                                $" and p.request_id = rc.request_crm_id";

                if(!String.IsNullOrEmpty(areaNames) && areaNames.ToLower() != "all")
                {
                    string[] areaArray = areaNames.Replace("[","").Replace("]","").Replace("\"","").Split(',');

                    for(int i = 0; i < areaArray.Count(); i++)
                    {
                        if(i == 0 && areaArray.Count() > 1)
                            whereCond += " and (p.area = '" + areaArray[i] + "'";
                        else if(i == 0)
                            whereCond += " and p.area = '" + areaArray[i] + "'";
                        else if(i == areaArray.Count()-1)
                            whereCond += " or p.area = '" + areaArray[i] + "')";
                        else 
                            whereCond += " or p.area = '" + areaArray[i] + "'";
                    }
                }

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
                    if(pcrDayNumber.Equals("4"))
                        whereCond += $" and sc.4day_pcr_test_date between '" + fromDate + "' and '" + toDate + "'";
                    else if(pcrDayNumber.Equals("6"))
                        whereCond += $" and sc.6day_pcr_test_date between '" + fromDate + "' and '" + toDate + "'";
                    else if(pcrDayNumber.Equals("8"))
                        whereCond += $" and sc.8day_pcr_test_date between '" + fromDate + "' and '" + toDate + "'";
                    else
                        whereCond += $" and sc.11day_pcr_test_date between '" + fromDate + "' and '" + toDate + "'";
                }
                else if(pcrDayNumber.Equals("4"))
                    whereCond += $" and sc.4day_pcr_test_date = '" + fromDate + "'";
                else if(pcrDayNumber.Equals("6"))
                    whereCond += $" and sc.6day_pcr_test_date = '" + fromDate + "'";
                else if(pcrDayNumber.Equals("8"))
                    whereCond += $" and sc.8day_pcr_test_date = '" + fromDate + "'";
                else
                    whereCond += $" and sc.11day_pcr_test_date = '" + fromDate + "'";

                if(dateSearchType.Equals("allocated"))
                {
                    if(toDate != "01-01-0001")
                        whereCond += $" and sc.team_allocated_date between '" + fromDate + "' and '" + toDate + "'";
                    else
                        whereCond += $" and sc.team_allocated_date = '" + fromDate + "'";
                }

                if (!string.IsNullOrEmpty(companyId))
                    whereCond += " and p.company_id = '" + companyId + "'";

                if(!string.IsNullOrEmpty(teamUserName))
                    whereCond += " and ((sc.allocated_team_name = '" + teamUserName + "' and sc.reallocated_team_name = '')" +
                                 " or (sc.allocated_team_name != '' and sc.reallocated_team_name = '" + teamUserName + "'))";

                if (!string.IsNullOrEmpty(serviceStatus) && serviceStatus != "all")
                {
                    if(pcrDayNumber.Equals("4"))
                        whereCond += " and sc.4day_pcr_test_result = '" + serviceStatus + "'";
                    else if(pcrDayNumber.Equals("6"))
                        whereCond += " and sc.6day_pcr_test_result = '" + serviceStatus + "'";
                    else if(pcrDayNumber.Equals("8"))
                        whereCond += " and sc.8day_pcr_test_result = '" + serviceStatus + "'";
                    else
                        whereCond += " and sc.11day_pcr_test_result = '" + serviceStatus + "'";
                }

                if (!string.IsNullOrEmpty(callStatus) && callStatus != "all")
                {//pending, visited, notvisited
                    if(pcrDayNumber.Equals("4"))
                        whereCond += " and sc.4day_pcr_team_status = '" + callStatus + "'";
                    else if(pcrDayNumber.Equals("6"))
                        whereCond += " and sc.6day_pcr_team_status = '" + callStatus + "'";
                    else if(pcrDayNumber.Equals("8"))
                        whereCond += " and sc.8day_pcr_team_status = '" + callStatus + "'";
                    else
                        whereCond += " and sc.11day_pcr_team_status = '" + callStatus + "'";
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
        public async Task<List<DrNurseCallDetails>> GetTrackerStickerCallDetails(string companyId, string teamUserName, bool isTracker, DateTime scheduledFromDate, DateTime scheduledToDate, string callStatus, string serviceStatus, string dateSearchType, string areaNames)
        {
            List<DrNurseCallDetails> retDrNurseCallDetails = new List<DrNurseCallDetails>();
            try
            {
                var tableName = $"HC_Staff_Patient.patient_obj p, " +
                                $"HC_Master_Details.company_obj co, " +
                                $"HC_Treatment.scheduled_obj sc, " +
                                $"HC_Master_Details.city_obj ci, HC_Master_Details.nationality_obj n, " +
                                $"HC_Master_Details.request_crm_obj rc";

                var ColumAssign = $"p.patient_id as PatientId, p.patient_name as PatientName, " +
                                  $"p.company_id as CompanyId, co.company_name as CompanyName, " +
                                  $"p.request_id as RequestId, rc.request_crm_name as RequestCrmName, " +
                                  $"p.crm_no as CRMNo, p.eid_no as EIDNo, " +
                                  $"p.area as Area, p.age as Age, " +
                                  $"sc.allocated_team_name as AllocatedTeamName, sc.team_allocated_date as AllocatedDate, " +
                                  $"sc.reallocated_team_name as ReAllocatedTeamName, sc.team_reallocated_date as ReAllocatedDate, " +
                                  $"p.city_id as CityId, ci.city_name as CityName, " +
                                  $"p.nationality_id as NationalityId, n.country_name as NationalityName, " +
                                  $"p.mobile_no as MobileNo, sc.scheduled_id as ScheduledId, " +
                                  $"false as IsPCRCall, false as ShowDischage, ";

                ColumAssign +=  $"sc.tracker_replace_date as TrackerReplacedDate, " +
                                $"sc.tracker_replace_no as TrackerReplaceNumber, " +
                                $"sc.tracker_replace_team_user_name as TrackerReplaceTeamUserName, " +
                                $"sc.tracker_replace_team_status as TrackerReplaceTeamStatus, " +
                                $"sc.tracker_replace_team_remark as TrackerReplaceTeamRemark, " +
                                $"sc.tracker_replace_team_date as TrackerReplaceTeamStatusDate, ";

                if(isTracker)
                    ColumAssign += $"'tracker' as CallId, sc.tracker_schedule_date as CallScheduledDate, " +
                                $"sc.tracker_applied_date as CalledDate, sc.sticker_tracker_no as Remarks, " +
                                $"sc.sticker_tracker_result as CallStatus, " +
                                $"sc.tracker_team_user_name as TeamUserName, " +
                                $"sc.tracker_team_status as TeamStatus, " +
                                $"sc.tracker_team_remark as TeamRemark, " +
                                $"sc.tracker_team_date as TeamStatusDate";
                else
                    ColumAssign += $"'sticker' as CallId, sc.sticker_schedule_date as CallScheduledDate, " +
                                $"sc.sticker_removed_date as CalledDate, sc.sticker_tracker_no as Remarks, " +
                                $"sc.sticker_tracker_result as CallStatus, " +
                                $"sc.sticker_team_user_name as TeamUserName, " +
                                $"sc.sticker_team_status as TeamStatus, " +
                                $"sc.sticker_team_remark as TeamRemark, " +
                                $"sc.sticker_team_date as TeamStatusDate";

                var whereCond = $" where p.company_id = co.company_id" +
                                $" and p.patient_id = sc.patient_id" +
                                $" and p.city_id = ci.city_id" +
                                $" and p.nationality_id = n.nationality_id" +
                                $" and p.status = 'Active'" +
                                $" and sc.status = 'Active'" +
                                $" and p.reception_status != 'closed'" +
                                $" and p.request_id = rc.request_crm_id";

                if(!String.IsNullOrEmpty(areaNames) && areaNames.ToLower() != "all")
                {
                    string[] areaArray = areaNames.Replace("[","").Replace("]","").Replace("\"","").Split(',');

                    for(int i = 0; i < areaArray.Count(); i++)
                    {
                        if(i == 0 && areaArray.Count() > 1)
                            whereCond += " and (p.area = '" + areaArray[i] + "'";
                        else if(i == 0)
                            whereCond += " and p.area = '" + areaArray[i] + "'";
                        else if(i == areaArray.Count()-1)
                            whereCond += " or p.area = '" + areaArray[i] + "')";
                        else 
                            whereCond += " or p.area = '" + areaArray[i] + "'";
                    }
                }

                string fromDate = scheduledFromDate.Date.ToString("dd-MM-yyyy");
                if(fromDate == "01-01-0001")
                {
                    scheduledFromDate = DateTime.Today;
                    fromDate = scheduledFromDate.ToString("yyyy-MM-dd 00:00:00.0");
                }
                else
                    fromDate = scheduledFromDate.ToString("yyyy-MM-dd 00:00:00.0");

                string toDate = scheduledToDate.Date.ToString("dd-MM-yyyy");
                if(toDate != "01-01-0001")
                {
                    toDate = scheduledToDate.ToString("yyyy-MM-dd 00:00:00.0");
                    if(isTracker)
                        whereCond += $" and sc.tracker_schedule_date between '" + fromDate + "' and '" + toDate + "'";
                    else
                        whereCond += $" and sc.sticker_schedule_date between '" + fromDate + "' and '" + toDate + "'";
                }
                else if(isTracker)
                    whereCond += $" and sc.tracker_schedule_date = '" + fromDate + "'";
                else
                    whereCond += $" and sc.sticker_schedule_date = '" + fromDate + "'";

                if(dateSearchType.Equals("allocated"))
                {
                    if(toDate != "01-01-0001")
                        whereCond += $" and sc.team_allocated_date between '" + fromDate + "' and '" + toDate + "'";
                    else
                        whereCond += $" and sc.team_allocated_date = '" + fromDate + "'";
                }

                if (!string.IsNullOrEmpty(companyId))
                    whereCond += " and p.company_id = '" + companyId + "'";

                if(!string.IsNullOrEmpty(teamUserName))
                    whereCond += " and ((sc.allocated_team_name = '" + teamUserName + "' and sc.reallocated_team_name = '')" +
                                 " or (sc.allocated_team_name != '' and sc.reallocated_team_name = '" + teamUserName + "'))";

                if (!string.IsNullOrEmpty(serviceStatus) && serviceStatus != "all")
                    whereCond += " and sc.sticker_tracker_result = '" + serviceStatus + "'";

                if (!string.IsNullOrEmpty(callStatus) && callStatus != "all")
                {//pending, visited, notvisited
                    if(isTracker)
                        whereCond += " and sc.tracker_team_status = '" + callStatus + "'";
                    else
                        whereCond += " and sc.sticker_team_status = '" + callStatus + "'";
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
        public async Task<List<DrNurseCallDetails>> GetDischargeCallDetails(string companyId, string teamUserName, DateTime scheduledFromDate, DateTime scheduledToDate, string callStatus, string serviceStatus, string dateSearchType, string areaNames)
        {
            List<DrNurseCallDetails> retDrNurseCallDetails = new List<DrNurseCallDetails>();
            try
            {
                var tableName = $"HC_Staff_Patient.patient_obj p, " +
                                $"HC_Master_Details.company_obj co, " +
                                $"HC_Treatment.scheduled_obj sc, " +
                                $"HC_Master_Details.city_obj ci, HC_Master_Details.nationality_obj n, " +
                                $"HC_Master_Details.request_crm_obj rc";

                var ColumAssign = $"p.patient_id as PatientId, p.patient_name as PatientName, " +
                                  $"p.company_id as CompanyId, co.company_name as CompanyName, " +
                                  $"p.request_id as RequestId, rc.request_crm_name as RequestCrmName, " +
                                  $"p.crm_no as CRMNo, p.eid_no as EIDNo, " +
                                  $"p.area as Area, p.age as Age, " +
                                  $"sc.allocated_team_name as AllocatedTeamName, sc.team_allocated_date as AllocatedDate, " +
                                  $"sc.reallocated_team_name as ReAllocatedTeamName, sc.team_reallocated_date as ReAllocatedDate, " +
                                  $"p.city_id as CityId, ci.city_name as CityName, " +
                                  $"p.nationality_id as NationalityId, n.country_name as NationalityName, " +
                                  $"p.mobile_no as MobileNo, sc.scheduled_id as ScheduledId, " +
                                  $"false as IsPCRCall, true as ShowDischage, " +
                                  $"'discharge' as CallId, sc.discharge_date as CallScheduledDate, " +
                                  $"sc.discharge_remarks as Remarks, " +
                                  $"sc.discharge_status as CallStatus, " +
                                  $"sc.discharge_team_user_name as TeamUserName, " +
                                  $"sc.discharge_team_status as TeamStatus, " +
                                  $"sc.discharge_team_remark as TeamRemark, " +
                                  $"sc.discharge_team_date as TeamStatusDate";

                var whereCond = $" where p.company_id = co.company_id" +
                                $" and p.patient_id = sc.patient_id" +
                                $" and p.city_id = ci.city_id" +
                                $" and p.nationality_id = n.nationality_id" +
                                $" and p.status = 'Active'" +
                                $" and sc.status = 'Active'" +
                                $" and p.reception_status != 'closed'" +
                                $" and p.request_id = rc.request_crm_id";

                if(!String.IsNullOrEmpty(areaNames) && areaNames.ToLower() != "all")
                {
                    string[] areaArray = areaNames.Replace("[","").Replace("]","").Replace("\"","").Split(',');

                    for(int i = 0; i < areaArray.Count(); i++)
                    {
                        if(i == 0 && areaArray.Count() > 1)
                            whereCond += " and (p.area = '" + areaArray[i] + "'";
                        else if(i == 0)
                            whereCond += " and p.area = '" + areaArray[i] + "'";
                        else if(i == areaArray.Count()-1)
                            whereCond += " or p.area = '" + areaArray[i] + "')";
                        else 
                            whereCond += " or p.area = '" + areaArray[i] + "'";
                    }
                }

                string fromDate = scheduledFromDate.Date.ToString("dd-MM-yyyy");
                if(fromDate == "01-01-0001")
                {
                    scheduledFromDate = DateTime.Today;
                    fromDate = scheduledFromDate.ToString("yyyy-MM-dd 00:00:00.0");
                }
                else
                    fromDate = scheduledFromDate.ToString("yyyy-MM-dd 00:00:00.0");

                string toDate = scheduledToDate.Date.ToString("dd-MM-yyyy");
                if(toDate != "01-01-0001")
                {
                    toDate = scheduledToDate.ToString("yyyy-MM-dd 00:00:00.0");
                    whereCond += $" and sc.discharge_date between '" + fromDate + "' and '" + toDate + "'";
                }
                else
                    whereCond += $" and sc.discharge_date = '" + fromDate + "'";

                if(dateSearchType.Equals("allocated"))
                {
                    if(toDate != "01-01-0001")
                        whereCond += $" and sc.team_allocated_date between '" + fromDate + "' and '" + toDate + "'";
                    else
                        whereCond += $" and sc.team_allocated_date = '" + fromDate + "'";
                }

                if (!string.IsNullOrEmpty(companyId))
                    whereCond += " and p.company_id = '" + companyId + "'";

                if(!string.IsNullOrEmpty(teamUserName))
                    whereCond += " and ((sc.allocated_team_name = '" + teamUserName + "' and sc.reallocated_team_name = '')" +
                                 " or (sc.allocated_team_name != '' and sc.reallocated_team_name = '" + teamUserName + "'))";

                if (!string.IsNullOrEmpty(serviceStatus) && serviceStatus != "all")
                    whereCond += " and p.discharge_status = '" + serviceStatus + "'";

                //pending, visited, notvisited
                if (!string.IsNullOrEmpty(callStatus) && callStatus != "all")
                    whereCond += " and sc.discharge_team_status = '" + callStatus + "'";

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


        public async Task<List<DrNurseCallDetails>> GetAllocatedDateDetails(string companyId, string teamUserName, DateTime scheduledFromDate, DateTime scheduledToDate)
        {
            List<DrNurseCallDetails> retDrNurseCallDetails = new List<DrNurseCallDetails>();
            try
            {
                var tableName = $"HC_Staff_Patient.patient_obj p, " +
                                $"HC_Master_Details.company_obj co, " +
                                $"HC_Treatment.scheduled_obj sc, " +
                                $"HC_Master_Details.city_obj ci, HC_Master_Details.nationality_obj n, " +
                                $"HC_Master_Details.request_crm_obj rc";

                var ColumAssign = $"p.patient_id as PatientId, p.patient_name as PatientName, " +
                                  $"p.company_id as CompanyId, co.company_name as CompanyName, " +
                                  $"p.request_id as RequestId, rc.request_crm_name as RequestCrmName, " +
                                  $"p.crm_no as CRMNo, p.eid_no as EIDNo, " +
                                  $"p.area as Area, p.age as Age, " +
                                  $"sc.allocated_team_name as AllocatedTeamName, sc.team_allocated_date as AllocatedDate, " +
                                  $"sc.reallocated_team_name as ReAllocatedTeamName, sc.team_reallocated_date as ReAllocatedDate, " +
                                  $"p.city_id as CityId, ci.city_name as CityName, " +
                                  $"p.nationality_id as NationalityId, n.country_name as NationalityName, " +
                                  $"p.mobile_no as MobileNo, sc.scheduled_id as ScheduledId, " +
                                  $"false as IsPCRCall, " +
                                  $"'allocated' as CallId, sc.team_allocated_date as CallScheduledDate, " +
                                  $"'Team Assigned Date' as Remarks, " +
                                  $"'' as CallStatus";

                var whereCond = $" where p.company_id = co.company_id" +
                                $" and p.patient_id = sc.patient_id" +
                                $" and p.city_id = ci.city_id" +
                                $" and p.nationality_id = n.nationality_id" +
                                $" and p.status = 'Active'" +
                                $" and sc.status = 'Active'" +
                                $" and p.reception_status != 'closed'" +
                                $" and p.request_id = rc.request_crm_id";

                string fromDate = scheduledFromDate.Date.ToString("dd-MM-yyyy");
                if(fromDate == "01-01-0001")
                {
                    scheduledFromDate = DateTime.Today;
                    fromDate = scheduledFromDate.ToString("yyyy-MM-dd 00:00:00.0");
                }
                else
                    fromDate = scheduledFromDate.ToString("yyyy-MM-dd 00:00:00.0");

                string toDate = scheduledToDate.Date.ToString("dd-MM-yyyy");
                if(toDate != "01-01-0001")
                {
                    toDate = scheduledToDate.ToString("yyyy-MM-dd 00:00:00.0");
                    whereCond += $" and sc.team_allocated_date between '" + fromDate + "' and '" + toDate + "'";
                }
                else
                    whereCond += $" and sc.team_allocated_date = '" + fromDate + "'";

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


        public async Task<bool> EditPCRCall(CallRequest callRequest)
        {
            try
            {
                bool sqlResult = true;
                var tableName = $"HC_Treatment.scheduled_obj";

                var colName = $"scheduled_id = @ScheduledId, " +
                              $"modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                if(!String.IsNullOrEmpty(callRequest.CallId))
                {
                    if(callRequest.CallId.ToLower().Equals("4thday"))
                    {
                        colName += $", 4day_pcr_test_sample_date = @CalledDate, " +
                                   $"4day_pcr_test_result = @CallStatus, " +
                                   $"4day_pcr_team_user_name = @TeamUserName, " +
                                   $"4day_pcr_team_status = @TeamStatus, " +
                                   $"4day_pcr_team_remark = @TeamRemark, " +
                                   $"4day_pcr_team_date = @TeamStatusDate";
                    }
                    else if(callRequest.CallId.ToLower().Equals("6thday"))
                    {
                        colName += $", 6day_pcr_test_sample_date = @CalledDate, " +
                                   $"6day_pcr_test_result = @CallStatus, " +
                                   $"6day_pcr_team_user_name = @TeamUserName, " +
                                   $"6day_pcr_team_status = @TeamStatus, " +
                                   $"6day_pcr_team_remark = @TeamRemark, " +
                                   $"6day_pcr_team_date = @TeamStatusDate";
                    }
                    else if(callRequest.CallId.ToLower().Equals("8thday"))
                    {
                        colName += $", 8day_pcr_test_sample_date = @CalledDate, " +
                                   $"8day_pcr_test_result = @CallStatus, " +
                                   $"8day_pcr_team_user_name = @TeamUserName, " +
                                   $"8day_pcr_team_status = @TeamStatus, " +
                                   $"8day_pcr_team_remark = @TeamRemark, " +
                                   $"8day_pcr_team_date = @TeamStatusDate";
                    }
                    else if(callRequest.CallId.ToLower().Equals("11thday"))
                    {
                        colName += $", 11day_pcr_test_sample_date = @CalledDate, " +
                                   $"11day_pcr_test_result = @CallStatus, " +
                                   $"11day_pcr_team_user_name = @TeamUserName, " +
                                   $"11day_pcr_team_status = @TeamStatus, " +
                                   $"11day_pcr_team_remark = @TeamRemark, " +
                                   $"11day_pcr_team_date = @TeamStatusDate";
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

                string teamStatusDate;
                if(callRequest.TeamStatusDate == null)
                    teamStatusDate = "";
                else
                {
                    teamStatusDate = callRequest.TeamStatusDate.ToString("yyyy-MM-dd");
                    if( teamStatusDate == "0001-01-01")
                        teamStatusDate = "";
                    else
                        teamStatusDate = callRequest.TeamStatusDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                object colValueParam = new
                {
                    ScheduledId = callRequest.ScheduledId,
                    CalledDate = calledDate,
                    CallStatus = callRequest.CallStatus,
                    TeamUserName = callRequest.TeamUserName,
                    TeamStatus = callRequest.TeamStatus,
                    TeamRemark = callRequest.TeamRemark,
                    TeamStatusDate = teamStatusDate,
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
        public async Task<bool> EditStickerTrackerDischargeCall(CallRequest callRequest)
        {
            try
            {
                bool sqlResult = true;
                var tableName = $"HC_Treatment.scheduled_obj";

                var colName = $"scheduled_id = @ScheduledId, " +
                              $"remarks = @Remarks, " +//emr_done = @EMRDone, is_pcr = @IsPCRCall," +
                              $"modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                if(!String.IsNullOrEmpty(callRequest.CallId))
                {
                    if(callRequest.CallId.ToLower().Equals("tracker"))
                    {
                        colName += $", tracker_applied_date = @CalledDate, " +
                                   $"sticker_tracker_result = @CallStatus, " +
                                   $"tracker_team_user_name = @TeamUserName, " +
                                   $"tracker_team_status = @TeamStatus, " +
                                   $"tracker_team_remark = @TeamRemark, " +
                                   $"tracker_team_date = @TeamStatusDate";
                    }
                    else if(callRequest.CallId.ToLower().Equals("sticker"))
                    {
                        colName += $", sticker_removed_date = @CalledDate, " +
                                   $"sticker_tracker_result = @CallStatus, " +
                                   $"sticker_team_user_name = @TeamUserName, " +
                                   $"sticker_team_status = @TeamStatus, " +
                                   $"sticker_team_remark = @TeamRemark, " +
                                   $"sticker_team_date = @TeamStatusDate";
                    }
                    else if(callRequest.CallId.ToLower().Equals("discharge"))
                    {
                        colName += $", discharge_remarks = @Remarks, " +
                                   $"discharge_status = @CallStatus, " +
                                   $"discharge_team_user_name = @TeamUserName, " +
                                   $"discharge_team_status = @TeamStatus, " +
                                   $"discharge_team_remark = @TeamRemark, " +
                                   $"discharge_team_date = @TeamStatusDate";
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

                string teamStatusDate;
                if(callRequest.TeamStatusDate == null)
                    teamStatusDate = "";
                else
                {
                    teamStatusDate = callRequest.TeamStatusDate.ToString("yyyy-MM-dd");
                    if( teamStatusDate == "0001-01-01")
                        teamStatusDate = "";
                    else
                        teamStatusDate = callRequest.TeamStatusDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                object colValueParam = new
                {
                    ScheduledId = callRequest.ScheduledId,
                    CalledDate = calledDate,
                    CallStatus = callRequest.CallStatus,
                    Remarks = callRequest.Remarks,
                    TeamUserName = callRequest.TeamUserName,
                    TeamStatus = callRequest.TeamStatus,
                    TeamRemark = callRequest.TeamRemark,
                    TeamStatusDate = teamStatusDate,
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
            bool retVal = true;
            try
            {
                await EditPatientEnrollmentDetails(servicePlanRequest);
                if(!String.IsNullOrEmpty(servicePlanRequest.ServiceName))
                {
                    if(servicePlanRequest.ServiceName.ToLower().Equals("sticker"))
                        retVal = await EditScheduleStickerDetails(servicePlanRequest);
                    else if(servicePlanRequest.ServiceName.ToLower().Equals("tracker"))
                        retVal = await EditScheduleTrackerDetails(servicePlanRequest);
                    else if(servicePlanRequest.ServiceName.ToLower().Equals("discharge"))
                        retVal = await EditScheduleTrackerDetails(servicePlanRequest);
                    else
                        retVal = await EditSchedulePCRDetails(servicePlanRequest);
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
                retVal = false;
            }
            return retVal;
        }
        public async Task<bool> EditPatientEnrollmentDetails(ServicePlanRequest servicePlanRequest)
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
        public async Task<bool> EditScheduleStickerDetails(ServicePlanRequest servicePlanRequest)
        {
            try
            {
                bool sqlResult = true;
                var tableName = $"HC_Staff_Patient.scheduled_obj";

                var colName = $"sticker_removed_date = @StickerRemovedDate, " +
                              $"sticker_tracker_no = @StickerTrackerNumber, " +
                              $"sticker_tracker_result = @StickerTrackerResult, " +
                              $"tracker_replace_date = @TrackerReplacedDate, tracker_replace_no = @TrackerReplaceNumber, " +
                              $"modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                var whereCond = $" where patient_id = @PatientId and scheduled_id = @ScheduledId";
                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

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
                    StickerRemovedDate = removedDate,
                    StickerTrackerNumber = servicePlanRequest.StickerTrackerNumber,
                    TrackerReplacedDate = replacedDate,
                    TrackerReplaceNumber = servicePlanRequest.TrackerReplaceNumber,
                    StickerTrackerResult = servicePlanRequest.StickerTrackerResult,
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
        public async Task<bool> EditScheduleTrackerDetails(ServicePlanRequest servicePlanRequest)
        {
            try
            {
                bool sqlResult = true;
                var tableName = $"HC_Staff_Patient.scheduled_obj";

                var colName = $"tracker_applied_date = @TrackerAppliedDate, " +
                              $"sticker_tracker_no = @StickerTrackerNumber, " +
                              $"sticker_tracker_result = @StickerTrackerResult, " +
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
                    TrackerAppliedDate = appliedDate,
                    StickerTrackerNumber = servicePlanRequest.StickerTrackerNumber,
                    TrackerReplacedDate = replacedDate,
                    TrackerReplaceNumber = servicePlanRequest.TrackerReplaceNumber,
                    StickerTrackerResult = servicePlanRequest.StickerTrackerResult,
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
        public async Task<bool> EditScheduleDischargeDetails(ServicePlanRequest servicePlanRequest)
        {
            try
            {
                bool sqlResult = true;
                var tableName = $"HC_Treatment.scheduled_obj";

                var colName = $"discharge_remarks = @DischargeRemarks, " +
                              $"discharge_status = @DischargeStatus, " +
                              $"modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                var whereCond = $" where patient_id = @PatientId and scheduled_id = @ScheduledId";

                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                object colValueParam = new
                {
                    PatientId = servicePlanRequest.PatientId,
                    ScheduledId = servicePlanRequest.ScheduledId,
                    DischargeStatus = servicePlanRequest.DischargeStatus,
                    DischargeRemarks = servicePlanRequest.DischargeRemarks,
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
        public async Task<bool> EditSchedulePCRDetails(ServicePlanRequest servicePlanRequest)
        {
            try
            {
                bool sqlResult = true;
                var tableName = $"HC_Treatment.scheduled_obj";

                var colName = $"modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                if(servicePlanRequest.ServiceName.ToLower().Equals("4thday"))
                {
                    colName += $", 4day_pcr_test_sample_date = @PCRSampleDate, " +
                                $"4day_pcr_test_result = @PCRResult";
                }
                else if(servicePlanRequest.ServiceName.ToLower().Equals("6thday"))
                {
                    colName += $", 6day_pcr_test_sample_date = @PCRSampleDate, " +
                                $"6day_pcr_test_result = @PCRResult";
                }
                else if(servicePlanRequest.ServiceName.ToLower().Equals("8thday"))
                {
                    colName += $", 8day_pcr_test_sample_date = @PCRSampleDate, " +
                                $"8day_pcr_test_result = @PCRResult";
                }
                else if(servicePlanRequest.ServiceName.ToLower().Equals("11thday"))
                {
                    colName += $", 11day_pcr_test_sample_date = @PCRSampleDate, " +
                                $"11day_pcr_test_result = @PCRResult";
                }

                var whereCond = $" where patient_id = @PatientId and scheduled_id = @ScheduledId";

                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                string sampleDate;
                if(servicePlanRequest.PCRSampleDate == null)
                    sampleDate = "";
                else
                {
                    sampleDate = servicePlanRequest.PCRSampleDate.ToString("yyyy-MM-dd");
                    if( sampleDate == "0001-01-01")
                        sampleDate = "";
                    else
                        sampleDate = servicePlanRequest.PCRSampleDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                object colValueParam = new
                {
                    PatientId = servicePlanRequest.PatientId,
                    ScheduledId = servicePlanRequest.ScheduledId,
                    PCRSampleDate = sampleDate,
                    PCRResult = servicePlanRequest.PCRResult,
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

        public async Task<bool> EditTeamVisitDetails(TeamVisitDetails teamVisitDetails)
        {
            bool retVal = true;
            try
            {
                var tableName = $"HC_Treatment.scheduled_obj";

                var colName = $"modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                if(!String.IsNullOrEmpty(teamVisitDetails.ServiceName))
                {
                    if(teamVisitDetails.ServiceName.ToLower().Equals("4thday"))
                    {
                        colName += $", 4day_pcr_team_user_name = @TeamUserName, " +
                                   $"4day_pcr_team_status = @TeamStatus, " +
                                   $"4day_pcr_team_remark = @TeamRemark, " +
                                   $"4day_pcr_team_date = @TeamStatusDate";
                    }
                    else if(teamVisitDetails.ServiceName.ToLower().Equals("6thday"))
                    {
                        colName += $", 6day_pcr_team_user_name = @TeamUserName, " +
                                   $"6day_pcr_team_status = @TeamStatus, " +
                                   $"6day_pcr_team_remark = @TeamRemark, " +
                                   $"6day_pcr_team_date = @TeamStatusDate";
                    }
                    else if(teamVisitDetails.ServiceName.ToLower().Equals("8thday"))
                    {
                        colName += $", 8day_pcr_team_user_name = @TeamUserName, " +
                                   $"8day_pcr_team_status = @TeamStatus, " +
                                   $"8day_pcr_team_remark = @TeamRemark, " +
                                   $"8day_pcr_team_date = @TeamStatusDate";
                    }
                    else if(teamVisitDetails.ServiceName.ToLower().Equals("11thday"))
                    {
                        colName += $", 11day_pcr_team_user_name = @TeamUserName, " +
                                   $"11day_pcr_team_status = @TeamStatus, " +
                                   $"11day_pcr_team_remark = @TeamRemark, " +
                                   $"11day_pcr_team_date = @TeamStatusDate";
                    }
                    else if(teamVisitDetails.ServiceName.ToLower().Equals("tracker"))
                    { 
                        colName += $", tracker_team_user_name = @TeamUserName, " +
                                   $"tracker_team_status = @TeamStatus, " +
                                   $"tracker_team_remark = @TeamRemark, " +
                                   $"tracker_team_date = @TeamStatusDate";
                    }
                    else if(teamVisitDetails.ServiceName.ToLower().Equals("sticker"))
                    {
                        colName += $", sticker_team_user_name = @TeamUserName, " +
                                   $"sticker_team_status = @TeamStatus, " +
                                   $"sticker_team_remark = @TeamRemark, " +
                                   $"sticker_team_date = @TeamStatusDate";
                    }
                    if(teamVisitDetails.ServiceName.ToLower().Equals("discharge") || teamVisitDetails.ShowDischage)
                    {
                        colName += $", discharge_team_user_name = @TeamUserName, " +
                                   $"discharge_team_status = @TeamStatus, " +
                                   $"discharge_team_remark = @TeamRemark, " +
                                   $"discharge_team_date = @TeamStatusDate";
                    }
                    if(teamVisitDetails.ServiceName.ToLower().Equals("tracker") ||
                        teamVisitDetails.ServiceName.ToLower().Equals("sticker"))
                    {
                        colName += $", tracker_replace_team_user_name = @TeamUserName, " +
                                   $"tracker_replace_team_status = @TeamStatus, " +
                                   $"tracker_replace_team_remark = @TeamRemark, " +
                                   $"tracker_replace_team_date = @TeamStatusDate";
                    }
                }

                var whereCond = $" where patient_id = @PatientId and scheduled_id = @ScheduledId";

                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                string teamStatusDate;
                if(teamVisitDetails.TeamStatusDate == null)
                    teamStatusDate = "";
                else
                {
                    teamStatusDate = teamVisitDetails.TeamStatusDate.ToString("yyyy-MM-dd");
                    if( teamStatusDate == "0001-01-01")
                        teamStatusDate = "";
                    else
                        teamStatusDate = teamVisitDetails.TeamStatusDate.ToString("yyyy-MM-dd 00:00:00.0");
                }

                object colValueParam = new
                {
                    PatientId = teamVisitDetails.PatientId,
                    ScheduledId = teamVisitDetails.ScheduledId,
                    TeamUserName = teamVisitDetails.TeamUserName,
                    TeamStatus = teamVisitDetails.TeamStatus,
                    TeamRemark = teamVisitDetails.TeamRemark,
                    TeamStatusDate = teamStatusDate,
                    ModifiedBy = teamVisitDetails.ModifiedBy,
                    ModifiedOn = DateTime.Today.ToString("yyyy-MM-dd 00:00:00.0")
                };

                using (var connection = _appDbContext.Connection)
                {
                    retVal = Convert.ToBoolean(await connection.ExecuteAsync(sqlUpdateQuery, colValueParam));
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
                retVal = false;
            }
            return retVal;
        }
    }
}