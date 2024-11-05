using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TaskManager.Pages.Account
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        public string Username { get; set; }

        public async Task OnGet()
        {
            Username = User.Identity.Name ?? string.Empty;
        }
    }
}
