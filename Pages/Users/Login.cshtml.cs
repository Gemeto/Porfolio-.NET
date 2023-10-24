using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PorfolioWeb.Models.Context;
using PorfolioWeb.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using PorfolioWeb.Models;
using PorfolioWeb.Services;

namespace PorfolioWeb.Pages
{
    public class Login : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly PortafolioContext _porfolioContext;
        private readonly EncryptSHA256 _encryptSHA256;
        [BindProperty]
        public LoginUserViewModel loginUserViewModel { get; set; }
        public Login(IHttpContextAccessor httpContextAccessor, PortafolioContext portafolioContext, EncryptSHA256 encryptSHA256)
        {
            _httpContextAccessor = httpContextAccessor;
            _porfolioContext = portafolioContext;
            _encryptSHA256 = encryptSHA256;
            loginUserViewModel = new LoginUserViewModel();
        }

        public void OnGet()
        {
            
        }

        public async Task<IActionResult?> OnPostAsync() {
            if (ModelState.IsValid)
            {
                IFormCollection form = await Request.ReadFormAsync();
                string? email = form["email"];
                string? password = form["password"];
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    password = _encryptSHA256.GetSHA256(password);
                    User? user = _porfolioContext.Users.FirstOrDefault(x => x.Email.ToLower() == email.ToLower() && x.Password == password);
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

        private async void CookieLogin(User user)
        {
            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email)};

            var claimsIdentity = new ClaimsIdentity(claims, "MyAuthScheme");

            await _httpContextAccessor.HttpContext
                .SignInAsync("MyAuthScheme",
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties());
        }
    }
}