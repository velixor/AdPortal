using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdPortalApi.Configurations;
using AdPortalApi.Data;
using AdPortalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AdPortalApi.Services
{
    public class AdService : IAdService
    {
        private readonly AdPortalContext _context;
        private readonly IUserService _userService;
        private readonly UserConfigs _userConfigs;

        public AdService(AdPortalContext context, IUserService userService, UserConfigs userConfigs)
        {
            _context = context;
            _userService = userService;
            _userConfigs = userConfigs;
        }

        public async Task<IEnumerable<Ad>> GetAllAdsAsync()
        {
            return await _context.Ads.Include(ad=>ad.User).AsNoTracking().ToListAsync();
        }

        public async Task<Ad> GetAdByIdAsync(Guid id)
        {
            return await _context.Ads.Include(ad=>ad.User).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Ad> PostNewAdAsync(Ad ad)
        {
            if (!await _userService.IsUserExistAsync(ad.User.Id))
                return null;

            var adCountOfUser = await _context.Ads.CountAsync(x => x.User.Id == ad.User.Id);
            if (adCountOfUser >= _userConfigs.AdCountLimit)
                return null;

            var created = _context.Ads.Add(ad);
            if (!await TrySaveChangesAsync())
                return null;
            return created.Entity;
        }

        // TODO Improve update logic
        public async Task<bool> UpdateAdAsync(Ad ad)
        {
            if (!await IsAdExistAsync(ad.Id))
                return false;
            
            var oldAd = await GetAdByIdAsync(ad.Id);

            oldAd.Content = ad.Content;
            oldAd.ImagePath = ad.ImagePath;
            
            return await TrySaveChangesAsync();
        }

        public async Task<bool> DeleteAdByIdAsync(Guid id)
        {
            if (!await IsAdExistAsync(id))
                return false;

            _context.Ads.Remove(await GetAdByIdAsync(id));

            return await TrySaveChangesAsync();
        }

        public async Task<bool> IsAdExistAsync(Guid id)
        {
            return await _context.Ads.AsNoTracking().AnyAsync(ad => ad.Id == id);
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