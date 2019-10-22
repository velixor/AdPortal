using System;
using System.Threading.Tasks;
using Data.Models;
using Dto.Contracts;
using Sieve.Models;

namespace Core.Services
{
    public interface IEntityBaseService<T, TRequest, TResponse>
        where T : class, IEntity
        where TRequest : class
        where TResponse : class
    {
        Task<TResponse> GetByIdAsync(Guid id);
        PagingResponse<TResponse> Get(SieveModel sieveModel);
        Task<TResponse> CreateNewAsync(TRequest request);
        Task<TResponse> UpdateAsync(Guid id, TRequest request);
        Task DeleteByIdAsync(Guid id);
        Task<bool> IsExistAsync(Guid id);
    }
}