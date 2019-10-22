using System;
using System.Threading.Tasks;
using Core.Services;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Dto.Contracts;
using Sieve.Models;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class EntityBaseController<T, TRequest, TResponse> : ControllerBase
        where T : class, IEntity
        where TRequest : class
        where TResponse : class
    {
        private readonly IEntityBaseService<T, TRequest, TResponse> _entityService;

        public EntityBaseController(IEntityBaseService<T, TRequest, TResponse> entityService)
        {
            _entityService = entityService ?? throw new ArgumentNullException(nameof(entityService));
        }

        [HttpGet("{id}")]
        public async Task<TResponse> Get([FromRoute] Guid id)
        {
            var entry = await _entityService.GetByIdAsync(id);
            return entry;
        }

        // Get with filtering, sorting and paginating
        [HttpGet]
        public PagingResponse<TResponse> Get([FromQuery] SieveModel sieveModel)
        {
            return _entityService.Get(sieveModel);
        }

        [HttpPost]
        public async Task<TResponse> Post([FromForm] TRequest request)
        {
            var newEntry = await _entityService.CreateNewAsync(request);
            return newEntry;
        }

        [HttpPut("{id}")]
        public async Task<TResponse> Put(Guid id, [FromForm] TRequest request)
        {
            var updatedEntry = await _entityService.UpdateAsync(id, request);
            return updatedEntry;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _entityService.DeleteByIdAsync(id);
            return NoContent();
        }
    }
}