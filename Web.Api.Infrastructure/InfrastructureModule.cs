using Autofac;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Core.Interfaces.Services;
using Web.Api.Infrastructure.Data.Repositories;
using Web.Api.Infrastructure.Logging;
using Web.Api.Infrastructure.Services;
using Module = Autofac.Module;

namespace Web.Api.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthRepository>().As<IAuthRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DashBoardRepository>().As<IDashBoardRepository>().InstancePerLifetimeScope();
            builder.RegisterType<HealthRepository>().As<IHealthRepository>().InstancePerLifetimeScope();
            builder.RegisterType<MasterRepository>().As<IMasterRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PatientRepository>().As<IPatientRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ScheduledRepository>().As<IScheduledRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DrNurseCallFieldAllocationRepository>().As<IDrNurseCallFieldAllocationRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ReportRepository>().As<IReportRepository>().InstancePerLifetimeScope();
            builder.RegisterType<HttpClientService>().As<IHttpClientService>().InstancePerLifetimeScope();
            builder.RegisterType<SettingRepository>().As<ISettingRepository>().InstancePerLifetimeScope();
            builder.RegisterType<Logger>().As<ILogger>().SingleInstance();
        }
    }
}