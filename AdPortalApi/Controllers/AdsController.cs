using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdPortalApi.Models;
using AdPortalApi.Services;
using AutoMapper;
using Dto.Contracts;
using Dto.Contracts.AdContracts;
using Sieve.Models;
using Sieve.Services;

namespace AdPortalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdsController : ControllerBase
    {
        private readonly IAdService _adService;
        private readonly IMapper _mapper;
        private readonly ISieveProcessor _sieveProcessor;
        private readonly IImageService _imageService;

        public AdsController(IAdService adService, IMapper mapper, ISieveProcessor sieveProcessor, IImageService imageService)
        {
            _adService = adService ?? throw new ArgumentNullException(nameof(adService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _sieveProcessor = sieveProcessor ?? throw new ArgumentNullException(nameof(sieveProcessor));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
        }

        [HttpGet]
        public PagingResponse<AdResponse> Get([FromQuery] SieveModel sieveModel)
        {
            var ads = _adService.GetAllAds();
            ads = _sieveProcessor.Apply(sieveModel, ads, applyPagination: false);
            var count = ads.Count();
            ads = _sieveProcessor.Apply(sieveModel, ads, applyFiltering: false, applySorting: false);

            var response = new PagingResponse<AdResponse>
            {
                Items = _mapper.Map<List<AdResponse>>(ads),
                Count = count
            };
            return response;
        }

        [HttpGet("{id}")]
        public async Task<AdResponse> Get([FromRoute] Guid id)
        {
            var ad = await _adService.GetAdByIdAsync(id);
            return _mapper.Map<AdResponse>(ad);
        }

        [HttpPost]
        public async Task<AdResponse> Post([FromForm]AdRequest ad)
        {
            var mappedAd = _mapper.Map<Ad>(ad);
            mappedAd.ImageName = _imageService.UploadImage(ad.Image);
            var newAd = await _adService.PostNewAdAsync(mappedAd);
            return _mapper.Map<AdResponse>(newAd);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, AdRequest ad)
        {
            var newAdd = await _adService.GetAdByIdAsync(id);
            newAdd.Content = ad.Content;
            // TODO update problems
//            newAdd.ImagePath = ad.Image;

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