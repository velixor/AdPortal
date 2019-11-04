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

namespace Core.Services
{
    public abstract class EntityService<T> : IEntityService<T> where T : class, IEntity
    {
        protected readonly AdPortalContext Context;
        protected readonly IMapper Mapper;
        protected readonly ISieveProcessor SieveProcessor;

        protected virtual IQueryable<T> Entries
            => Context.Set<T>();

        protected virtual TResponse MapToResponse<TResponse>(T entry) where TResponse : IResponse
        {
            var response = Mapper.Map<TResponse>(entry);
            AdaptResponse(response);
            return response;
        }

        protected virtual List<TResponse> MapToResponses<TResponse>(IQueryable<T> entries) where TResponse : IResponse
        {
            var responses = Mapper.ProjectTo<TResponse>(entries).ToList();
            responses.ForEach(AdaptResponse);
            return responses;
        }

        protected virtual T MapFromRequest(IRequest request)
            => Mapper.Map<T>(request);
        
        protected virtual void AdaptResponse<TResponse>(TResponse response)
        {}

        protected abstract bool IsAuthorized(Guid id, Guid userId);

        public EntityService(AdPortalContext context, IMapper mapper, ISieveProcessor sieveProcessor)
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

        public virtual PagingResponse<TResponse> Get<TResponse>(SieveModel sieveModel = null) where TResponse : IResponse
        {
            if (sieveModel == null) sieveModel = new SieveModel();

            var entries = Entries.AsNoTracking();
            var count = SieveProcessor.ApplyAndCount(sieveModel, ref entries);

            return new PagingResponse<TResponse>
            {
                Items = MapToResponses<TResponse>(entries),
                Count = count
            };
        }
        
        public virtual async Task<TResponse> UpdateAsync<TResponse>(Guid id, IRequest request, Guid userId)
            where TResponse : IResponse
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (!IsAuthorized(id, userId)) throw new UnauthorizedAccessException(nameof(request));
            
            var entry = await Entries.SingleAsync(x => x.Id == id);
            entry = Mapper.Map(request, entry);
            await Context.SaveChangesAsync();

            return MapToResponse<TResponse>(entry);
        }

        public virtual async Task DeleteByIdAsync(Guid id, Guid userId)
        {
            if (!IsAuthorized(id, userId)) throw new UnauthorizedAccessException();
            
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