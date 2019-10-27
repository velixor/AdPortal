using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Options;
using Data;
using Data.Models;
using Dto.Contracts;
using Dto.Contracts.AdContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sieve.Services;

namespace Core.Helpers
{
    public class AdServiceHelper : EntityServiceHelper<Ad>
    {
        private readonly IOptions<UserOptions> _userOptions;
        private readonly IImageHelper _imageHelper;

        public override TResponse MapToResponse<TResponse>(Ad ad)
        {
            var response = base.MapToResponse<TResponse>(ad);
            _imageHelper.ImageNameToImageUrl(response as IHasImage);
            return response;
        }

        public override List<TResponse> MapToResponses<TResponse>(IQueryable<Ad> entries)
        {
            var result = base.MapToResponses<TResponse>(entries);
            result.ForEach(ad => _imageHelper.ImageNameToImageUrl(ad as IHasImage));
            return result;
        }

        public AdServiceHelper(AdPortalContext context, IMapper mapper, ISieveProcessor sieveProcessor,
            IOptions<UserOptions> userOptions, IImageHelper imageHelper)
            : base(context, mapper, sieveProcessor)
        {
            _userOptions = userOptions ?? throw new ArgumentNullException(nameof(userOptions));
            _imageHelper = imageHelper ?? throw new ArgumentNullException(nameof(imageHelper));
        }

        public override async Task<TResponse> CreateNewAsync<TResponse>(IRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
   
            var adRequest = request as AdRequest;
            if (adRequest == null) throw new ArgumentNullException(nameof(adRequest)); 
            
            await using var transaction = await Context.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead);

            var user = await Context.Users.SingleAsync(x => x.Id == adRequest.UserId);
            if (user.AdsCount >= _userOptions.Value.AdCountLimit)
                throw new ConstraintException($"User {user.Id} has reached his ad limit");

            var ad = MapFromRequest(adRequest);
            ad.CreationDate = DateTime.Now;
            ad.ImageName = await _imageHelper.UploadImageAndGetNameAsync(adRequest.Image);

            Context.Ads.Add(ad);
            user.AdsCount++;

            await Context.SaveChangesAsync();
            await transaction.CommitAsync();

            return MapToResponse<TResponse>(ad);
        }

        public override async Task<TResponse> UpdateAsync<TResponse>(Guid id, IRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            var adRequest = request as AdRequest;
            if (adRequest == null) throw new ArgumentNullException(nameof(adRequest)); 
            
            var ad = await Entries.SingleAsync(x => x.Id == id);

            _imageHelper.DeleteImage(ad.ImageName);
            ad = Mapper.Map(adRequest, ad);
            ad.ImageName = await _imageHelper.UploadImageAndGetNameAsync(adRequest.Image);

            await Context.SaveChangesAsync();

            return MapToResponse<TResponse>(ad);
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