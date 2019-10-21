using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdPortalApi.Data;
using AdPortalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AdPortalApi.Services
{
    public class UserService : EntityBaseService<User>, IUserService
    {
        public UserService(AdPortalContext context) : base(context)
        {
        }

        public override async Task<User> GetByIdAsync(Guid id)
        {
            return await Context.Users.Include(u => u.Ads).AsNoTracking()
                .SingleAsync(u => u.Id == id);
        }

        public async Task<User> RegisterNewUserAsync(User user)
        {
            var created = Context.Users.Add(user);

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return null;
            }

            return created.Entity;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            if (!await IsEntryExistAsync(user.Id))
                return false;

            Context.Entry(user).State = EntityState.Modified;

            throw new NotImplementedException();
        }
    }
}