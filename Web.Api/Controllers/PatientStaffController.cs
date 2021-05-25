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
    public class PatientStaffController : ControllerBase
    {
        private readonly IPatientStaffUseCases _patientStaffUseCases;
        private readonly AcknowledgementPresenter _acknowledgementPresenter;
        private readonly GetDetailsPresenter _getDetailsPresenter;
        private readonly AvailabilityPresenter _availabilityPresenter;
        private readonly IMapper _mapper;

        public PatientStaffController(IPatientStaffUseCases patientStaffUseCases, AcknowledgementPresenter acknowledgementPresenter, GetDetailsPresenter getDetailsPresenter, AvailabilityPresenter availabilityPresenter, IMapper mapper)
        {   
            _patientStaffUseCases = patientStaffUseCases;
            _acknowledgementPresenter = acknowledgementPresenter;
            _getDetailsPresenter = getDetailsPresenter;
            _availabilityPresenter = availabilityPresenter;
            _mapper = mapper;
        }

        /// <summary>
        /// Getting a Patient Staff Details
        /// </summary>
        /// <param name="patientStaffId">Patient Staff Id (optional)</param>
        /// <param name="patientId">Patient Id (optional)</param>
        /// <param name="staffId">Staff Id (optional)</param>
        /// <returns>Patient Staff Details</returns>
        [HttpGet("patient-staff")]
        public async Task<ActionResult> GetPatientDetails(string patientStaffId = "", string patientId = "", string staffId = "")
        {
            await _patientStaffUseCases.Handle(new GetDetailsRequest("", patientId, staffId, patientStaffId, ""), _getDetailsPresenter);
            return _getDetailsPresenter.ContentResult;
        }

        /// <summary>
        /// Creating a Patient Staff
        /// </summary>
        /// <param name="request">New Patient Staff Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpPost("patient-staff")]
        public async Task<ActionResult> CreatePatient([FromBody] Models.Request.PatientStaffRequest request)
        {
            request.IsUpdate = false;
            await _patientStaffUseCases.Handle(_mapper.Map<PatientStaffRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }

        /// <summary>
        /// Modifying a Patient Staff
        /// </summary>
        /// <param name="request">Modifying Patient Staff Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpPut("patient-staff")]
        public async Task<ActionResult> EditPatient([FromBody] Models.Request.PatientStaffRequest request)
        {
            request.IsUpdate = true;
            await _patientStaffUseCases.Handle(_mapper.Map<PatientStaffRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }

    }
}