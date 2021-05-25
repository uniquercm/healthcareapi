using System.Threading.Tasks;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;

namespace Web.Api.Core.UseCases
{
    public sealed class PatientStaffUseCases : IPatientStaffUseCases
    {
        private readonly IPatientStaffRepository _patientStaffRepository;
        public PatientStaffUseCases(IPatientStaffRepository patientStaffRepository)
        {
            _patientStaffRepository = patientStaffRepository;
        }

        public async Task<bool> Handle(GetDetailsRequest request, IOutputPort<GetDetailsResponse> outputPort)
        {
            GetDetailsResponse getDetailsResponse;

            getDetailsResponse = new GetDetailsResponse(await _patientStaffRepository.GetPatientStaffDetails(request.PatientStaffId, request.PatientId, request.StaffId), true, "Data Fetched Successfully");

            outputPort.Handle(getDetailsResponse);
            return true;
        }
        public async Task<bool> Handle(PatientStaffRequest request, IOutputPort<AcknowledgementResponse> outputPort)
        {
            AcknowledgementResponse acknowledgementResponse;

            if(request.IsUpdate)//Edit a Patient
            {
                if(await _patientStaffRepository.EditPatientStaff(request))
                    acknowledgementResponse = new AcknowledgementResponse(true, "Staff Patient Successfully Modifyed");
                else
                    acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", "Error Occurred")}, false);
            }
            else//Create a Patient
            {
                if(await _patientStaffRepository.CreatePatientStaff(request))
                    acknowledgementResponse = new AcknowledgementResponse(true, "Staff Patient Created Successfully");
                else
                    acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", "Error Occurred")}, false);
            }
            outputPort.Handle(acknowledgementResponse);
            return true;
        }

    }
}