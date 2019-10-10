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
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        private string BaseUrl =>
            $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}/api/users";

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserResponse>>> Get()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(_mapper.Map<List<UserResponse>>(users));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> Get([FromRoute] Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(_mapper.Map<UserResponse>(user));
        }

        [HttpPost]
        public async Task<ActionResult<UserResponse>> Post([FromBody] UserRequest user)
        {
            var registeredUser = await _userService.RegisterNewUserAsync(_mapper.Map<User>(user));
            if (registeredUser == null)
                return BadRequest();
            var uri = BaseUrl + $"/{registeredUser.Id}";
            return Created(uri, _mapper.Map<UserResponse>(registeredUser));
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