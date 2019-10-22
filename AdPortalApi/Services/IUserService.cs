using AdPortalApi.Models;
using Dto.Contracts.UserContracts;

namespace AdPortalApi.Services
{
    public interface IUserService : IEntityBaseService<User, UserRequest, UserResponse>
    {
    }
}