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
{//[Route("v1/[controller]")]
    [Route("v1")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly IMasterUseCases _masterUseCases;
        private readonly AcknowledgementPresenter _acknowledgementPresenter;
        private readonly GetDetailsPresenter _getDetailsPresenter;
        private readonly AvailabilityPresenter _availabilityPresenter;
        private readonly IMapper _mapper;

        public MasterController(IMasterUseCases masterUseCases, AcknowledgementPresenter acknowledgementPresenter, GetDetailsPresenter getDetailsPresenter, AvailabilityPresenter availabilityPresenter, IMapper mapper)
        {
            _masterUseCases = masterUseCases;
            _acknowledgementPresenter = acknowledgementPresenter;
            _getDetailsPresenter = getDetailsPresenter;
            _availabilityPresenter = availabilityPresenter;
            _mapper = mapper;
        }

        /// <summary>
        /// Getting a Master Details
        /// </summary>
        /// <returns>Master Details</returns>
        [HttpGet("master")]
        public async Task<ActionResult> GetMasterDetails()
        {
            await _masterUseCases.Handle(new GetDetailsRequest("Master"), _getDetailsPresenter);
            return _getDetailsPresenter.ContentResult;
        }

        /// <summary>
        /// Getting a City Details
        /// </summary>
        /// <returns>City Details</returns>
        [HttpGet("city")]
        public async Task<ActionResult> GetCityDetails()
        {
            await _masterUseCases.Handle(new GetDetailsRequest("City"), _getDetailsPresenter);
            return _getDetailsPresenter.ContentResult;
        }

        /// <summary>
        /// Getting a Nationality Details
        /// </summary>
        /// <returns>Nationality Details</returns>
        [HttpGet("nationality")]
        public async Task<ActionResult> GetNationalityDetails()
        {
            await _masterUseCases.Handle(new GetDetailsRequest("Nationality"), _getDetailsPresenter);
            return _getDetailsPresenter.ContentResult;
        }

        /// <summary>
        /// Getting a Section Details
        /// </summary>
        /// <returns>Section Details</returns>
        [HttpGet("section")]
        public async Task<ActionResult> GetSectionDetails()
        {
            await _masterUseCases.Handle(new GetDetailsRequest("Section"), _getDetailsPresenter);
            return _getDetailsPresenter.ContentResult;
        }

        /// <summary>
        /// Getting a RequestCRM Details
        /// </summary>
        /// <returns>RequestCRM Details</returns>
        [HttpGet("requestCRM")]
        public async Task<ActionResult> GetRequestCRMDetails()
        {
            await _masterUseCases.Handle(new GetDetailsRequest("RequestCRM"), _getDetailsPresenter);
            return _getDetailsPresenter.ContentResult;
        }

        /// <summary>
        /// Getting a Company Details
        /// </summary>
        /// <param name="companyId">Company Id (optional)</param>
        /// <returns>Company Details</returns>
        [HttpGet("company")]
        public async Task<ActionResult> GetCompanyDetails(string companyId = "")
        {
            await _masterUseCases.Handle(new GetDetailsRequest(companyId), _getDetailsPresenter);
            return _getDetailsPresenter.ContentResult;
        }

        /// <summary>
        /// Creating a Company
        /// </summary>
        /// <param name="request">New Company Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpPost("company")]
        public async Task<ActionResult> CreateCompany([FromBody] Models.Request.CompanyRequest request)
        {
            request.IsUpdate = false;
            await _masterUseCases.Handle(_mapper.Map<CompanyRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }

        /// <summary>
        /// Modifying a Company
        /// </summary>
        /// <param name="request">Modifying Company Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpPut("company")]
        public async Task<ActionResult> EditUser([FromBody] Models.Request.CompanyRequest request)
        {
            request.IsUpdate = true;
            await _masterUseCases.Handle(_mapper.Map<CompanyRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }

    }
}
