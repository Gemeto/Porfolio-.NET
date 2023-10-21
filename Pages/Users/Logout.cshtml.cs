using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplicationTest.Pages.Users
{
    [Authorize(AuthenticationSchemes="MyAuthScheme")]
    public class LogoutModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LogoutModel(IHttpContextAccessor httpContextAccessor) { 
            _httpContextAccessor = httpContextAccessor;
        }
        public async void OnGet()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync("MyAuthScheme");
            Response.Redirect("/");
        }

        public async Task MyCustomSignOut(string redirectUri)
        {
            await _httpContextAccessor.HttpContext.SignOutAsync("MyAuthScheme");
            var prop = new AuthenticationProperties()
            {
                RedirectUri = "/Users/Login"
            };
            await _httpContextAccessor.HttpContext.SignOutAsync("MyAuthScheme", prop);
        }
    }
}
