using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Services;
using Dto.Contracts.UserContracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Services;

namespace WebApp.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IIdentityService _identityService;

        public LoginModel(IIdentityService identityService)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        [BindProperty] public UserLoginRequest UserLogin { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var logged = await _identityService.LoginAsync(UserLogin);
            if (!logged)
            {
                ModelState.AddModelError("", "Incorrect login and (or) password");
                return Page();
            }
            else
            {
                return RedirectToPage("Index");
            }
        }
    }
}