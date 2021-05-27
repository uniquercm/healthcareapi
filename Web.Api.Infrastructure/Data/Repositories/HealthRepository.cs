using System;
using System.Threading.Tasks;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Dapper;

namespace Web.Api.Infrastructure.Data.Repositories
{
    internal sealed class HealthRepository : IHealthRepository
    {
        private readonly AppDbContext _appDbContext;
        public HealthRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        /// <summary>
        /// Check the database health
        /// </summary>
        /// <returns>true</returns>
        public async Task<bool> CheckDBHealth()
        {
            using (var connection = _appDbContext.Connection)
            {
                var sqlQuery = $"select 5";
                Int16 sql = await connection.QuerySingleOrDefaultAsync<Int16>(sqlQuery);
                return sql.Equals(5) ? true : false;
            }


        }
    }
}
