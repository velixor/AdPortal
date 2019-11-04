using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class Test : PageModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public void OnGet()
        {
            Id = User.Claims.Single(x=>x.Type=="Id").Value;
            Email = User.Identity.Name;
        }
    }
}