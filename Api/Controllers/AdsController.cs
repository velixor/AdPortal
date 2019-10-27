using System;
using System.Threading.Tasks;
using Core.Services;
using Data.Models;
using Dto.Contracts;
using Dto.Contracts.AdContracts;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdsController : ControllerBase
    {
        private readonly IEntityService<Ad> _adService;

        public AdsController(IEntityService<Ad> adService)
        {
            _adService = adService ?? throw new ArgumentNullException(nameof(adService));
        }
        
        [HttpGet("{id}")]
        public async Task<AdResponse> Get([FromRoute] Guid id)
        {
            var entry = await _adService.GetByIdAsync<AdResponse>(id);
            return entry;
        }

        // Get with filtering, sorting and paginating
        [HttpGet]
        public PagingResponse<AdResponse> Get([FromQuery] SieveModel sieveModel)
        {
            return _adService.Get<AdResponse>(sieveModel);
        }

        [HttpPost]
        public async Task<AdResponse> Post([FromForm] AdCreateRequest createRequest)
        {
            var newEntry = await _adService.CreateNewAsync<AdResponse>(createRequest);
            return newEntry;
        }

        [HttpPut("{id}")]
        public async Task<AdResponse> Put(Guid id, [FromForm] AdUpdateRequest createRequest)
        {
            var updatedEntry = await _adService.UpdateAsync<AdResponse>(id, createRequest);
            return updatedEntry;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _adService.DeleteByIdAsync(id);
            return NoContent();
        }
    }
}