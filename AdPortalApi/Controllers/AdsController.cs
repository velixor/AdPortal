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
        private string BaseUrl =>
            $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}/api/ads";
        public AdsController(IAdService adService, IMapper mapper)
        {
            _adService = adService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<AdResponse>>> Get()
        {
            var ads = await _adService.GetAllAdsAsync();
            return Ok(_mapper.Map<List<AdResponse>>(ads));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdResponse>> Get([FromRoute]Guid id)
        {
            var ad = await _adService.GetAdByIdAsync(id);

            if (ad == null)
                return NotFound();

            return Ok(_mapper.Map<AdResponse>(ad));
        }

        [HttpPost]
        public async Task<ActionResult<AdResponse>> Post(AdRequest ad)
        {
            var newAd = await _adService.PostNewAdAsync(_mapper.Map<Ad>(ad));
            if (newAd == null)
                return BadRequest();
            var uri = BaseUrl + $"/{newAd.Id}";
            return Created(uri, _mapper.Map<AdResponse>(newAd));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, AdRequest ad)
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
