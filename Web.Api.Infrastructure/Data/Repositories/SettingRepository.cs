using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;

namespace Web.Api.Infrastructure.Data.Repositories
{
    [ExcludeFromCodeCoverage]
    public class SettingRepository:ISettingRepository
    {
        public string AwsRegion {get;}
        public string AppPlazaBaseURL {get;set;}
       
        // public  SettingRepository(IConfiguration configuration)
        // {
        //     ElasticCacheConnectionString=configuration.GetValue<string>("BI_ELASTIC_CACHE_CONNECTION_STRING");
        //     VerifyConfigurationValue(ElasticCacheConnectionString,"BI_ELASTIC_CACHE_CONNECTION_STRING");

        //     var defaultSessionTimeoutSeconds=configuration.GetValue<string>("BI_DEFAULT_SESSION_TIMEOUT_IN_SECONDS");
        //     VerifyConfigurationValue(defaultSessionTimeoutSeconds,"BI_DEFAULT_SESSION_TIMEOUT_IN_SECONDS");
        //     DefaultSessionTimeoutSeconds=Convert.ToInt32(defaultSessionTimeoutSeconds);

        //     var defaultLastVerifySessionCacheTtlInSeconds=configuration.GetValue<string>("BI_DEFAULT_LASTVERIFY_SESSION_CACHE_TTL_IN_SECONDS");
        //     VerifyConfigurationValue(defaultLastVerifySessionCacheTtlInSeconds,"BI_DEFAULT_LASTVERIFY_SESSION_CACHE_TTL_IN_SECONDS");
        //     DefaultLastVerifySessionCacheTtlInSeconds=Convert.ToInt32(defaultLastVerifySessionCacheTtlInSeconds);

        // }
        // private void VerifyConfigurationValue(string value,string configurationNmae)
        // {
        //     if(string.IsNullOrWhiteSpace(value))
        //     {
        //         throw new System.Exception($"Environment variable {configurationNmae} not set");
        //     }
        // }
    }
}