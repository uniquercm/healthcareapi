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
        private readonly IPatientUseCases _patientUseCases;
        private readonly AcknowledgementPresenter _acknowledgementPresenter;
        private readonly GetDetailsPresenter _getDetailsPresenter;
        private readonly AvailabilityPresenter _availabilityPresenter;
        private readonly IMapper _mapper;

        public DrNurseCallFieldAllocationController(IPatientUseCases patientUseCases, AcknowledgementPresenter acknowledgementPresenter, GetDetailsPresenter getDetailsPresenter, AvailabilityPresenter availabilityPresenter, IMapper mapper)
        {   
            _patientUseCases = patientUseCases;
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
                await _patientUseCases.Handle(new GetDetailsRequest(companyId, fromDate, toDate, "DrCall", callStatus), _getDetailsPresenter);
            else if(isNurseCall)
                await _patientUseCases.Handle(new GetDetailsRequest(companyId, fromDate, toDate, "NurseCall", callStatus), _getDetailsPresenter);
            else
                await _patientUseCases.Handle(new GetDetailsRequest(companyId, fromDate, toDate, "FieldAllow", callStatus), _getDetailsPresenter);
            return _getDetailsPresenter.ContentResult;
        }

    }
}