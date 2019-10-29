using System;
using System.Threading.Tasks;
using Data.Models;
using Dto.Contracts;

namespace Core.Services
{
    public interface IAdService : IEntityService<Ad>
    {
        Task<TResponse> PostNewAdAsync<TResponse>(IRequest request, Guid userId) where TResponse : IResponse;
    }
}