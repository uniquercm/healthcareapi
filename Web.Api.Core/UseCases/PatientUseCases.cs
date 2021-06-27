using System.Threading.Tasks;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.UseCaseRequests;
using Web.Api.Core.Dto.UseCaseResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.UseCases;

namespace Web.Api.Core.UseCases
{
    public sealed class PatientUseCases : IPatientUseCases
    {
        private readonly IPatientRepository _patientRepository;
        public PatientUseCases(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
           
        }

        public async Task<bool> Handle(GetDetailsRequest request, IOutputPort<GetDetailsResponse> outputPort)
        {
            GetDetailsResponse getDetailsResponse;

            /*if(request.LableName == "DrCall")
                getDetailsResponse = new GetDetailsResponse(await _patientRepository.GetDrNurseCallDetails(request.CompanyId, "DrCall", request.ScheduledFromDate, request.ScheduledToDate, request.CallStatus), true, "Data Fetched Successfully");
            else if(request.LableName == "NurseCall")
                getDetailsResponse = new GetDetailsResponse(await _patientRepository.GetDrNurseCallDetails(request.CompanyId, "NurseCall", request.ScheduledFromDate, request.ScheduledToDate, request.CallStatus), true, "Data Fetched Successfully");
            else*/
                getDetailsResponse = new GetDetailsResponse(await _patientRepository.GetPatientDetails(request.CompanyId, request.PatientId, request.LableName), true, "Data Fetched Successfully");

            outputPort.Handle(getDetailsResponse);
            return true;
        }
        public async Task<bool> Handle(PatientRequest request, IOutputPort<AcknowledgementResponse> outputPort)
        {
            AcknowledgementResponse acknowledgementResponse;

            if(request.IsUpdate)//Edit a Patient
            {
                if(await _patientRepository.EditPatient(request))
                    acknowledgementResponse = new AcknowledgementResponse(true, "Patient Successfully Modifyed");
                else
                    acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", "Error Occurred")}, false);
            }
            else//Create a Patient
            {
                if(await _patientRepository.CreatePatient(request))
                    acknowledgementResponse = new AcknowledgementResponse(request.PatientId, true, "Patient Created Successfully");
                else
                    acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", "Error Occurred")}, false);
            }
            outputPort.Handle(acknowledgementResponse);
            return true;
        }
        public async Task<bool> Handle(DeleteRequest request, IOutputPort<AcknowledgementResponse> outputPort)
        {
            AcknowledgementResponse acknowledgementResponse;
            if (await _patientRepository.DeletePatient(request))
                acknowledgementResponse = new AcknowledgementResponse(true, "Patient Deleted Successfully");
            else
                acknowledgementResponse = new AcknowledgementResponse(new[] { new Error("Error Occurred", "Error Occurred") }, false);
            outputPort.Handle(acknowledgementResponse);
            return true;
        }

    }
}