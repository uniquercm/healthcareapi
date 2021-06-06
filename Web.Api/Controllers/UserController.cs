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
    public class UserController : ControllerBase
    {
        private readonly IAuthUseCases _authUseCases;
        private readonly AcknowledgementPresenter _acknowledgementPresenter;
        private readonly GetDetailsPresenter _getDetailsPresenter;
        private readonly AvailabilityPresenter _availabilityPresenter;
        private readonly IMapper _mapper;
        public UserController(IAuthUseCases authUseCases, AcknowledgementPresenter acknowledgementPresenter, GetDetailsPresenter getDetailsPresenter, AvailabilityPresenter availabilityPresenter, IMapper mapper)
        {   
            _authUseCases = authUseCases;
             _acknowledgementPresenter = acknowledgementPresenter;
             _getDetailsPresenter = getDetailsPresenter;
             _availabilityPresenter = availabilityPresenter;
            _mapper = mapper;
        }

        /// <summary>
        /// Getting a User Details
        /// </summary>
        /// <param name="userId">User Id (optional)</param>
        /// <returns>User Details</returns>
        [HttpGet("user")]
        public async Task<ActionResult> GetUserDetails(string userId = "")
        {
            await _authUseCases.Handle(new GetDetailsRequest(userId), _getDetailsPresenter);
            return _getDetailsPresenter.ContentResult;
        }

        /// <summary>
        /// Getting a all Area
        /// </summary>
        /// <returns>List of Areas</returns>
        [HttpGet("area")]
        public async Task<ActionResult> GetAreaList()
        {
            await _authUseCases.Handle(new GetDetailsRequest(), _getDetailsPresenter);
            return _getDetailsPresenter.ContentResult;
        }

        /// <summary>
        /// Creating a Area
        /// </summary>
        /// <param name="request">Area Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpPost("area")]
        public async Task<ActionResult> CreateArea([FromBody] Models.Request.AreaRequest request)
        {
            await _authUseCases.Handle(_mapper.Map<AreaRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }


        /// <summary>
        /// Creating a User
        /// </summary>
        /// <param name="request">New User Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpPost("user")]
        public async Task<ActionResult> CreateUser([FromBody] Models.Request.UserRequest request)
        {
            request.IsUpdate = false;
            await _authUseCases.Handle(_mapper.Map<UserRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }

        /// <summary>
        /// Modifying a User
        /// </summary>
        /// <param name="request">Modifying User Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpPut("user")]
        public async Task<ActionResult> EditUser([FromBody] Models.Request.UserRequest request)
        {
            request.IsUpdate = true;
            await _authUseCases.Handle(_mapper.Map<UserRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }
        
        /// <summary>
        /// User Name Availability
        /// </summary>
        /// <param name="userId">User Id (Optional)</param>
        /// <param name="userName">User Name</param>
        /// <returns></returns>
        [HttpGet("user-name-available")]
        public async Task<ActionResult> UserNameAvailable(string userId, string userName)
        {
            await _authUseCases.Handle(new AvailabilityRequest(userId, userName), _availabilityPresenter);
            return _availabilityPresenter.ContentResult;
        }
    }
}
