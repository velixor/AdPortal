using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Extensions;
using Data;
using Data.Models;
using Dto.Contracts;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace Core.Helpers
{
    public class EntityServiceHelper<T> : IEntityServiceHelper<T> where T : class, IEntity
    {
        public readonly AdPortalContext Context;
        public readonly IMapper Mapper;
        public readonly ISieveProcessor SieveProcessor;

        public virtual IQueryable<T> Entries
            => Context.Set<T>();

        public virtual TResponse MapToResponse<TResponse>(T entry) where TResponse : IResponse
            => Mapper.Map<TResponse>(entry);

        public virtual List<TResponse> MapToResponses<TResponse>(IQueryable<T> entries) where TResponse : IResponse
            => Mapper.ProjectTo<TResponse>(entries).ToList();

        public virtual T MapFromRequest(IRequest request)
            => Mapper.Map<T>(request);


        public EntityServiceHelper(AdPortalContext context, IMapper mapper, ISieveProcessor sieveProcessor)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            SieveProcessor = sieveProcessor ?? throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public virtual async Task<TResponse> GetByIdAsync<TResponse>(Guid id) where TResponse : IResponse
        {
            var entry = await Entries.AsNoTracking().SingleAsync(x => x.Id == id);
            return MapToResponse<TResponse>(entry);
        }

        public virtual PagingResponse<TResponse> Get<TResponse>(SieveModel sieveModel) where TResponse : IResponse
        {
            if (sieveModel == null) throw new ArgumentNullException(nameof(sieveModel));

            var entries = Entries.AsNoTracking();
            var count = SieveProcessor.ApplyAndCount(sieveModel, ref entries);

            return new PagingResponse<TResponse>
            {
                Items = MapToResponses<TResponse>(entries),
                Count = count
            };
        }

        public virtual async Task<TResponse> CreateNewAsync<TResponse>(IRequest request) where TResponse : IResponse
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var newEntry = MapFromRequest(request);
            Context.Set<T>().Add(newEntry);
            await Context.SaveChangesAsync();

            return MapToResponse<TResponse>(newEntry);
        }

        public virtual async Task<TResponse> UpdateAsync<TResponse>(Guid id, IRequest request)
            where TResponse : IResponse
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var entry = await Entries.SingleAsync(x => x.Id == id);
            entry = Mapper.Map(request, entry);
            await Context.SaveChangesAsync();

            return MapToResponse<TResponse>(entry);
        }

        public virtual async Task DeleteByIdAsync(Guid id)
        {
            var entry = await Context.Set<T>().SingleAsync(x => x.Id == id);
            Context.Set<T>().Remove(entry);
            await Context.SaveChangesAsync();
        }

        public virtual async Task<bool> IsExistAsync(Guid id)
        {
            return await Context.Set<T>().AsNoTracking().AnyAsync(x => x.Id == id);
        }
    }
}