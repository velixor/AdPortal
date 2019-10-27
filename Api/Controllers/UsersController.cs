using System;
using System.Threading.Tasks;
using Core.Services;
using Dto.Contracts;
using Dto.Contracts.UserContracts;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }


        [HttpGet("{id}")]
        public async Task<UserResponse> Get([FromRoute] Guid id)
        {
            var entry = await _userService.GetByIdAsync<UserResponse>(id);
            return entry;
        }

        // Get with filtering, sorting and paginating
        [HttpGet]
        public PagingResponse<UserResponse> Get([FromQuery] SieveModel sieveModel)
        {
            return _userService.Get<UserResponse>(sieveModel);
        }

        [HttpPost]
        public async Task<UserResponse> Post([FromBody] UserRequest request)
        {
            var newEntry = await _userService.CreateNewAsync<UserResponse>(request);
            return newEntry;
        }

        [HttpPut("{id}")]
        public async Task<UserResponse> Put(Guid id, [FromBody] UserRequest request)
        {
            var updatedEntry = await _userService.UpdateAsync<UserResponse>(id, request);
            return updatedEntry;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _userService.DeleteByIdAsync(id);
            return NoContent();
        }
    }
}