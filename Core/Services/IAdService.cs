using Data.Models;
using Dto.Contracts.AdContracts;

namespace Core.Services
{
    public interface IAdService : IEntityBaseService<Ad, AdRequest, AdResponse>
    {
    }
}