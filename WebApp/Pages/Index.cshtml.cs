using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Data;
using Data.Models;
using Dto.Contracts;
using Dto.Contracts.UserContracts;
using Sieve.Models;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _service;

        public IndexModel(IUserService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public PagingResponse<UserResponse> Users { get;set; }

        public void OnGetAsync()
        {
            Users = _service.Get<UserResponse>(new SieveModel());
        }
    }
}
