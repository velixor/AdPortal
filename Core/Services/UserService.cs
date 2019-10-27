using AutoMapper;
using Data;
using Data.Models;
using Sieve.Services;

namespace Core.Services
{
    public class UserService : EntityService<User>
    {
        public UserService(AdPortalContext context, IMapper mapper, ISieveProcessor sieveProcessor)
            : base(context, mapper, sieveProcessor)
        {
        }
    }
}