using System;
using System.Threading.Tasks;
using Data.Models;
using Dto.Contracts;

namespace Core.Services
{
    public interface IUserService : IEntityService<User>
    {
        Task<TResponse> RegisterNewAsync<TResponse>(IRequest request) where TResponse : IResponse;
        Task RegisterNewAsync(IRequest request);
        Task<TResponse> LoginAsync<TResponse>(IRequest request) where TResponse : IResponse;
        Task<bool> LoginAsync(IRequest request);
    }
}