using AutoMapper;
using Data;
using Data.Models;
using Dto.Contracts.UserContracts;
using Sieve.Services;

namespace Core.Services
{
    public class UserService : EntityBaseService<User, UserRequest, UserResponse>, IUserService
    {
        public UserService(AdPortalContext context, IMapper mapper, ISieveProcessor sieveProcessor)
            : base(context, mapper, sieveProcessor)
        {
        }
    }
}