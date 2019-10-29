using System;
using System.Threading.Tasks;
using Data.Models;
using Dto.Contracts;

namespace Core.Services
{
    public interface IUserService : IEntityService<User>
    {
        Task<TResponse> CreateNewAsync<TResponse>(IRequest request) where TResponse : IResponse;
        Task<TResponse> AuthenticateAsync<TResponse>(IRequest request) where TResponse : IResponse;
    }
}