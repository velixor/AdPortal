using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdPortalApi.Models;

namespace AdPortalApi.Services
{
    public interface IUserService : IEntityBaseService<User>
    {
        Task<User> RegisterNewUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
    }
}