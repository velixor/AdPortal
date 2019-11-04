using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Core.Services;
using Dto.Contracts.UserContracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace Mvc.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IUserService _userService;
        private readonly HttpContext _httpContext;
        private readonly IMapper _mapper;

        public IdentityService(IUserService userService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _httpContext = httpContextAccessor?.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<bool> LoginAsync(UserLoginRequest loginRequest)
        {
            var user = await _userService.LoginAsync<UserLoggedResponse>(loginRequest);
            if (user == null)
            {
                return false;
            }

            var claims = new List<Claim>
            {
                new Claim(nameof(user.Id), user.Id.ToString()),
                new Claim(nameof(user.Email), user.Email)
            };

            var id =
                new ClaimsIdentity(claims, "ApplicationCookie", nameof(user.Email),
                    ClaimsIdentity.DefaultRoleClaimType);

            await _httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            return true;
        }

        public async Task LogoutAsync()
        {
            await _httpContext.SignOutAsync();
        }

        public async Task RegisterAsync(UserRegisterRequest registerRequest)
        {
            var loginRequest = _mapper.Map<UserLoginRequest>(registerRequest);
            await _userService.RegisterNewAsync(registerRequest);
            await LoginAsync(loginRequest);
        }

        public UserInfo User
        {
            get
            {
                var email = _httpContext.User.Claims.SingleOrDefault(x => x.Type == "Email")?.Value;
                Guid.TryParse(_httpContext.User.Claims.SingleOrDefault(x => x.Type == "Id")?.Value, out var id);
                
                return new UserInfo
                {
                    Email = email,
                    Id = id
                };
            }
        }

        public bool IsLogged => User.Email != null && User.Id != Guid.Empty;
    }

    public class UserInfo
    {
        public string Email { get; set; }
        public Guid Id { get; set; }
    }
}