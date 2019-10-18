using System;
using System.Linq;
using System.Threading.Tasks;
using AdPortalApi.Configurations;
using AdPortalApi.Data;
using AdPortalApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AdPortalApi.Services
{
    public class AdService : IAdService
    {
        private readonly AdPortalContext _context;
        private readonly IUserService _userService;
        private readonly IOptions<UserConfigs> _userConfigs;

        public AdService(AdPortalContext context, IUserService userService, IOptions<UserConfigs> userConfigs)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _userConfigs = userConfigs ?? throw new ArgumentNullException(nameof(userConfigs));
        }

        public IQueryable<Ad> GetAllAds()
        {
            return _context.Ads.Include(ad => ad.User).AsNoTracking();
        }

        public async Task<Ad> GetAdByIdAsync(Guid id)
        {
            return await _context.Ads
                .Include(ad => ad.User)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Ad> PostNewAdAsync(Ad ad)
        {
            if (!await _userService.IsUserExistAsync(ad.UserId))
                return null;

            var adCountOfUser = await _context.Ads.AsNoTracking().CountAsync(x => x.UserId == ad.UserId);
            if (adCountOfUser >= _userConfigs.Value.AdCountLimit)
                return null;

            ad.CreationDate = DateTime.Now;

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

            var dbAd = await GetAdByIdAsync(ad.Id);
            if (ad.Content != null)
                dbAd.Content = ad.Content;
            dbAd.ImageName = ad.ImageName;

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
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }
    }
}