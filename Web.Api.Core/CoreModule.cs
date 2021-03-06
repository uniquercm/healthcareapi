using Autofac;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Core.UseCases;

namespace Web.Api.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthUseCases>().As<IAuthUseCases>().InstancePerLifetimeScope();
            builder.RegisterType<HealthUseCases>().As<IHealthUseCases>().InstancePerLifetimeScope();
            builder.RegisterType<MasterUseCases>().As<IMasterUseCases>().InstancePerLifetimeScope();
            builder.RegisterType<PatientUseCases>().As<IPatientUseCases>().InstancePerLifetimeScope();
            builder.RegisterType<ScheduledUseCases>().As<IScheduledUseCases>().InstancePerLifetimeScope();
            builder.RegisterType<ReportUseCases>().As<IReportUseCases>().InstancePerLifetimeScope();
            builder.RegisterType<DashBoardUseCases>().As<IDashBoardUseCases>().InstancePerLifetimeScope();
            builder.RegisterType<DrNurseCallFieldAllocationUseCases>().As<IDrNurseCallFieldAllocationUseCases>().InstancePerLifetimeScope();
        }
    }
}