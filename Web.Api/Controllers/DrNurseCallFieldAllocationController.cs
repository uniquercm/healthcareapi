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
        /// <param name="isDoctorCall">is Doctor Call (optional)</param>
        /// <param name="isNurseCall">is Nurse Call (optional)</param>
        /// <param name="callStatus"> Call Status(all, called, pending) (optional)</param>
        /// <param name="isFieldAllow">is Field Allocation (optional)</param>
        /// <returns>Patient Details</returns>
        [HttpGet("doctor-nurse-team-call")]
        public async Task<ActionResult> GetPatientDetails(DateTime fromDate, DateTime toDate, string companyId = "", bool isDoctorCall = false, bool isNurseCall = false, string callStatus = "all", bool isFieldAllow = false)
        {
            if(isDoctorCall)
                await _drNurseCallFieldAllocationUseCases.Handle(new GetDetailsRequest(companyId, fromDate, toDate, "DrCall", callStatus), _getDetailsPresenter);
            else if(isNurseCall)
                await _drNurseCallFieldAllocationUseCases.Handle(new GetDetailsRequest(companyId, fromDate, toDate, "NurseCall", callStatus), _getDetailsPresenter);
            else
                await _drNurseCallFieldAllocationUseCases.Handle(new GetDetailsRequest(companyId, fromDate, toDate, "FieldAllow", callStatus), _getDetailsPresenter);
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

    }
}