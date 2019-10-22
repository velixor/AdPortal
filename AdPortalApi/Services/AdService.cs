using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdPortalApi.Configurations;
using AdPortalApi.Data;
using AdPortalApi.Models;
using AutoMapper;
using Dto.Contracts.AdContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sieve.Services;

namespace AdPortalApi.Services
{
    public class AdService : EntityBaseService<Ad, AdRequest, AdResponse>, IAdService
    {
        private readonly IUserService _userService;
        private readonly IOptions<UserConfigs> _userConfigs;
        private readonly IImageService _imageService;
        private readonly IOptions<ImageConfigs> _imageConfigs;
        
        public AdService(AdPortalContext context, IMapper mapper, ISieveProcessor sieveProcessor,
            IUserService userService, IOptions<UserConfigs> userConfigs, IImageService imageService,
            IOptions<ImageConfigs> imageConfigs) 
            : base(context, mapper, sieveProcessor)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _userConfigs = userConfigs ?? throw new ArgumentNullException(nameof(userConfigs));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
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
            newAd.ImageName = _imageService.UploadImageAndGetName(ad.Image);
            
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