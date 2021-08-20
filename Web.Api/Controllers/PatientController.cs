using System;
using System.Collections.Generic;
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

/*        /// <summary>
        /// Getting a Patient Details
        /// </summary>
        /// <param name="fromDate">Scheduled From Date (optional)</param>
        /// <param name="toDate">Scheduled To Date (optional)</param>
        /// <param name="companyId">Company Id (optional)</param>
        /// <param name="patientId">Patient Id (optional)</param>
        /// <param name="isDoctorCall">is Doctor Call (optional)</param>
        /// <param name="isNurseCall">is Nurse Call (optional)</param>
        /// <param name="callStatus"> Call Status(all, called, pending) (optional)</param>
        /// <returns>Patient Details</returns>
        [HttpGet("patient")]
        public async Task<ActionResult> GetPatientDetails(DateTime fromDate, DateTime toDate, string companyId = "", string patientId = "", bool isDoctorCall = false, bool isNurseCall = false, string callStatus = "all")
        {
            if(isDoctorCall)
                await _patientUseCases.Handle(new GetDetailsRequest(companyId, fromDate, toDate, "DrCall", callStatus, ""), _getDetailsPresenter);
            else if(isNurseCall)
                await _patientUseCases.Handle(new GetDetailsRequest(companyId, fromDate, toDate, "NurseCall", callStatus, ""), _getDetailsPresenter);
            else
                await _patientUseCases.Handle(new GetDetailsRequest(companyId, patientId), _getDetailsPresenter);
            return _getDetailsPresenter.ContentResult;
        }*/

        /// <summary>
        /// Getting a Patient Details
        /// </summary>
        /// <param name="fromDate">Assigned From Date (optional)</param>
        /// <param name="toDate">Assigned To Date (optional)</param>
        /// <param name="companyId">Company Id (optional)</param>
        /// <param name="patientId">Patient Id (optional)</param>
        /// <param name="gMapLinkSatus"> Google Map Link (all, yes, no) (optional)</param>
        /// <param name="searchStatus"> Search Status (all, completed, pending, closed, scheduled, notscheduled) (optional)</param>
        /// <param name="areaNames">Multiple Area Name (all, )</param>
        /// <returns>Patient Details</returns>
        [HttpGet("patient")]
        public async Task<ActionResult> GetPatientDetails(DateTime fromDate, DateTime toDate, string companyId = "", string patientId = "", string gMapLinkSatus = "all", string searchStatus = "all", string areaNames = "all")
        {
            await _patientUseCases.Handle(new GetDetailsRequest(companyId, patientId, fromDate, toDate, gMapLinkSatus, searchStatus, areaNames), _getDetailsPresenter);
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
            Web.Api.Core.Dto.UseCaseRequests.PatientRequest corePatRequest = new PatientRequest();
            corePatRequest.PatientName = request.PatientName;
            corePatRequest.CompanyId = request.CompanyId;
            corePatRequest.RequestId = request.RequestId;
            corePatRequest.DateOfBirth = Convert.ToDateTime(request.DateOfBirth);
            corePatRequest.Age = request.Age;
            corePatRequest.Sex = request.Sex;
            corePatRequest.Address = request.Address;
            corePatRequest.LandMark = request.LandMark;
            corePatRequest.Area = request.Area;
            corePatRequest.CityId = request.CityId;
            corePatRequest.NationalityId = request.NationalityId;
            corePatRequest.MobileNo = request.MobileNo;
            corePatRequest.GoogleMapLink = request.GoogleMapLink;
            corePatRequest.AdultsCount = request.AdultsCount;
            corePatRequest.ChildrensCount = request.ChildrensCount;
            corePatRequest.StickerApplication = request.StickerApplication;
            corePatRequest.TrackerApplication = request.TrackerApplication;
            corePatRequest.StickerRemoval = request.StickerRemoval;
            corePatRequest.TrackerRemoval = request.TrackerRemoval;
            corePatRequest.CreatedBy = request.CreatedBy;
            corePatRequest.AssignedDate = Convert.ToDateTime(request.AssignedDate);
            await _patientUseCases.Handle(corePatRequest, _acknowledgementPresenter);
            //await _patientUseCases.Handle(_mapper.Map<PatientRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }

        /// <summary>
        /// Creating a List Of Patient
        /// </summary>
        /// <param name="request">New Patient Details List</param>
        /// <returns>Acknowledgement</returns>
        [HttpPost("patient-file")]
        public async Task<ActionResult> CreateListOfPatient([FromBody] Models.Request.FilePatientRequest request)
        {
            Web.Api.Core.Dto.UseCaseRequests.FilePatientRequest coreFilePatientRequest = new FilePatientRequest();
            foreach(Models.Request.PatientRequest singleCorePatRequest in request.PatientRequestList)
            {
                try{
                Web.Api.Core.Dto.UseCaseRequests.PatientRequest corePatRequest = new PatientRequest();
                corePatRequest.PatientName = singleCorePatRequest.PatientName;
                corePatRequest.CompanyId = singleCorePatRequest.CompanyId;
                corePatRequest.RequestId = singleCorePatRequest.RequestId;
                corePatRequest.DateOfBirth = Convert.ToDateTime(singleCorePatRequest.DateOfBirth);
                corePatRequest.Age = singleCorePatRequest.Age;
                corePatRequest.Sex = singleCorePatRequest.Sex;
                corePatRequest.Address = singleCorePatRequest.Address;
                corePatRequest.LandMark = singleCorePatRequest.LandMark;
                corePatRequest.Area = singleCorePatRequest.Area;
                corePatRequest.CityId = singleCorePatRequest.CityId;
                corePatRequest.NationalityId = singleCorePatRequest.NationalityId;
                corePatRequest.MobileNo = singleCorePatRequest.MobileNo;
                corePatRequest.GoogleMapLink = singleCorePatRequest.GoogleMapLink;
                corePatRequest.AdultsCount = singleCorePatRequest.AdultsCount;
                corePatRequest.ChildrensCount = singleCorePatRequest.ChildrensCount;
                corePatRequest.StickerApplication = singleCorePatRequest.StickerApplication;
                corePatRequest.TrackerApplication = singleCorePatRequest.TrackerApplication;
                corePatRequest.StickerRemoval = singleCorePatRequest.StickerRemoval;
                corePatRequest.TrackerRemoval = singleCorePatRequest.TrackerRemoval;
                corePatRequest.CreatedBy = singleCorePatRequest.CreatedBy;
                corePatRequest.AssignedDate = Convert.ToDateTime(singleCorePatRequest.AssignedDate);
                coreFilePatientRequest.PatientRequestList.Add(corePatRequest);
                }
                catch(Exception Err)
                {var Error = Err.Message.ToString();}
            }
            await _patientUseCases.Handle(coreFilePatientRequest, _acknowledgementPresenter);
            //await _patientUseCases.Handle(_mapper.Map<FilePatientRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }
        /*public async Task<ActionResult> CreateListOfPatient([FromBody] Models.Request.FilePatientRequest request)
        {
            await _patientUseCases.Handle(_mapper.Map<FilePatientRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }*/

        /// <summary>
        /// Modifying a Patient
        /// </summary>
        /// <param name="request">Modifying Patient Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpPut("patient")]
        public async Task<ActionResult> EditPatient([FromBody] Models.Request.PatientRequest request)
        {
            request.IsUpdate = true;
            Web.Api.Core.Dto.UseCaseRequests.PatientRequest corePatRequest = new PatientRequest();
            corePatRequest.PatientName = request.PatientName;
            corePatRequest.CompanyId = request.CompanyId;
            corePatRequest.RequestId = request.RequestId;
            corePatRequest.DateOfBirth = Convert.ToDateTime(request.DateOfBirth);
            corePatRequest.Age = request.Age;
            corePatRequest.Sex = request.Sex;
            corePatRequest.Address = request.Address;
            corePatRequest.LandMark = request.LandMark;
            corePatRequest.Area = request.Area;
            corePatRequest.CityId = request.CityId;
            corePatRequest.NationalityId = request.NationalityId;
            corePatRequest.MobileNo = request.MobileNo;
            corePatRequest.GoogleMapLink = request.GoogleMapLink;
            corePatRequest.AdultsCount = request.AdultsCount;
            corePatRequest.ChildrensCount = request.ChildrensCount;
            corePatRequest.StickerApplication = request.StickerApplication;
            corePatRequest.TrackerApplication = request.TrackerApplication;
            corePatRequest.StickerRemoval = request.StickerRemoval;
            corePatRequest.TrackerRemoval = request.TrackerRemoval;
            corePatRequest.ModifiedBy = request.ModifiedBy;
            corePatRequest.AssignedDate = Convert.ToDateTime(request.AssignedDate);
            await _patientUseCases.Handle(corePatRequest, _acknowledgementPresenter);
            //await _patientUseCases.Handle(_mapper.Map<PatientRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }

        /// <summary>
        /// Deleting a Patient
        /// </summary>
        /// <param name="request">Delete Patient Details</param>
        /// <returns>Acknowledgement</returns>
        [HttpDelete("patient")]
        public async Task<ActionResult> DeletePatient([FromBody] Models.Request.DeleteRequest request)
        {
            await _patientUseCases.Handle(_mapper.Map<DeleteRequest>(request), _acknowledgementPresenter);
            return _acknowledgementPresenter.ContentResult;
        }

        /// <summary>
        /// CRM Number Availability
        /// </summary>
        /// <param name="crmNumber">CRM Number</param>
        /// <param name="patientId">Patient Id (Optional)</param>
        /// <param name="companyId">Company Id (Optional)</param>
        /// <returns></returns>
        [HttpGet("patient-crmno-available")]
        public async Task<ActionResult> PatientCRMNumberAvailable(string crmNumber, string patientId = "", string companyId = "")
        {
            await _patientUseCases.Handle(new AvailabilityRequest(crmNumber, patientId, companyId), _availabilityPresenter);
            return _availabilityPresenter.ContentResult;
        }

    }
}