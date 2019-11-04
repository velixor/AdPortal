using System;
using System.Threading.Tasks;
using Data.Models;
using Dto.Contracts;
using Sieve.Models;

namespace Core.Services
{
    public interface IEntityService<TEntity> where TEntity : class, IEntity
    {
        Task<TResponse> GetByIdAsync<TResponse>(Guid id) where TResponse : IResponse;
        PagingResponse<TResponse> Get<TResponse>(SieveModel sieveModel = null) where TResponse : IResponse;
        Task<TResponse> UpdateAsync<TResponse>(Guid id, IRequest request, Guid userId) where TResponse : IResponse;
        Task DeleteByIdAsync(Guid id, Guid userId);
        Task<bool> IsExistAsync(Guid id);
    }
}