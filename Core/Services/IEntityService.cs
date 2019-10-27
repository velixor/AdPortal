using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Dto.Contracts;
using Sieve.Models;

namespace Core.Services
{
    public interface IEntityService<T> where T : class, IEntity
    {
        Task<TResponse> GetByIdAsync<TResponse>(Guid id) where TResponse : IResponse;
        PagingResponse<TResponse> Get<TResponse>(SieveModel sieveModel) where TResponse : IResponse;
        Task<TResponse> CreateNewAsync<TResponse>(IRequest request) where TResponse : IResponse;
        Task<TResponse> UpdateAsync<TResponse>(Guid id, IRequest request) where TResponse : IResponse;
        Task DeleteByIdAsync(Guid id);
        Task<bool> IsExistAsync(Guid id);
    }
}