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
    public class PatientController : ControllerBase
    {
        private readonly IPatientUseCases _patientUseCases;
        private readonly AcknowledgementPresenter _acknowledgementPresenter;
        private readonly GetDetailsPresenter _getDetailsPresenter;
        private readonly AvailabilityPresenter _availabilityPresenter;
        private readonly IMapper _mapper;

        public PatientController(IPatientUseCases patientUseCases, AcknowledgementPresenter acknowledgementPresenter, GetDetailsPresenter getDetailsPresenter, AvailabilityPresenter availabilityPresenter, IMapper mapper)
        {   
            _patientUseCases = patientUseCases;
            _acknowledgementPresenter = acknowledgementPresenter;
            _getDetailsPresenter = getDetailsPresenter;
            _availabilityPresenter = availabilityPresenter;
            _mapper = mapper;
        }

        /// <summary>
        /// Getting a Patient Details
        /// </summary>
        /// <param name="fromDate">Scheduled From Date (optional)</param>
        /// <param name="toDate">Scheduled To Date (optional)</param>
        /// <param name="companyId">Company Id (optional)</param>
        /// <param name="patientId">Patient Id (optional)</param>
        /// <param name="isDoctorCall">is Doctor Call (optional)</param>
        /// <param name="isNurseCall">is Nurse Call (optional)</param>
        /// <param name="isFieldAllow">is Field Allocation (optional)</param>
        /// <returns>Patient Details</returns>
        [HttpGet("patient")]
        public async Task<ActionResult> GetPatientDetails(DateTime fromDate, DateTime toDate, string companyId = "", string patientId = "", bool isDoctorCall = false, bool isNurseCall = false)
        {
            if(isDoctorCall)
                await _patientUseCases.Handle(new GetDetailsRequest(companyId, fromDate, toDate, "DrCall"), _getDetailsPresenter);
            else if(isNurseCall)
                await _patientUseCases.Handle(new GetDetailsRequest(companyId, fromDate, toDate, "NurseCall"), _getDetailsPresenter);
            else if(isNurseCall)
                await _patientUseCases.Handle(new GetDetailsRequest(companyId, fromDate, toDate, "FieldAllow"), _getDetailsPresenter);
            else
                await _patientUseCases.Handle(new GetDetailsRequest(companyId, patientId), _getDetailsPresenter);
            return _getDetailsPresenter.ContentResult;
        }

        /// <summary>
        /// Creating a Patient
        /// </summary>
        /// <param name="request">New Patient Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpPost("patient")]
        public async Task<ActionResult> CreatePatient([FromBody] Models.Request.PatientRequest request)
        {
            request.IsUpdate = false;
            await _patientUseCases.Handle(_mapper.Map<PatientRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }

        /// <summary>
        /// Modifying a Patient
        /// </summary>
        /// <param name="request">Modifying Patient Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpPut("patient")]
        public async Task<ActionResult> EditPatient([FromBody] Models.Request.PatientRequest request)
        {
            request.IsUpdate = true;
            await _patientUseCases.Handle(_mapper.Map<PatientRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }

    }
}