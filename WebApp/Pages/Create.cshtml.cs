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

namespace WebApp.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IUserService _service;

        public CreateModel(IUserService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public UserCreateRequest User { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            await _service.CreateNewAsync<UserResponse>(User);

            return RedirectToPage("./Index");
        }
    }
}
