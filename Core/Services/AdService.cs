using System;
using System.Threading.Tasks;
using Core.Helpers;
using Data.Models;
using Dto.Contracts;
using Dto.Contracts.AdContracts;
using Sieve.Models;

namespace Core.Services
{
    public class AdService : IAdService
    {
        private readonly IEntityServiceHelper<Ad> _serviceHelper;

        public AdService(IEntityServiceHelper<Ad> serviceHelper)
        {
            _serviceHelper = serviceHelper;
        }


        public async Task<TRequest> GetByIdAsync<TRequest>(Guid id) where TRequest : IAdResponse
        {
            return await _serviceHelper.GetByIdAsync<TRequest>(id);
        }

        public PagingResponse<TRequest> Get<TRequest>(SieveModel sieveModel) where TRequest : IAdResponse
        {
            return _serviceHelper.Get<TRequest>(sieveModel);
        }

        public async Task<TRequest> CreateNewAsync<TRequest>(IAdRequest request) where TRequest : IAdResponse
        {
            return await _serviceHelper.CreateNewAsync<TRequest>(request);
        }

        public async Task<TRequest> UpdateAsync<TRequest>(Guid id, IAdRequest request) where TRequest : IAdResponse
        {
            return await _serviceHelper.UpdateAsync<TRequest>(id, request);
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