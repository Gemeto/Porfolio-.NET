using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PorfolioWeb.Models;
using PorfolioWeb.Services.Context;
using PorfolioWeb.Widgets;
using System.Security.Claims;

namespace PorfolioWeb.Pages.Users
{
    [Authorize(AuthenticationSchemes = "MyAuthScheme")]
    public class EditModel : PageModel
    {
        private readonly PortafolioContextService _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EditModel(PortafolioContextService context, IHttpContextAccessor contextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public WebUser MainUser { get; set; } = default!;

        [BindProperty]
        public IFormFile? MainImage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user =  await _context.Users.Include( u => u.MainImage).FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            MainUser = user;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            MainUser = await _retrieveUserModel();
            if(MainImage != null)
            {
                FileUploader<Image> fUp = new FileUploader<Image>(MainImage, MainImage.FileName, 
                    Path.Combine(_webHostEnvironment.ContentRootPath,
                    "wwwroot/uploads",
                    MainImage.FileName
                    )
                );
                MainUser.MainImage = (Image?)await fUp.UploadFileAsync();
            }
            _context.Attach(MainUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(MainUser.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Response.Redirect("/" + MainUser.Id);
            return null;
        }

        private async Task<WebUser> _retrieveUserModel()
        {
            return await _context.Users.Include(u => u.MainImage).FirstOrDefaultAsync( x => x.Id ==  int.Parse(_contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value));
        }

        private bool UserExists(int id)
        {
          return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
