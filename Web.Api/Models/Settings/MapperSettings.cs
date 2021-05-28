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
            CreateMap<Models.Request.CompanyRequest, CompanyRequest>();
            CreateMap<Models.Request.PatientRequest, PatientRequest>();
            CreateMap<Models.Request.StaffRequest, StaffRequest>();
            CreateMap<Models.Request.PatientStaffRequest, PatientStaffRequest>();
            CreateMap<Models.Request.ScheduledRequest, ScheduledRequest>();
            CreateMap<Models.Request.CallRequest, CallRequest>();
        }
    }
}