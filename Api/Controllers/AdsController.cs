using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Services;
using Dto.Contracts;
using Dto.Contracts.AdContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdsController : ControllerBase
    {
        private readonly IAdService _adService;

        public AdsController(IAdService adService)
        {
            _adService = adService ?? throw new ArgumentNullException(nameof(adService));
        }
        
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<AdResponse> Get([FromRoute] Guid id)
        {
            var entry = await _adService.GetByIdAsync<AdResponse>(id);
            return entry;
        }

        // Get with filtering, sorting and paginating
        [AllowAnonymous]
        [HttpGet]
        public PagingResponse<AdResponse> Get([FromQuery] SieveModel sieveModel)
        {
            return _adService.Get<AdResponse>(sieveModel);
        }

        [HttpPost]
        public async Task<AdResponse> Post([FromForm] AdCreateRequest createRequest)
        {
            var newEntry = await _adService.PostNewAdAsync<AdResponse>(createRequest, UserId);
            return newEntry;
        }

        [HttpPut("{id}")]
        public async Task<AdResponse> Put(Guid id, [FromForm] AdUpdateRequest createRequest)
        {
            var updatedEntry = await _adService.UpdateAsync<AdResponse>(id, createRequest, UserId);
            return updatedEntry;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _adService.DeleteByIdAsync(id, UserId);
            return NoContent();
        }

        private Guid UserId =>
            Guid.Parse(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
    }
}