using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplicationTest.Models.Context;
using WebApplicationTest.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using WebApplicationTest.Models;
using WebApplicationTest.Services;

namespace WebApplicationTest.Pages
{
    public class Signin : PageModel
    {
        private readonly PortafolioContext _porfolioContext;
        private readonly EncryptSHA256 _encryptSHA256;
        [BindProperty]
        public AddUserViewModel AddUser { get; set; }
        public Signin(PortafolioContext portafolioContext, EncryptSHA256 encryptSHA256)
        {
            _porfolioContext = portafolioContext;
            _encryptSHA256 = encryptSHA256;
            AddUser = new AddUserViewModel();
        }

        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostAsync() {
            if (ModelState.IsValid)
            {
                string? email = Request.Form["email"];
                string? password = _encryptSHA256.GetSHA256(Request.Form["password"]);
                User? user = _porfolioContext.Users.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
                if (user != null)
                {
                    ModelState.AddModelError("email", "El usuario ya existe.");
                }
                else if(email != null && password != null)
                {
                    string? name = Request.Form["name"];
                    string? surname = Request.Form["surname"];
                    user = new User { Email = email, Password = password, Name = name??"", Surname = surname??"" };
                    await _porfolioContext.Users.AddAsync(user);
                    await _porfolioContext.SaveChangesAsync();
                    Response.Redirect("/Users/Login");
                }
            }
                return Page();
            }
    }
}