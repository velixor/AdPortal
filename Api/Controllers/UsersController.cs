using Core.Services;
using Data.Models;
using Dto.Contracts.UserContracts;

namespace Api.Controllers
{
    public class UsersController : EntityBaseController<User, UserRequest, UserResponse>
    {
        public UsersController(IUserService entityService) : base(entityService)
        {
        }
    }
}