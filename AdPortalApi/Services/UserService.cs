using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdPortalApi.Data;
using AdPortalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AdPortalApi.Services
{
    public class UserService : IUserService
    {
        private readonly AdPortalContext _context;

        public UserService(AdPortalContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> RegisterNewUserAsync(User user)
        {
            var created = _context.Users.Add(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return null;
            }

            return created.Entity;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            if (!await IsUserExistAsync(user.Id))
                return false;

            _context.Entry(user).State = EntityState.Modified;

            return await TrySaveChangesAsync();
        }

        public async Task<bool> DeleteUserByIdAsync(Guid id)
        {
            if (!await IsUserExistAsync(id))
                return false;

            _context.Users.Remove(await GetUserByIdAsync(id));

            return await TrySaveChangesAsync();
        }

        public async Task<bool> IsUserExistAsync(Guid id)
        {
            return await _context.Users.AsNoTracking().AnyAsync(user => user.Id == id);
        }

        private async Task<bool> TrySaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return false;
            }

            return true;
        }
    }
}