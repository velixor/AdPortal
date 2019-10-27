using System;
using System.Threading.Tasks;
using Dto.Contracts;
using Sieve.Models;

namespace Core.Services
{
    public interface IEntityService<TIRequest, TIResponse>
    {
        Task<TResponse> GetByIdAsync<TResponse>(Guid id) where TResponse : TIResponse;
        PagingResponse<TResponse> Get<TResponse>(SieveModel sieveModel) where TResponse : TIResponse;
        Task<TResponse> CreateNewAsync<TResponse>(TIRequest request) where TResponse : TIResponse;
        Task<TResponse> UpdateAsync<TResponse>(Guid id, TIRequest request) where TResponse : TIResponse;
        Task DeleteByIdAsync(Guid id);
        Task<bool> IsExistAsync(Guid id);
    }
}