using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdPortalApi.Models;

namespace AdPortalApi.Services
{
    public interface IAdService
    {
        Task<IEnumerable<Ad>> GetAllAdsAsync();
        Task<Ad> GetAdByIdAsync(Guid id);
        Task<Ad> PostNewAdAsync(Ad ad);
        Task<bool> UpdateAdAsync(Ad ad);
        Task<bool> DeleteAdByIdAsync(Guid id);
        Task<bool> IsAdExistAsync(Guid id);
    }
}