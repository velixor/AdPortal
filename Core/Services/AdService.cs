using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Helpers;
using Core.Options;
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
        private readonly IOptions<UserOptions> _userOptions;
        private readonly IImageHelper _imageHelper;

        public AdService(AdPortalContext context, IMapper mapper, ISieveProcessor sieveProcessor,
            IOptions<UserOptions> userConfigs, IImageHelper imageHelper)
            : base(context, mapper, sieveProcessor)
        {
            _userOptions = userConfigs ?? throw new ArgumentNullException(nameof(userConfigs));
            _imageHelper = imageHelper ?? throw new ArgumentNullException(nameof(imageHelper));
        }

        public override async Task<AdResponse> CreateNewAsync(AdRequest ad)
        {
            if (ad == null) throw new ArgumentNullException(nameof(ad));

            var user = await Context.Users.SingleAsync(x => x.Id == ad.UserId);
            if (user.AdsCount >= _userOptions.Value.AdCountLimit)
                throw new ConstraintException($"User {user.Id} has reached his ad limit");

            await using var transaction = await Context.Database.BeginTransactionAsync();
            
            var newAd = Mapper.Map<Ad>(ad);
            newAd.CreationDate = DateTime.Now;
            newAd.ImageName = await _imageHelper.UploadImageAndGetName(ad.Image);
                
            Context.Ads.Add(newAd);
            user.AdsCount++;
            
            await Context.SaveChangesAsync();
            await transaction.CommitAsync();
                
            return Mapper.Map<AdResponse>(newAd);
        }

        public override async Task<AdResponse> UpdateAsync(Guid id, AdRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var ad = await Entities().SingleAsync(x => x.Id == id);

            _imageHelper.DeleteImage(ad.ImageName);
            ad = Mapper.Map(request, ad);
            ad.ImageName = await _imageHelper.UploadImageAndGetName(request.Image);

            await Context.SaveChangesAsync();

            return Mapper.Map<AdResponse>(ad);
        }

        public override async Task DeleteByIdAsync(Guid id)
        {
            var ad = await Context.Ads.SingleAsync(x => x.Id == id);

            await using var transaction = await Context.Database.BeginTransactionAsync();
            
            _imageHelper.DeleteImage(ad.ImageName);
            Context.Ads.Remove(ad);
            var user = await Context.Users.SingleAsync(x => x.Id == ad.UserId);
            user.AdsCount--;
            
            await Context.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        protected override IQueryable<Ad> Entities()
        {
            return Context.Ads.Include(ad => ad.User);
        }
    }
}