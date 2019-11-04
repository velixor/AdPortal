using System;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Services;
using Data.Models;
using Dto.Contracts;
using Dto.Contracts.UserContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }


        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<UserResponse> Get([FromRoute] Guid id)
        {
            var entry = await _userService.GetByIdAsync<UserResponse>(id);
            return entry;
        }

        // Get with filtering, sorting and paginating
        [AllowAnonymous]
        [HttpGet]
        public PagingResponse<UserResponse> Get([FromQuery] SieveModel sieveModel)
        {
            return _userService.Get<UserResponse>(sieveModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<UserResponse> Post([FromBody] UserRegisterRequest registerRequest)
        {
            var newEntry = await _userService.RegisterNewAsync<UserResponse>(registerRequest);
            return newEntry;
        }
        
        [AllowAnonymous]
        [HttpPost("auth")]
        public async Task<UserResponse> Post([FromBody] UserLoginRequest request)
        {
            var newEntry = await _userService.LoginAsync<UserResponse>(request);
            
            if (newEntry == null)
                throw new AuthenticationException("Username or password is incorrect" );

            return newEntry;
        }

        [HttpPut("{id}")]
        public async Task<UserResponse> Put(Guid id, [FromBody] UserRegisterRequest registerRequest)
        {
            var updatedEntry = await _userService.UpdateAsync<UserResponse>(id, registerRequest, UserId);
            return updatedEntry;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _userService.DeleteByIdAsync(id, UserId);
            return NoContent();
        }
        
        private Guid UserId =>
            Guid.Parse(User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
    }
}