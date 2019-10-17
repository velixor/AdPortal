using System;
using System.Linq;
using System.Threading.Tasks;
using AdPortalApi.Models;

namespace AdPortalApi.Services
{
    public interface IAdService
    {
        IQueryable<Ad> GetAllAds();
        Task<Ad> GetAdByIdAsync(Guid id);
        Task<Ad> PostNewAdAsync(Ad ad);
        Task<bool> UpdateAdAsync(Ad ad);
        Task<bool> DeleteAdByIdAsync(Guid id);
        Task<bool> IsAdExistAsync(Guid id);
    }
}