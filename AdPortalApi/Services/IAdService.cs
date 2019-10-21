using System;
using System.Linq;
using System.Threading.Tasks;
using AdPortalApi.Models;

namespace AdPortalApi.Services
{
    public interface IAdService : IEntityBaseService<Ad>
    {
        Task<Ad> PostNewAdAsync(Ad ad);
        Task<bool> UpdateAdAsync(Ad ad);
    }
}