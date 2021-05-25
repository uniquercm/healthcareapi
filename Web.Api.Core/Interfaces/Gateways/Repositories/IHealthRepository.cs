using System.Threading.Tasks;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface IHealthRepository 
    {
        Task<bool> CheckDBHealth();
    }
}
