using System;
using System.Threading.Tasks;
using Core.Helpers;
using Data.Models;
using Dto.Contracts;
using Dto.Contracts.UserContracts;
using Sieve.Models;

namespace Core.Services
{
    public class UserService : IUserService
    {
        private readonly IEntityServiceHelper<User> _serviceHelper;

        public UserService(IEntityServiceHelper<User> serviceHelper)
        {
            _serviceHelper = serviceHelper ?? throw new ArgumentNullException(nameof(serviceHelper));
        }

        public async Task<TResponse> GetByIdAsync<TResponse>(Guid id) where TResponse : IUserResponse
        {
            return await _serviceHelper.GetByIdAsync<TResponse>(id);
        }

        public PagingResponse<TResponse> Get<TResponse>(SieveModel sieveModel) where TResponse : IUserResponse
        {
            return _serviceHelper.Get<TResponse>(sieveModel);
        }

        public async Task<TResponse> CreateNewAsync<TResponse>(IUserRequest request) where TResponse : IUserResponse
        {
            return await _serviceHelper.CreateNewAsync<TResponse>(request);
        }

        public async Task<TResponse> UpdateAsync<TResponse>(Guid id, IUserRequest request) where TResponse : IUserResponse
        {
            return await _serviceHelper.UpdateAsync<TResponse>(id, request);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            await _serviceHelper.DeleteByIdAsync(id);
        }

        public async Task<bool> IsExistAsync(Guid id)
        {
            return await _serviceHelper.IsExistAsync(id);
        }
    }
}