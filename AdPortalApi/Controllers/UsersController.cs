using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdPortalApi.Models;
using AdPortalApi.Services;
using AutoMapper;
using Dto.Contracts;
using Dto.Contracts.UserContracts;
using Sieve.Models;
using Sieve.Services;

namespace AdPortalApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ISieveProcessor _sieveProcessor;

        public UsersController(IUserService userService, IMapper mapper, ISieveProcessor sieveProcessor)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _sieveProcessor = sieveProcessor ?? throw new ArgumentNullException(nameof(sieveProcessor));
        }

        [HttpGet]
        public PagingResponse<UserResponse> Get([FromQuery] SieveModel sieveModel)
        {
            var users = _userService.GetAllUsers();
            users = _sieveProcessor.Apply(sieveModel, users, applyPagination: false);
            var count = users.Count();
            users = _sieveProcessor.Apply(sieveModel, users, applyFiltering: false, applySorting: false);

            var response = new PagingResponse<UserResponse>
            {
                Items = _mapper.Map<List<UserResponse>>(users),
                Count = count
            };
            return response;
        }

        [HttpGet("{id}")]
        public async Task<UserResponse> Get([FromRoute] Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return _mapper.Map<UserResponse>(user);
        }

        [HttpPost]
        public async Task<UserResponse> Post([FromBody] UserRequest user)
        {
            var registeredUser = await _userService.RegisterNewUserAsync(_mapper.Map<User>(user));
            return _mapper.Map<UserResponse>(registeredUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] UserRequest user)
        {
            var newUser = _mapper.Map<User>(user);
            newUser.Id = id;

            var modified = await _userService.UpdateUserAsync(newUser);

            if (!modified)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            if (!await _userService.DeleteUserByIdAsync(id))
                return NotFound();

            return NoContent();
        }
    }
}