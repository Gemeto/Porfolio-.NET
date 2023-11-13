using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using PorfolioWeb.Models;
using PorfolioWeb.Services.Context;
using System.ComponentModel.DataAnnotations;
using PorfolioWeb.Services;

namespace PorfolioWeb.Pages
{
    public class Login : PageModel
    {
        [BindProperty]
        [Required]
        [StringLength(255)]
        public string Email { get; set; } = null!;

        [BindProperty]
        [Required]
        [StringLength(255)]
        public string Password { get; set; } = null!;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly PortafolioContextService _porfolioContext;
        private readonly EncryptSHA256Service _encryptSHA256;

        public Login(IHttpContextAccessor httpContextAccessor, PortafolioContextService portafolioContext, EncryptSHA256Service encryptSHA256)
        {
            _httpContextAccessor = httpContextAccessor;
            _porfolioContext = portafolioContext;
            _encryptSHA256 = encryptSHA256;
        }

        public void OnGet()
        {
            
        }

        public async Task<IActionResult?> OnPostAsync() {
            if (ModelState.IsValid)
            {
                IFormCollection form = await Request.ReadFormAsync();
                if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password))
                {
                    String password = await _encryptSHA256.GetSHA256(Password);
                    WebUser? user = _porfolioContext.Users.FirstOrDefault(x => x.Email.ToLower() == Email.ToLower() && x.Password == password);
                    if (user != null)
                    {
                        CookieLogin(user);
                        Response.Redirect("/" + user.Id);
                        return null;
                    }
                }
            }
            return Page();
        }

        private async void CookieLogin(WebUser user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "MyAuthScheme");

            await _httpContextAccessor.HttpContext.SignInAsync("MyAuthScheme",
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties());
        }
    }
}