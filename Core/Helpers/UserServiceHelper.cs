using AutoMapper;
using Data;
using Data.Models;
using Sieve.Services;

namespace Core.Helpers
{
    public class UserServiceHelper : EntityServiceHelper<User>
    {
        public UserServiceHelper(AdPortalContext context, IMapper mapper, ISieveProcessor sieveProcessor)
            : base(context, mapper, sieveProcessor)
        {
        }
    }
}