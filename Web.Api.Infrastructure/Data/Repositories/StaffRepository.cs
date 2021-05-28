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
    internal sealed class StaffRepository : IStaffRepository
    {
        private new readonly AppDbContext _appDbContext;
        public StaffRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<StaffDetails>> GetStaffDetails(string companyId, string staffId)
        {
            List<StaffDetails> retStaffDetailsList = new List<StaffDetails>();
            try
            {
                var tableName = $"HC_Staff_Patient.staff_obj s, HC_Master_Details.company_obj c, " +
                                $"HC_Authentication.user_obj u";

                var ColumAssign = $"s.staff_id as StaffId, s.staff_name as StaffName, " +
                              $"s.staff_type as StaffType, s.address as Address, " +
                              $"s.user_id as UserId, u.user_name as UserName, u.password as Password, " +
                              $"s.mobile_no as MobileNo, s.team_name as TeamName, " +
                              $"s.company_id as CompanyId, c.company_name as CompanyName, " +
                              $"s.created_by as CreatedBy, s.modified_by as ModifiedBy";

                var whereCond = $" where s.company_id = c.company_id and s.user_id = u.user_id";

                if (!string.IsNullOrEmpty(companyId))
                    whereCond += " and s.company_id = '" + companyId + "'";

                if (!string.IsNullOrEmpty(staffId))
                    whereCond += " and s.staff_id = '" + staffId + "'";

                var sqlSelQuery = $"select " + ColumAssign + " from " + tableName + whereCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelResult = await connection.QueryAsync<StaffDetails>(sqlSelQuery);
                    retStaffDetailsList = sqlSelResult.ToList();
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
            }
            return retStaffDetailsList;
        }

        public async Task<bool> CreateStaff(StaffRequest staffRequest)
        {
            CompanyDetails retCompanyDetails = new CompanyDetails();
            try
            {
                var uuid = GenerateUUID();
                bool sqlResult = true;
                staffRequest.StaffId = uuid;

                var tableName = $"HC_Staff_Patient.staff_obj";

                var colName = $"staff_id, staff_name, staff_type, user_id, address, mobile_no, " +
                              $"company_id, team_name, created_by, created_on";

                var colValueName = $"@PatientId, @PatientName, @CompanyId, @RequestId, @CRMNo, @EIDNo, " +
                                   $"@DateOfBirth, @Age, @Sex, @Address, @LandMark, @Area, @CityId, @NationalityId, " +
                                   $"@MobileNo, @GoogleMapLink, @StickerApplication, @StickerRemoval, " +
                                   $"@CreatedBy, @CreatedOn";

                var sqlInsQuery = $"INSERT INTO "+ tableName + "( " + colName + " )" +
                                    $"VALUES ( " + colValueName + " )";

                object colValueParam = new
                {
                    StaffId = staffRequest.StaffId,
                    StaffName = staffRequest.StaffName,
                    StaffType = staffRequest.StaffType,
                    UserId = staffRequest.UserId,
                    Address = staffRequest.Address,
                    MobileNo = staffRequest.MobileNo,
                    CompanyId = staffRequest.CompanyId,
                    TeamName = staffRequest.TeamName,
                    CreatedBy = staffRequest.CreatedBy,
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
        public async Task<bool> EditStaff(StaffRequest staffRequest)
        {
            try
            {
                bool sqlResult = true;
                var tableName = $"HC_Staff_Patient.staff_obj";

                var colName = $"staff_id = @StaffId, staff_name = @StaffName, " +
                              $"staff_type = @StaffType, user_id = @UserId, address = @Address, " +
                              $"mobile_no = @MobileNo, company_id = @CompanyId, team_name = @TeamName, " +
                              $"modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                var whereCond = $" where staff_id = @StaffId";
                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                object colValueParam = new
                {
                    StaffId = staffRequest.StaffId,
                    StaffName = staffRequest.StaffName,
                    StaffType = staffRequest.StaffType,
                    UserId = staffRequest.UserId,
                    Address = staffRequest.Address,
                    MobileNo = staffRequest.MobileNo,
                    CompanyId = staffRequest.CompanyId,
                    TeamName = staffRequest.TeamName,
                    ModifiedBy = staffRequest.ModifiedBy,
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

    }
}