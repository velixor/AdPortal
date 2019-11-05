using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using Dto.Contracts.UserContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mvc.Services;
using Sieve.Models;

namespace Mvc.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IIdentityService _identityService;

        public UsersController(IUserService userService, IIdentityService identityService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        [AllowAnonymous]
        public IActionResult Index([FromQuery] SieveModel sieveModel)
        {
            var properties = typeof(UserResponse).GetProperties().Select(x => x.Name).ToList();
            if (sieveModel.Sorts != null)
            {
                var sortTerm = sieveModel.GetSortsParsed().First();
                properties.Remove(sortTerm.Name);
                ViewData[sortTerm.Name] = sortTerm.Descending
                    ? sortTerm.Name
                    : $"-{sortTerm.Name}";
            }
            properties.ForEach(x => ViewData[x] = x);
            
            ViewData["CurrentSieve"] = sieveModel;

            return View(_userService.Get<UserResponse>(sieveModel));
        }


        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                if (!_identityService.IsLogged) return NotFound();
                id = _identityService.User.Id;
            }

            var user = await _userService.GetByIdAsync<UserResponse>(id.Value);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete()
        {
            await _userService.DeleteByIdAsync(_identityService.User.Id, _identityService.User.Id);
            await Logout();
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Name,Email,Password,ConfirmPassword")]
            UserRegisterRequest user)
        {
            if (!ModelState.IsValid) return View(user);
            await _identityService.RegisterAsync(user);
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Password")] UserLoginRequest user)
        {
            if (!ModelState.IsValid) return View(user);
            var logged = await _identityService.LoginAsync(user);
            if (!logged)
            {
                ModelState.AddModelError("", "Incorrect login or password");
                return View(user);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Logout()
        {
            await _identityService.LogoutAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit()
        {
            var user = await _userService.GetByIdAsync<UserEdit>(_identityService.User.Id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Name, Email, NewPassword")] UserEdit user)
        {
            var updated =
                await _userService.UpdateAsync<UserResponse>(_identityService.User.Id, user, _identityService.User.Id);
            if (updated == null)
                return View(user);
            return RedirectToAction(nameof(Index));
        }
    }
}