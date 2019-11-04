using System.Threading.Tasks;
using Dto.Contracts.UserContracts;

namespace WebApp.Services
{
    public interface IIdentityService
    {
        Task<bool> LoginAsync(UserLoginRequest loginRequest);
        Task LogoutAsync();
        Task RegisterAsync(UserRegisterRequest registerRequest);
        UserInfo User { get; }
        bool IsLogged { get; }
    }
}