using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdPortalApi.Data;
using AdPortalApi.Extensions;
using AdPortalApi.Models;
using AutoMapper;
using Dto.Contracts;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace AdPortalApi.Services
{
    public abstract class EntityBaseService<T, TRequest, TResponse> : IEntityBaseService<T, TRequest, TResponse>
        where T : class, IEntity
        where TRequest : class
        where TResponse : class
    {
        protected readonly AdPortalContext Context;
        protected readonly IMapper Mapper;
        protected readonly ISieveProcessor SieveProcessor;

        protected EntityBaseService(AdPortalContext context, IMapper mapper, ISieveProcessor sieveProcessor)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            SieveProcessor = sieveProcessor ?? throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public virtual async Task<TResponse> GetByIdAsync(Guid id)
        {
            var entry = await Entities().AsNoTracking().SingleAsync(x => x.Id == id);
            return Mapper.Map<TResponse>(entry);
        }

        public virtual PagingResponse<TResponse> Get(SieveModel sieveModel)
        {
            if (sieveModel == null) throw new ArgumentNullException(nameof(sieveModel));

            var entries = Entities().AsNoTracking();
            var count = SieveProcessor.ApplyAndCount(sieveModel, ref entries);

            return new PagingResponse<TResponse>
            {
                Items = Mapper.Map<List<TResponse>>(entries),
                Count = count
            };
        }

        public virtual async Task<TResponse> CreateNewAsync(TRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            var newEntry = Mapper.Map<T>(request);
            Context.Set<T>().Add(newEntry);
            await Context.SaveChangesAsync();
            
            return Mapper.Map<TResponse>(newEntry);
        }

        public virtual async Task<TResponse> UpdateAsync(Guid id, TRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var entry = await Entities().SingleAsync(x => x.Id == id);
            entry = Mapper.Map(request, entry);
            await Context.SaveChangesAsync();

            return Mapper.Map<TResponse>(entry);
        }

        public virtual async Task DeleteByIdAsync(Guid id)
        {
            var entry = await Context.Set<T>().SingleAsync(x => x.Id == id);
            Context.Set<T>().Remove(entry);
            await Context.SaveChangesAsync();
        }

        public async Task<bool> IsExistAsync(Guid id)
        {
            return await Context.Set<T>().AsNoTracking().AnyAsync(x => x.Id == id);
        }

        protected abstract IQueryable<T> Entities();
    }
}