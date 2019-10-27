using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Dto.Contracts;
using Sieve.Models;

namespace Core.Helpers
{
    public interface IEntityServiceHelper<T> where T : class, IEntity
    {
        IQueryable<T> Entries { get; }
        TResponse MapToResponse<TResponse>(T entry) where TResponse : IResponse;
        List<TResponse> MapToResponses<TResponse>(IQueryable<T> entries) where TResponse : IResponse;
        T MapFromRequest(IRequest request);
        Task<TResponse> GetByIdAsync<TResponse>(Guid id) where TResponse : IResponse;
        PagingResponse<TResponse> Get<TResponse>(SieveModel sieveModel) where TResponse : IResponse;
        Task<TResponse> CreateNewAsync<TResponse>(IRequest request) where TResponse : IResponse;
        Task<TResponse> UpdateAsync<TResponse>(Guid id, IRequest request) where TResponse : IResponse;
        Task DeleteByIdAsync(Guid id);
        Task<bool> IsExistAsync(Guid id);
    }
}