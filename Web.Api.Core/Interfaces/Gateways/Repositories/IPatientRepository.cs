﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseRequests;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface IPatientRepository 
    {
        Task<List<PatientDetails>> GetPatientDetails(string companyId, string patientId);
        Task<bool> CreatePatient(PatientRequest patientRequest);
        string GenerateUUID();
        Task<bool> EditPatient(PatientRequest patientRequest);
        Task<List<DrNurseCallDetails>> GetFieldAllowCallDetails(string companyId, DateTime scheduledFromDate, DateTime scheduledToDate);
        Task<List<DrNurseCallDetails>> GetDrNurseCallDetails(string companyId, string callName, DateTime scheduledFromDate, DateTime scheduledToDate);
    }
}
