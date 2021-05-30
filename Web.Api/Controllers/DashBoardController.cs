using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Models.Settings;
using Web.Api.Presenters;
using AutoMapper;

namespace Web.Api.Controllers
{
    [Route("v1")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        private readonly IDashBoardUseCases _dashBoardUseCases;
        private readonly AcknowledgementPresenter _acknowledgementPresenter;
        private readonly GetDetailsPresenter _getDetailsPresenter;
        private readonly AvailabilityPresenter _availabilityPresenter;
        private readonly IMapper _mapper;

        public DashBoardController(IDashBoardUseCases dashBoardUseCases, AcknowledgementPresenter acknowledgementPresenter, GetDetailsPresenter getDetailsPresenter, AvailabilityPresenter availabilityPresenter, IMapper mapper)
        {   
            _dashBoardUseCases = dashBoardUseCases;
            _acknowledgementPresenter = acknowledgementPresenter;
            _getDetailsPresenter = getDetailsPresenter;
            _availabilityPresenter = availabilityPresenter;
            _mapper = mapper;
        }
        
        /// <summary>
        /// Getting a DashBoard Details
        /// </summary>
        /// <param name="companyId">Company Id (optional)</param>
        /// <returns>Dash Board Details</returns>
        [HttpGet("dash-board")]
        public async Task<ActionResult> GetDashBoardDetails(string companyId = "")
        {
            await _dashBoardUseCases.Handle(new GetDetailsRequest(companyId), _getDetailsPresenter);
            return _getDetailsPresenter.ContentResult;
        }
    }
}