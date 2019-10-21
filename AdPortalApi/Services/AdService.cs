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
    public class AdService : EntityBaseService<Ad>, IAdService
    {
        private readonly IUserService _userService;
        private readonly IOptions<UserConfigs> _userConfigs;

        public AdService(AdPortalContext context, IUserService userService, IOptions<UserConfigs> userConfigs) : base(context)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _userConfigs = userConfigs ?? throw new ArgumentNullException(nameof(userConfigs));
        }

        public override async Task<Ad> GetByIdAsync(Guid id)
        {
            return await Context.Ads
                .Include(ad => ad.User)
                .SingleAsync(u => u.Id == id);
        }

        public async Task<Ad> PostNewAdAsync(Ad ad)
        {
            if (!await _userService.IsEntryExistAsync(ad.UserId))
                return null;

            var adCountOfUser = await Context.Ads.AsNoTracking().CountAsync(x => x.UserId == ad.UserId);
            if (adCountOfUser >= _userConfigs.Value.AdCountLimit)
                return null;    // TODO make more clearer

            ad.CreationDate = DateTime.Now;

            var created = Context.Ads.Add(ad);
            if (!await TrySaveChangesAsync())
                return null;
            return created.Entity;
        }

        // TODO Improve update logic
        public async Task<bool> UpdateAdAsync(Ad ad)
        {
            if (!await IsEntryExistAsync(ad.Id))
                return false;

            var dbAd = await GetByIdAsync(ad.Id);
            if (ad.Content != null)
                dbAd.Content = ad.Content;
            dbAd.ImageName = ad.ImageName;

            return await TrySaveChangesAsync();
        }

        private async Task<bool> TrySaveChangesAsync()
        {
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return false;
            }

            return true;
        }
    }
}