using Core.Services;
using Data.Models;
using Dto.Contracts.AdContracts;

namespace Api.Controllers
{
    public class AdsController : EntityBaseController<Ad, AdRequest, AdResponse>
    {
        public AdsController(IAdService adService) : base(adService)
        {
        }
    }
}