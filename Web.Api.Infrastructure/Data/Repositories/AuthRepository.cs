using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Dto.UseCaseRequests;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using Web.Api.Core.Enums;
using Dapper;

namespace Web.Api.Infrastructure.Data.Repositories
{
    internal sealed class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _appDbContext;
        public AuthRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        /// <summary>
        /// Check whether username is present or not
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns>userId</returns>
        public async Task<string> CheckUsername(LoginRequest loginRequest)
        {
            try
            {
                var tableName = $"HC_Authentication.user_obj";

                var whereCond = $" where ";
                if(!string.IsNullOrEmpty(loginRequest.UserName))
                    whereCond += "user_name = '" + loginRequest.UserName + "'";

                if(!string.IsNullOrEmpty(loginRequest.Password))
                {
                    //var encryptPassword = EncryptWithSalt(loginRequest.Password, loginRequest.UserName);
                    var encryptPassword = loginRequest.Password;
            
                    if(!string.IsNullOrEmpty(loginRequest.UserName))
                        whereCond += " and password = '" + encryptPassword + "'";
                    else
                        whereCond += "password = '" + encryptPassword + "'";
                }

                var sqlSelQuery = $"select user_id from " + tableName + whereCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelResult = await connection.QueryAsync<string>(sqlSelQuery);
                    return sqlSelResult.FirstOrDefault();
                }
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
                return "";
            }
        }
        public string EncryptWithSalt(string text, string salt)
        {
            string hash;
            using (var sha512 = SHA512.Create())
            {
                var hashedBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(text));
                var hashedBytesAndSalt = sha512.ComputeHash(Encoding.UTF8.GetBytes($"{salt}{BitConverter.ToString(hashedBytes).Replace("-", "").ToLower()}"));
                hash = BitConverter.ToString(hashedBytesAndSalt).Replace("-", "").ToLower();
            }
            return hash;
        }
        public async Task<LoginUserDetails> GetLoginUserDetails(string userId)
        {
            LoginUserDetails retLoginUserDetails = new LoginUserDetails();
            try
            {
                var tableName = $"HC_Authentication.user_obj u, HC_Master_Details.company_obj c";

                var ColumAssign = $"u.user_id  as UserId, u.user_name as UserName, " +
                                   $"u.user_type as UserType, u.company_id as CompanyId, " +
                                   $"c.company_name as CompanyName";

                var whereCond = $" where u.company_id = c.company_id";
                if (!string.IsNullOrEmpty(userId))
                    whereCond += " and u.user_id = '" + userId + "'";
                var sqlSelQuery = $"select " + ColumAssign + " from " + tableName + whereCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelResult = await connection.QueryAsync<LoginUserDetails>(sqlSelQuery);
                    retLoginUserDetails = sqlSelResult.FirstOrDefault();
                }
                return retLoginUserDetails;
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
                return null;
            }
        }
        public async Task<List<UserDetails>> GetUserDetails(string userId)
        {
            List<UserDetails> retUserDetailsList = new List<UserDetails>();
            try
            {
                var tableName = $"HC_Authentication.user_obj u, HC_Master_Details.company_obj c";

                var ColumAssign = $"u.user_id  as UserId, u.user_name as UserName, " +
                                   $"u.password as Password, u.user_type as UserType, " +
                                   $"u.company_id as CompanyId, c.company_name as CompanyName";

                var whereCond = $" where u.company_id = c.company_id";

                if (!string.IsNullOrEmpty(userId))
                    whereCond += " and u.user_id = '" + userId + "'";
                //else
                    //whereCond += " and u.user_type = '" + (int) UserType.SuperAdmin + "'";

                var sqlSelQuery = $"select " + ColumAssign + " from " + tableName + whereCond;
                using (var connection = _appDbContext.Connection)
                {
                    var sqlSelResult = await connection.QueryAsync<UserDetails>(sqlSelQuery);
                    retUserDetailsList = sqlSelResult.ToList();//.FirstOrDefault();
                }
                return retUserDetailsList;
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
                return null;
            }
        }

        public async Task<bool> CheckUserNameAvailability(string userId, string userName)
        {
            using (var connection = _appDbContext.Connection)
            {
                var tableName = $"HC_Authentication.user_obj";
                var whereCond = $" where ";

                if(!string.IsNullOrEmpty(userName))
                    whereCond += $"user_name = @UserName";

                if(!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(userName))
                    whereCond += $" and user_id != @UserId";
                else if(!string.IsNullOrEmpty(userId))
                    whereCond += $"user_id != @UserId";

                object colValueParam = new
                {
                    UserId = userId,
                    UserName = userName
                };
                var sqlSelQuery = $"select * from " + tableName + whereCond;
                var sqlResult = await connection.QueryAsync<string>(sqlSelQuery, colValueParam);
                return sqlResult.ToList().Any();
            }
        }

        public async Task<bool> CreateUser(UserRequest userRequest)
        {
            try
            {
                var uuid = GenerateUUID();
                bool sqlResult = true;
                userRequest.UserId = uuid;

                var colName = $"user_id, full_name, user_name, password, user_type, company_id, created_by, created_on";
                var colValueName = $"@UserId, @FullName, @UserName, @Password, @UserType, @CompanyId, @CreatedBy, @CreatedOn";
                var tableName = $"HC_Authentication.user_obj";

                var sqlInsQuery = $"INSERT INTO "+ tableName + "( " + colName + " )" +
                                    $"VALUES ( " + colValueName + " )";

                //var encryptPassword = EncryptWithSalt(userRequest.Password, userRequest.UserName);
                var encryptPassword = userRequest.Password;
                object colValueParam = new
                {
                    UserId = userRequest.UserId,
                    FullName = userRequest.FullName,
                    UserName = userRequest.UserName,
                    Password = encryptPassword,
                    UserType = userRequest.UserType,
                    CompanyId = userRequest.CompanyId,
                    CreatedBy = userRequest.CreatedBy,
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

        public async Task<bool> EditUser(UserRequest userRequest)
        {
            try
            {
                bool sqlResult = true;
                var tableName = $"HC_Authentication.user_obj";
                var colName = $"user_id = @UserId, full_name = @FullName, user_name = @UserName, password = @Password, " +
                                $"user_type = @UserType, company_id = @CompanyId, modified_by = @ModifiedBy, modified_on = @ModifiedOn";

                var whereCond = $" where user_id = @UserId";
                var sqlUpdateQuery = $"UPDATE "+ tableName + " set " + colName + whereCond;

                //var encryptPassword = EncryptWithSalt(userRequest.Password, userRequest.UserName);
                var encryptPassword = userRequest.Password;
                object colValueParam = new
                {
                    UserId = userRequest.UserId,
                    FullName = userRequest.FullName,
                    UserName = userRequest.UserName,
                    Password = encryptPassword,
                    UserType = userRequest.UserType,
                    CompanyId = userRequest.CompanyId,
                    ModifiedBy = userRequest.ModifiedBy,
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