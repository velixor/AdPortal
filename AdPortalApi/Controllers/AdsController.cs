using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdPortalApi.Models;
using AdPortalApi.Services;
using AutoMapper;
using Dtos.Contracts.Requests;
using Dtos.Contracts.Responses;

namespace AdPortalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdsController : ControllerBase
    {
        private readonly IAdService _adService;
        private readonly IMapper _mapper;

        public AdsController(IAdService adService, IMapper mapper)
        {
            _adService = adService ?? throw new ArgumentNullException(nameof(adService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<List<AdResponse>> Get()
        {
            var ads = await _adService.GetAllAdsAsync();
            return _mapper.Map<List<AdResponse>>(ads);
        }

        [HttpGet("{id}")]
        public async Task<AdResponse> Get([FromRoute]Guid id)
        {
            var ad = await _adService.GetAdByIdAsync(id);
            return _mapper.Map<AdResponse>(ad);
        }

        [HttpPost]
        public async Task<AdResponse> Post(AdRequest ad)
        {
            var newAd = await _adService.PostNewAdAsync(_mapper.Map<Ad>(ad));
            return _mapper.Map<AdResponse>(newAd);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, AdRequest ad)
        {
            var newAdd = await _adService.GetAdByIdAsync(id);
            newAdd.Content = ad.Content;
            newAdd.ImagePath = ad.ImagePath;

            var modified = await _adService.UpdateAdAsync(newAdd);

            if (!modified)
                return NotFound();

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (!await _adService.DeleteAdByIdAsync(id))
                return NotFound();

            return NoContent();
        }
    }
}
