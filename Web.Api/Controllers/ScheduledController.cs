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
        /// <param name="fromDate">Scheduled From Date (optional)</param>
        /// <param name="toDate">Scheduled To Date (optional)</param>
        /// <param name="companyId">Company Id (optional)</param>
        /// <param name="scheduledId">Scheduled Id (optional)</param>
        /// <param name="patientId">Patient Id (optional)</param>
        /// <param name="isFieldAllocation">Is Field Allocation (optional)</param>
        /// <param name="fieldAllocationStatus"> Field Allocation Status(all, allowed, notallowed) (optional)</param>
        /// <param name="serviceName">Service Name(all, tracker, sticker, 4pcr, 8pcr, discharge) (optional)</param>
        /// <param name="serviceStatus">Service Status(all, applied, removed, replaced, visited, notvisited, discharged, pending, others) (optional)</param>
        /// <returns>Scheduled Details</returns>
        [HttpGet("scheduled")]
        public async Task<ActionResult> GetScheduledDetails(DateTime fromDate = new DateTime(), DateTime toDate = new DateTime(), string companyId = "", string scheduledId = "", string patientId = "", bool isFieldAllocation = false, string fieldAllocationStatus = "all", string serviceName = "all", string serviceStatus = "all")
        {
            if(isFieldAllocation)
                await _scheduledUseCases.Handle(new GetDetailsRequest(companyId, patientId, scheduledId, fromDate, toDate, "FieldAllocation", fieldAllocationStatus, serviceName, serviceStatus), _getDetailsPresenter);
            else
                await _scheduledUseCases.Handle(new GetDetailsRequest(companyId, patientId, scheduledId, fromDate, toDate, "", fieldAllocationStatus, serviceName, serviceStatus), _getDetailsPresenter);
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
            await _scheduledUseCases.Handle(new GetDetailsRequest(callId, "", "", "", scheduledId, "Call"), _getDetailsPresenter);
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

        /// <summary>
        /// Modifying a Field Allocation
        /// </summary>
        /// <param name="request">Modifying Field Allocation Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpPut("field-allocation")]
        public async Task<ActionResult> EditFieldAllocation([FromBody] Models.Request.FieldAllocationRequest request)
        {
            await _scheduledUseCases.Handle(_mapper.Map<FieldAllocationRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }

    }
}