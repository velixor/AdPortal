using AdPortalApi.Models;
using Dto.Contracts.AdContracts;

namespace AdPortalApi.Services
{
    public interface IAdService : IEntityBaseService<Ad, AdRequest, AdResponse>
    {
    }
}