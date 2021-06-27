using System;
using AutoMapper;
using Web.Api.Core.Dto.UseCaseRequests;

namespace Web.Api.Models.Settings
{
    public class MapperSettings :Profile
    {
        public MapperSettings()
        {
            CreateMap<Models.Request.LoginRequest, LoginRequest>();
            CreateMap<Models.Request.UserRequest, UserRequest>();
            CreateMap<Models.Request.AreaRequest, AreaRequest>();
            CreateMap<Models.Request.MasterRequest, MasterRequest>();
            CreateMap<Models.Request.CompanyRequest, CompanyRequest>();
            CreateMap<Models.Request.PatientRequest, PatientRequest>();
            CreateMap<Models.Request.ServicePlanRequest, ServicePlanRequest>();
            CreateMap<Models.Request.ScheduledRequest, ScheduledRequest>();
            CreateMap<Models.Request.CallRequest, CallRequest>();
            CreateMap<Models.Request.FieldAllocationRequest, FieldAllocationRequest>();
            CreateMap<Models.Request.FieldAllocationDetails, FieldAllocationDetails>();
            CreateMap<Models.Request.ReportDetails, ReportDetails>();
            CreateMap<Models.Request.DeleteRequest, DeleteRequest>();
        }
    }
}