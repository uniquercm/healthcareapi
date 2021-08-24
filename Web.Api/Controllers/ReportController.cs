using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Presenters;
using AutoMapper;

namespace Web.Api.Controllers
{
    [Route("v1")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportUseCases _reportUseCases;
        private readonly GetDetailsPresenter _getDetailsPresenter;
        private readonly AcknowledgementPresenter _acknowledgementPresenter;
        private readonly IMapper _mapper;

        public ReportController(IReportUseCases reportUseCases, GetDetailsPresenter getDetailsPresenter, AcknowledgementPresenter acknowledgementPresenter, IMapper mapper)
        {   
            _reportUseCases = reportUseCases;
            _getDetailsPresenter = getDetailsPresenter;
            _acknowledgementPresenter = acknowledgementPresenter;
            _mapper = mapper;
        }

        /// <summary>
        /// Getting a Report Details
        /// </summary>
        /// <param name="sendOnFromDate">SendOn From Date (optional)</param>
        /// <param name="sendOnToDate">SendOn To Date (optional)</param>
        /// <param name="companyId">Company Id (optional)</param>
        /// <param name="patientId">Patient Id (optional)</param>
        /// <param name="scheduledId">Scheduled Id (optional)</param>
        /// <param name="extractData">Extract Data (all, yes, no) (optional)</param>
        /// <param name="sendClaim">Send Claim (all, yes, no) (optional)</param>
        /// <param name="areaNames">Multiple Area Name (all, )</param>
        /// <returns>Report Details</returns>
        [HttpGet("report")]
        public async Task<ActionResult> GetReportDetails(DateTime sendOnFromDate, DateTime sendOnToDate, string companyId = "", string patientId = "", string scheduledId = "", string extractData = "all", string sendClaim = "all", string areaNames = "all")
        {
            await _reportUseCases.Handle(new GetDetailsRequest(companyId, patientId, scheduledId, sendOnFromDate, sendOnToDate, "", extractData, sendClaim, areaNames), _getDetailsPresenter);
            return _getDetailsPresenter.ContentResult;
        }

        /// <summary>
        /// Modifying a Report Details
        /// </summary>
        /// <param name="request">Modifying Report Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpPut("report")]
        public async Task<ActionResult> EditReportDetails([FromBody] Models.Request.ReportDetails request)
        {
            await _reportUseCases.Handle(_mapper.Map<ReportDetails>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }

        /// <summary>
        /// Getting a Team Field Allocation Report Details
        /// </summary>
        /// <param name="companyId">Company Id</param>
        /// <param name="teamUserName">Team User Name</param>
        /// <param name="scheduledFromDate">Scheduled From Date (optional)</param>
        /// <param name="scheduledToDate">Scheduled To Date (optional)</param>
        /// <returns>Team Field Allocation Report Details</returns>
        [HttpGet("company/{companyId}/teamname/{teamUserName}/team-report")]
        public async Task<ActionResult> GetTeamReportDetails(string companyId, string teamUserName, DateTime scheduledFromDate, DateTime scheduledToDate)
        {
            await _reportUseCases.Handle(new GetDetailsRequest(scheduledFromDate, scheduledToDate, companyId, teamUserName, true), _getDetailsPresenter);
            return _getDetailsPresenter.ContentResult;
        }
    }
}