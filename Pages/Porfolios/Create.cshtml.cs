using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using PorfolioWeb.Widgets;
using PorfolioWeb.Services.Context;
using PorfolioWeb.Models;

namespace PorfolioWeb.Pages.Porfolios
{
    [Authorize(AuthenticationSchemes = "MyAuthScheme")]
    public class CreateModel : PageModel
    {
        private readonly PortafolioContextService _context;
        private readonly HttpContext? httpContext;
        private IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public JobExperience JobExperience { get; set; } = default!;

        [BindProperty]
        public IFormFile? mainImage { get; set; } = default;

        public CreateModel(PortafolioContextService context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            httpContext = httpContextAccessor.HttpContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _setModelUserId();

            if (!ModelState.IsValid || _context.JobExperiences == null || JobExperience == null)
            {
                return Page();
            }

            if(mainImage != null)
            {
                FileUploader<Image> fileUploader = new FileUploader<Image>(mainImage,
                    mainImage.FileName,
                    Path.Combine(_webHostEnvironment.ContentRootPath,
                    "uploads",
                    mainImage.FileName
                    )
                );

                JobExperience.Image = (Image?)await fileUploader.UploadFileAsync();
            }

            _context.JobExperiences.Add(JobExperience);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Porfolios/Index");
        }

        private void _setModelUserId()
        {
            String? userId = httpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                JobExperience.UserId = int.Parse(userId);
            }
        }
    }
}
