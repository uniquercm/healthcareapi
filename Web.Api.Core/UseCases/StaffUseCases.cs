using System.Threading.Tasks;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;

namespace Web.Api.Core.UseCases
{
    public sealed class StaffUseCases : IStaffUseCases
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IAuthRepository _authRepository;
        public StaffUseCases(IStaffRepository staffRepository, IAuthRepository authRepository)
        {
            _staffRepository = staffRepository;
            _authRepository = authRepository;
           
        }

        public async Task<bool> Handle(GetDetailsRequest request, IOutputPort<GetDetailsResponse> outputPort)
        {
            GetDetailsResponse getDetailsResponse;

            getDetailsResponse = new GetDetailsResponse(await _staffRepository.GetStaffDetails(request.Id, request.StaffId), true, "Data Fetched Successfully");

            outputPort.Handle(getDetailsResponse);
            return true;
        }
        public async Task<bool> Handle(StaffRequest request, IOutputPort<AcknowledgementResponse> outputPort)
        {
            AcknowledgementResponse acknowledgementResponse;

            UserRequest userRequest = new UserRequest();
            userRequest.UserName = request.UserName;
            userRequest.Password = request.Password;
            userRequest.CompanyId = request.CompanyId;
            userRequest.CreatedBy = request.CreatedBy;

            if(Enums.StaffType.Doctor.Equals(request.StaffType))
                userRequest.UserType = (int) Enums.UserType.Doctor;
            else if(Enums.StaffType.Nurse.Equals(request.StaffType))
                userRequest.UserType = (int) Enums.UserType.Nurse;
            else if(Enums.StaffType.Receptionist.Equals(request.StaffType))
                userRequest.UserType = (int) Enums.UserType.Receptionist;
            else if(Enums.StaffType.Admin.Equals(request.StaffType))
                userRequest.UserType = (int) Enums.UserType.Admin;


            if(request.IsUpdate)//Edit a Staff
            {
                userRequest.IsUpdate = true;

                    if(await _authRepository.EditUser(userRequest))
                    {
                        if(await _staffRepository.EditStaff(request))
                            acknowledgementResponse = new AcknowledgementResponse(true, "Staff Successfully Modifyed");
                        else
                            acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", "Error Occurred")}, false);
                    }
                    else
                        acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", "Error Occurred")}, false);
            }
            else//Create a Staff
            {
                if(await _authRepository.CheckUserNameAvailability(request.UserId, request.UserName))
                    acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Already Exist", "The User Name already available")}, false);
                else
                {
                    userRequest.IsUpdate = false;

                    if(await _authRepository.CreateUser(userRequest))
                    {
                        request.UserId = userRequest.UserId;
                        if(await _staffRepository.CreateStaff(request))
                            acknowledgementResponse = new AcknowledgementResponse(true, "Staff Created Successfully");
                        else
                            acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", "Error Occurred")}, false);
                    }
                    else
                        acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", "Error Occurred")}, false);
                }
            }
            outputPort.Handle(acknowledgementResponse);
            return true;
        }

    }
}