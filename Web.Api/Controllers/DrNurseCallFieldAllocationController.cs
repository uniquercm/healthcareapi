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
    public class DrNurseCallFieldAllocationController : ControllerBase
    {
        private readonly IDrNurseCallFieldAllocationUseCases _drNurseCallFieldAllocationUseCases;
        private readonly AcknowledgementPresenter _acknowledgementPresenter;
        private readonly GetDetailsPresenter _getDetailsPresenter;
        private readonly AvailabilityPresenter _availabilityPresenter;
        private readonly IMapper _mapper;

        public DrNurseCallFieldAllocationController(IDrNurseCallFieldAllocationUseCases drNurseCallFieldAllocationUseCases, AcknowledgementPresenter acknowledgementPresenter, GetDetailsPresenter getDetailsPresenter, AvailabilityPresenter availabilityPresenter, IMapper mapper)
        {   
            _drNurseCallFieldAllocationUseCases = drNurseCallFieldAllocationUseCases;
            _acknowledgementPresenter = acknowledgementPresenter;
            _getDetailsPresenter = getDetailsPresenter;
            _availabilityPresenter = availabilityPresenter;
            _mapper = mapper;
        }

        /// <summary>
        /// Getting a Doctor Nurse and Team Field Allocation Call Details
        /// </summary>
        /// <param name="fromDate">Scheduled From Date (optional)</param>
        /// <param name="toDate">Scheduled To Date (optional)</param>
        /// <param name="companyId">Company Id (optional)</param>
        /// <param name="callName">Call Name (doctor, nurse, team, field) (optional)</param>
        /// <param name="callStatus">Call Status(all, called, pending, visited, notvisited) (optional)</param>
        /// <param name="teamUserName">Team User Name (optional)</param>
        /// <param name="serviceName">Service Name(all, tracker, sticker, 4pcr, 8pcr, discharge) (optional)</param>
        /// <param name="serviceStatus">Service Status(all, applied, removed, replaced, visited, notvisited, discharged, pending, others) (optional)</param>
        /// <param name="dateSearchType">Date Search Type(schedule, allocated, reallocaed) (optional)</param>
        /// <returns>Doctor Nurse Team Field Allocation Call Details</returns>
        [HttpGet("doctor-nurse-team-call")]
        public async Task<ActionResult> GetDrNurseCallFieldAllocationCallDetails(DateTime fromDate, DateTime toDate, string companyId = "", string callName = "field", string callStatus = "all", string teamUserName = "", string serviceName = "all", string serviceStatus = "all", string dateSearchType = "schedule")
        {//tracker,
            if(callName.ToLower().Equals("doctor"))
                await _drNurseCallFieldAllocationUseCases.Handle(new GetDetailsRequest(companyId, fromDate, toDate, "DrCall", callStatus, teamUserName, serviceName, dateSearchType), _getDetailsPresenter);
            else if(callName.ToLower().Equals("nurse"))
                await _drNurseCallFieldAllocationUseCases.Handle(new GetDetailsRequest(companyId, fromDate, toDate, "NurseCall", callStatus, teamUserName, serviceName, dateSearchType), _getDetailsPresenter);
            else if(callName.ToLower().Equals("team"))
                await _drNurseCallFieldAllocationUseCases.Handle(new GetDetailsRequest(companyId, fromDate, toDate, "TeamCall", callStatus, teamUserName, serviceName, serviceStatus, dateSearchType), _getDetailsPresenter);
            else
                await _drNurseCallFieldAllocationUseCases.Handle(new GetDetailsRequest(companyId, fromDate, toDate, "FieldAllow", callStatus, teamUserName, serviceName, serviceStatus, dateSearchType), _getDetailsPresenter);
            return _getDetailsPresenter.ContentResult;
        }

        /// <summary>
        /// Modifying a Call
        /// </summary>
        /// <param name="request">Modifying Call Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpPut("doctor-nurse-team-call")]
        public async Task<ActionResult> EditCall([FromBody] Models.Request.CallRequest request)
        {
            request.IsUpdate = true;
            await _drNurseCallFieldAllocationUseCases.Handle(_mapper.Map<CallRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }

        /// <summary>
        /// Modifying a Service Plan
        /// </summary>
        /// <param name="request">Modifying Service Plan Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpPut("serviceplan")]
        public async Task<ActionResult> EditServicePlan([FromBody] Models.Request.ServicePlanRequest request)
        {
            //request.IsUpdate = true;
            await _drNurseCallFieldAllocationUseCases.Handle(_mapper.Map<ServicePlanRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }

    }
}