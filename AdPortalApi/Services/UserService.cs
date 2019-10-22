using System;
using System.Linq;
using System.Threading.Tasks;
using AdPortalApi.Data;
using AdPortalApi.Models;
using AutoMapper;
using Dto.Contracts.UserContracts;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;

namespace AdPortalApi.Services
{
    public class UserService : EntityBaseService<User, UserRequest, UserResponse>, IUserService
    {
        public UserService(AdPortalContext context, IMapper mapper, ISieveProcessor sieveProcessor) 
            : base(context, mapper, sieveProcessor)
        {
        }

        protected override IQueryable<User> Entities()
        {
            return Context.Users.Include(u => u.Ads);
        }
    }
}