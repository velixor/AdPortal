using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Data;
using Data.Models;
using Dto.Contracts.UserContracts;
using WebApp.Services;

namespace WebApp.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IIdentityService _identityService;

        public CreateModel(IIdentityService identityService)
        {
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public UserRegisterRequest UserRegister { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            await _identityService.RegisterAsync(UserRegister);

            return RedirectToPage("./Index");
        }
    }
}
