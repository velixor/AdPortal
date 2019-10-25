using System;
using System.Collections.Generic;
using System.Data;
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

        protected override AdResponse MapToResponse(Ad ad)
        {
            var response = base.MapToResponse(ad);
            _imageHelper.ImageNameToImageUrl(response);
            return response;
        }

        protected override List<AdResponse> MapToResponses(IQueryable<Ad> entries)
        {
            var result = base.MapToResponses(entries);
            result.ForEach(_imageHelper.ImageNameToImageUrl);
            return result;
        }

        public AdService(AdPortalContext context, IMapper mapper, ISieveProcessor sieveProcessor,
            IOptions<UserOptions> userOptions, IImageHelper imageHelper)
            : base(context, mapper, sieveProcessor)
        {
            _userOptions = userOptions ?? throw new ArgumentNullException(nameof(userOptions));
            _imageHelper = imageHelper ?? throw new ArgumentNullException(nameof(imageHelper));
        }

        public override async Task<AdResponse> CreateNewAsync(AdRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            await using var transaction = await Context.Database.BeginTransactionAsync();

            var user = await Context.Users.SingleAsync(x => x.Id == request.UserId);
            if (user.AdsCount >= _userOptions.Value.AdCountLimit)
                throw new ConstraintException($"User {user.Id} has reached his ad limit");

            var ad = MapFromRequest(request);
            ad.CreationDate = DateTime.Now;
            ad.ImageName = await _imageHelper.UploadImageAndGetNameAsync(request.Image);

            Context.Ads.Add(ad);
            user.AdsCount++;

            await Context.SaveChangesAsync();
            await transaction.CommitAsync();

            return MapToResponse(ad);
        }

        public override async Task<AdResponse> UpdateAsync(Guid id, AdRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var ad = await Entries.SingleAsync(x => x.Id == id);

            _imageHelper.DeleteImage(ad.ImageName);
            ad = Mapper.Map(request, ad);
            ad.ImageName = await _imageHelper.UploadImageAndGetNameAsync(request.Image);

            await Context.SaveChangesAsync();

            return MapToResponse(ad);
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
    }
}