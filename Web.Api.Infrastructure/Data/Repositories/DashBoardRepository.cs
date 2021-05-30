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
    internal sealed class DashBoardRepository : IDashBoardRepository
    {
        private readonly AppDbContext _appDbContext;
        public DashBoardRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<DashBoardDetails> GetDashBoardDetails(string companyId)
        {
            DashBoardDetails retDashBoardDetails = new DashBoardDetails();
            try
            {
                retDashBoardDetails.TotalPatientNumber = 0;
                retDashBoardDetails.DischargePatientNumber = 0;
                retDashBoardDetails.CurrentPatientNumber = 0;
                retDashBoardDetails.TodayPatientRegNumber = 0;
                retDashBoardDetails.TotalUserTypeMemberNumber = 0;
                retDashBoardDetails.TotalTeamNumber = 10;
                return retDashBoardDetails;
            }
            catch (Exception Err)
            {
                var Error = Err.Message.ToString();
                return null;
            }
        }
    }
}