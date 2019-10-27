using Dto.Contracts.UserContracts;

namespace Core.Services
{
    public interface IUserService : IEntityService<IUserRequest, IUserResponse>
    {}
}