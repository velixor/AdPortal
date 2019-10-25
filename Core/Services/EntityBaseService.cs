using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Extensions;
using Data;
using Data.Models;
using Dto.Contracts;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace Core.Services
{
    public abstract class EntityBaseService<T, TRequest, TResponse> : IEntityBaseService<T, TRequest, TResponse>
        where T : class, IEntity
        where TRequest : class
        where TResponse : class
    {
        protected readonly AdPortalContext Context;
        protected readonly IMapper Mapper;
        protected readonly ISieveProcessor SieveProcessor;

        protected virtual IQueryable<T> Entries
            => Context.Set<T>();

        protected virtual TResponse MapToResponse(T entry)
            => Mapper.Map<TResponse>(entry);

        protected virtual List<TResponse> MapToResponses(IQueryable<T> entries)
            => Mapper.ProjectTo<TResponse>(entries).ToList();

        protected virtual T MapFromRequest(TRequest request)
            => Mapper.Map<T>(request);


        protected EntityBaseService(AdPortalContext context, IMapper mapper, ISieveProcessor sieveProcessor)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            SieveProcessor = sieveProcessor ?? throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public virtual async Task<TResponse> GetByIdAsync(Guid id)
        {
            var entry = await Entries.AsNoTracking().SingleAsync(x => x.Id == id);
            return MapToResponse(entry);
        }

        public virtual PagingResponse<TResponse> Get(SieveModel sieveModel)
        {
            if (sieveModel == null) throw new ArgumentNullException(nameof(sieveModel));

            var entries = Entries.AsNoTracking();
            var count = SieveProcessor.ApplyAndCount(sieveModel, ref entries);

            return new PagingResponse<TResponse>
            {
                Items = MapToResponses(entries),
                Count = count
            };
        }

        public virtual async Task<TResponse> CreateNewAsync(TRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var newEntry = MapFromRequest(request);
            Context.Set<T>().Add(newEntry);
            await Context.SaveChangesAsync();

            return MapToResponse(newEntry);
        }

        public virtual async Task<TResponse> UpdateAsync(Guid id, TRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var entry = await Entries.SingleAsync(x => x.Id == id);
            entry = Mapper.Map(request, entry);
            await Context.SaveChangesAsync();

            return MapToResponse(entry);
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