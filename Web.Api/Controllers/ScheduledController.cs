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
    [Route("v1/[controller]")]
    [ApiController]
    public class ScheduledController : ControllerBase
    {
        private readonly IScheduledUseCases _scheduledUseCases;
        private readonly AcknowledgementPresenter _acknowledgementPresenter;
        private readonly GetDetailsPresenter _getDetailsPresenter;
        private readonly AvailabilityPresenter _availabilityPresenter;
        private readonly IMapper _mapper;

        public ScheduledController(IScheduledUseCases scheduledUseCases, AcknowledgementPresenter acknowledgementPresenter, GetDetailsPresenter getDetailsPresenter, AvailabilityPresenter availabilityPresenter, IMapper mapper)
        {   
            _scheduledUseCases = scheduledUseCases;
            _acknowledgementPresenter = acknowledgementPresenter;
            _getDetailsPresenter = getDetailsPresenter;
            _availabilityPresenter = availabilityPresenter;
            _mapper = mapper;
        }

        /// <summary>
        /// Getting a Scheduled Details
        /// </summary>
        /// <param name="scheduledId">Scheduled Id (optional)</param>
        /// <param name="patientstaffId">Patient Staff Id (optional)</param>
        /// <returns>Scheduled Details</returns>
        [HttpGet("scheduled")]
        public async Task<ActionResult> GetScheduledDetails(string scheduledId = "", string patientstaffId = "")
        {
            await _scheduledUseCases.Handle(new GetDetailsRequest(scheduledId, "", "", patientstaffId, ""), _getDetailsPresenter);
            return _getDetailsPresenter.ContentResult;
        }

        /// <summary>
        /// Creating a Scheduled
        /// </summary>
        /// <param name="request">New Scheduled Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpPost("scheduled")]
        public async Task<ActionResult> CreateScheduled([FromBody] Models.Request.ScheduledRequest request)
        {
            request.IsUpdate = false;
            await _scheduledUseCases.Handle(_mapper.Map<ScheduledRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }

        /// <summary>
        /// Modifying a Scheduled
        /// </summary>
        /// <param name="request">Modifying Scheduled Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpPut("scheduled")]
        public async Task<ActionResult> EditScheduled([FromBody] Models.Request.ScheduledRequest request)
        {
            request.IsUpdate = true;
            await _scheduledUseCases.Handle(_mapper.Map<ScheduledRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }

        /// <summary>
        /// Getting a Call Details
        /// </summary>
        /// <param name="callId">Call Id (optional)</param>
        /// <param name="scheduledId">Scheduled Id (optional)</param>
        /// <returns>Call Details</returns>
        [HttpGet("call")]
        public async Task<ActionResult> GetCallDetails(string callId = "", string scheduledId = "")
        {
            await _scheduledUseCases.Handle(new GetDetailsRequest(callId, "Call", "", "", scheduledId), _getDetailsPresenter);
            return _getDetailsPresenter.ContentResult;
        }

        /// <summary>
        /// Creating a Call
        /// </summary>
        /// <param name="request">New Call Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpPost("call")]
        public async Task<ActionResult> CreateCall([FromBody] Models.Request.CallRequest request)
        {
            request.IsUpdate = false;
            await _scheduledUseCases.Handle(_mapper.Map<CallRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }

        /// <summary>
        /// Modifying a Call
        /// </summary>
        /// <param name="request">Modifying Call Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpPut("call")]
        public async Task<ActionResult> EditCall([FromBody] Models.Request.CallRequest request)
        {
            request.IsUpdate = true;
            await _scheduledUseCases.Handle(_mapper.Map<CallRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }

    }
}