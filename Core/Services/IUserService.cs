using Data.Models;
using Dto.Contracts.UserContracts;

namespace Core.Services
{
    public interface IUserService : IEntityBaseService<User, UserRequest, UserResponse>
    {
    }
}