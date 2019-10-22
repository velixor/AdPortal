using AdPortalApi.Models;
using AdPortalApi.Services;
using Dto.Contracts.AdContracts;

namespace AdPortalApi.Controllers
{
    public class AdsController : EntityBaseController<Ad, AdRequest, AdResponse>
    {
        public AdsController(IAdService adService) : base(adService)
        {
        }
    }
}