using AdPortalApi.Models;
using AdPortalApi.Services;
using Dto.Contracts.UserContracts;

namespace AdPortalApi.Controllers
{
    public class UsersController : EntityBaseController<User, UserRequest, UserResponse>
    {
        public UsersController(IUserService entityService) : base(entityService)
        {
        }
    }
}