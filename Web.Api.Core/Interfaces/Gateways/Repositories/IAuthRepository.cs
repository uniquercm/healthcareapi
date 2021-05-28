using System.Threading.Tasks;
using Web.Api.Core.Dto.UseCaseRequests;

namespace Web.Api.Core.Interfaces.Gateways.Repositories
{
    public interface IAuthRepository 
    {
        Task<string> CheckUsername(LoginRequest loginRequest);
        string EncryptWithSalt(string text, string salt);
        Task<LoginUserDetails> GetLoginUserDetails(string userId);
        Task<UserDetails> GetUserDetails(string userId);
        Task<bool> CheckUserNameAvailability(string userId, string userName);
        Task<bool> CreateUser(UserRequest userRequest);
        string GenerateUUID();
        Task<bool> EditUser(UserRequest userRequest);
    }
}
