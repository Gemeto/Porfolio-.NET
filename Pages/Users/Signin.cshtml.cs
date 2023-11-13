using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using PorfolioWeb.Models;
using PorfolioWeb.Services.Context;
using System.ComponentModel.DataAnnotations;
using PorfolioWeb.Services;

namespace PorfolioWeb.Pages
{
    public class Signin : PageModel
{
        [BindProperty]
        [Required]
        [StringLength(255)]
        public string Email { get; set; } = null!;

        [BindProperty]
        [Required]
        [StringLength(255)]
        public string Password { get; set; } = null!;

        [BindProperty]
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = null!;

        [BindProperty]
        [Required]
        [StringLength(255)]
        public string Surname { get; set; } = null!;

        private readonly PortafolioContextService _porfolioContext;
        private readonly EncryptSHA256Service _encryptSHA256;

        public Signin(PortafolioContextService portafolioContext, EncryptSHA256Service encryptSHA256)
        {
            _porfolioContext = portafolioContext;
            _encryptSHA256 = encryptSHA256;
        }

        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostAsync() {
            if (ModelState.IsValid)
            {
                WebUser? user = _porfolioContext.Users.FirstOrDefault(x => x.Email.ToLower() == Email.ToLower());
                if (user != null)
                {
                    ModelState.AddModelError("email", "El usuario ya existe.");
                }
                else if(Email != null && Password != null)
                {
                    user = new WebUser { Email = Email, Password = await _encryptSHA256.GetSHA256(Password), Name = Name, Surname = Surname };
                    await _porfolioContext.Users.AddAsync(user);
                    await _porfolioContext.SaveChangesAsync();
                    Response.Redirect("/Users/Login");
                }
            }
                return Page();
            }
    }
}