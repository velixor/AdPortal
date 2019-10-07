using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdPortalApi.Contracts.Requests;
using AdPortalApi.Contracts.Responses;
using AdPortalApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdPortalApi.Models;
using AdPortalApi.Services;
using AutoMapper;

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
            // TODO Warning mapping could be wrong
            var newAd = await _adService.PostNewAdAsync(_mapper.Map<Ad>(ad));
            if (newAd == null)
                return BadRequest();
            var uri = BaseUrl + $"/{newAd.Id}";
            return Created(uri, _mapper.Map<AdResponse>(newAd));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, AdRequest ad)
        {
            var newAd = _mapper.Map<Ad>(ad);
            newAd.Id = id;

            var modified = await _adService.UpdateAdAsync(newAd);

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
