using System;
using System.Threading.Tasks;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;

namespace Web.Api.Core.UseCases
{
    public sealed class AuthUseCases : IAuthUseCases
    {
        private readonly IAuthRepository _authRepository;
        public AuthUseCases(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }//

        public async Task<bool> Handle(LoginRequest message, IOutputPort<LoginResponse> outputPort)
        {
            LoginResponse loginResponse;

            string userId = await _authRepository.CheckUsername(message);
            if (string.IsNullOrEmpty(userId))
            {
                loginResponse = new LoginResponse(new[] { new Error("login_failure", "Invalid Request", 25 )}, false);
            }
            else
            {
                LoginUserDetails loginUserDetails = await _authRepository.GetLoginUserDetails(userId);
                if(loginUserDetails == null)
                    loginResponse = new LoginResponse(new[] { new Error("login_failure", "Invalid Request", 25 )}, false);
                else
                    loginResponse = new LoginResponse(loginUserDetails, true, "Login Successful");
            }
            outputPort.Handle(loginResponse);
            return true;
        }
        public async Task<bool> Handle(GetDetailsRequest request, IOutputPort<GetDetailsResponse> outputPort)
        {
            GetDetailsResponse getDetailsResponse;
            if(request.Id == "area")
                getDetailsResponse = new GetDetailsResponse(await _authRepository.GetAreaDetails(), true, "Data Fetched Successfully");
            else
                getDetailsResponse = new GetDetailsResponse(await _authRepository.GetUserDetails(request.Id, request.CompanyId, 0), true, "Data Fetched Successfully");
            outputPort.Handle(getDetailsResponse);
            return true;
        }
        public async Task<bool> Handle(UserRequest request, IOutputPort<AcknowledgementResponse> outputPort)
        {
            AcknowledgementResponse acknowledgementResponse;

            if(await _authRepository.CheckUserNameAvailability(request.UserId, request.UserName))
                acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Already Exist", "The User Name already available")}, false);
            else
            {
                if(request.IsUpdate)//Edit a User
                {
                    if(await _authRepository.EditUser(request))
                        acknowledgementResponse = new AcknowledgementResponse(true, "User Successfully Modifyed");
                    else
                        acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", "Error Occurred")}, false);
                }
                else//Create a User
                {
                    if(await _authRepository.CreateUser(request))
                        acknowledgementResponse = new AcknowledgementResponse(request.UserId, true, "User Created Successfully");
                    else
                        acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", "Error Occurred")}, false);
                }
            }
            outputPort.Handle(acknowledgementResponse);
            return true;
        }
        public async Task<bool> Handle(AvailabilityRequest request, IOutputPort<AvailabilityResponse> outputPort)
        {
            AvailabilityResponse availabilityResponse;
            bool retVal = false;

            retVal = await _authRepository.CheckUserNameAvailability(request.Id, request.Name);
            availabilityResponse = new AvailabilityResponse(retVal, "User Name", true);

            outputPort.Handle(availabilityResponse);
            return true;
        }

        public async Task<bool> Handle(AreaRequest request, IOutputPort<AcknowledgementResponse> outputPort)
        {
            AcknowledgementResponse acknowledgementResponse;

            if(await _authRepository.AddArea(request))
                acknowledgementResponse = new AcknowledgementResponse(true, "Area Name Successfully Added");
            else
                acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", "Error Occurred")}, false);

            outputPort.Handle(acknowledgementResponse);
            return true;
        }
        public async Task<bool> Handle(DeleteRequest request, IOutputPort<AcknowledgementResponse> outputPort)
        {
            AcknowledgementResponse acknowledgementResponse;
            if (await _authRepository.DeleteUser(request))
                acknowledgementResponse = new AcknowledgementResponse(true, "User Deleted Successfully");
            else
                acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", "Error Occurred") }, false);
            outputPort.Handle(acknowledgementResponse);
            return true;
        }

    }
}