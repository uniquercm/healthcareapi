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
        /// <param name="companyId">Company Id (optional)</param>
        /// <param name="patientId">Patient Id (optional)</param>
        /// <param name="scheduledId">Scheduled Id (optional)</param>
        /// <returns>Report Details</returns>
        [HttpGet("report")]
        public async Task<ActionResult> GetScheduledDetails(string companyId = "", string patientId = "", string scheduledId = "")
        {
            await _reportUseCases.Handle(new GetDetailsRequest(companyId, patientId, "", "", scheduledId, ""), _getDetailsPresenter);
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
    }
}