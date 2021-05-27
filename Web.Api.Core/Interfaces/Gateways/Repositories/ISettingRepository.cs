namespace Web.Api.Infrastructure.Data.Repositories
{
    public interface ISettingRepository
    {
        string AwsRegion { get; }
        string AppPlazaBaseURL { get; set;}
    }
}