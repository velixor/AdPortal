using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Configurations;
using Data;
using Data.Models;
using Dto.Contracts.AdContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sieve.Services;

namespace Core.Services
{
    public class AdService : EntityBaseService<Ad, AdRequest, AdResponse>, IAdService
    {
        private readonly IUserService _userService;
        private readonly IOptions<UserConfigs> _userConfigs;
        private readonly IImageHelper _imageHelper;
        private readonly IOptions<ImageConfigs> _imageConfigs;

        public AdService(AdPortalContext context, IMapper mapper, ISieveProcessor sieveProcessor,
            IUserService userService, IOptions<UserConfigs> userConfigs, IImageHelper imageHelper,
            IOptions<ImageConfigs> imageConfigs)
            : base(context, mapper, sieveProcessor)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _userConfigs = userConfigs ?? throw new ArgumentNullException(nameof(userConfigs));
            _imageHelper = imageHelper ?? throw new ArgumentNullException(nameof(imageHelper));
            _imageConfigs = imageConfigs ?? throw new ArgumentNullException(nameof(imageConfigs));
        }

        public override async Task<AdResponse> CreateNewAsync(AdRequest ad)
        {
            if (ad == null) throw new ArgumentNullException(nameof(ad));

            var user = await _userService.GetByIdAsync(ad.UserId);
            if (user.AdsCount >= _userConfigs.Value.AdCountLimit)
                throw new ConstraintException($"User {user.Id} has reached his ad limit");

            var newAd = Mapper.Map<Ad>(ad);
            newAd.CreationDate = DateTime.Now;
            newAd.ImageName = _imageHelper.UploadImageAndGetName(ad.Image);

            Context.Ads.Add(newAd);
            await Context.SaveChangesAsync();

            return Mapper.Map<AdResponse>(newAd);
        }

        public override async Task DeleteByIdAsync(Guid id)
        {
            var ad = await Context.Ads.SingleAsync(x => x.Id == id);
            DeleteImage(ad.ImageName);
            Context.Ads.Remove(ad);
            await Context.SaveChangesAsync();
        }

        protected override IQueryable<Ad> Entities()
        {
            return Context.Ads.Include(ad => ad.User);
        }

        private void DeleteImage(string imageName)
        {
            var path = Path.Combine(_imageConfigs.Value.Path, imageName);
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}