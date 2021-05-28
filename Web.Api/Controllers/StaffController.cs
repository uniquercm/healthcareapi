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
    public class StaffController : ControllerBase
    {
        private readonly IStaffUseCases _staffUseCases;
        private readonly AcknowledgementPresenter _acknowledgementPresenter;
        private readonly GetDetailsPresenter _getDetailsPresenter;
        private readonly AvailabilityPresenter _availabilityPresenter;
        private readonly IMapper _mapper;

        public StaffController(IStaffUseCases staffUseCases, AcknowledgementPresenter acknowledgementPresenter, GetDetailsPresenter getDetailsPresenter, AvailabilityPresenter availabilityPresenter, IMapper mapper)
        {   
            _staffUseCases = staffUseCases;
            _acknowledgementPresenter = acknowledgementPresenter;
            _getDetailsPresenter = getDetailsPresenter;
            _availabilityPresenter = availabilityPresenter;
            _mapper = mapper;
        }

        /// <summary>
        /// Getting a Staff Details
        /// </summary>
        /// <param name="companyId">Company Id (optional)</param>
        /// <param name="staffId">Staff Id (optional)</param>
        /// <returns>Staff Details</returns>
        [HttpGet("staff")]
        public async Task<ActionResult> GetPatientDetails(string companyId = "", string staffId = "")
        {
            await _staffUseCases.Handle(new GetDetailsRequest(companyId, "", staffId, "", ""), _getDetailsPresenter);
            return _getDetailsPresenter.ContentResult;
        }

        /// <summary>
        /// Creating a Staff
        /// </summary>
        /// <param name="request">New Staff Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpPost("staff")]
        public async Task<ActionResult> CreatePatient([FromBody] Models.Request.StaffRequest request)
        {
            request.IsUpdate = false;
            await _staffUseCases.Handle(_mapper.Map<StaffRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }

        /// <summary>
        /// Modifying a Staff
        /// </summary>
        /// <param name="request">Modifying Staff Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpPut("staff")]
        public async Task<ActionResult> EditPatient([FromBody] Models.Request.StaffRequest request)
        {
            request.IsUpdate = true;
            await _staffUseCases.Handle(_mapper.Map<StaffRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }

    }
}