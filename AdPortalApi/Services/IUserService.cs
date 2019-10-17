using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdPortalApi.Models;

namespace AdPortalApi.Services
{
    public interface IUserService
    {
        IQueryable<User> GetAllUsers();
        Task<User> GetUserByIdAsync(Guid id);
        Task<User> RegisterNewUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserByIdAsync(Guid id);
        Task<bool> IsUserExistAsync(Guid id);
    }
}