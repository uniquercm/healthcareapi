﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Web.Api.Core.Dto.UseCaseRequests;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface IPatientRepository 
    {
        Task<List<PatientDetails>> GetPatientDetails(string companyId, string patientId, string gMapLinkSatus);
        Task<bool> CreatePatient(PatientRequest patientRequest);
        string GenerateUUID();
        Task<bool> EditPatient(PatientRequest patientRequest);
        Task<bool> EditServicePlan(ServicePlanRequest servicePlanRequest);
    }
}
