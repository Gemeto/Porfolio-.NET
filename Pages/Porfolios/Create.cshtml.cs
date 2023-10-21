using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using WebApplicationTest.Models.Context;
using WebApplicationTest.Models.Domain;

namespace WebApplicationTest.Pages.Porfolios
{
    [Authorize(AuthenticationSchemes = "MyAuthScheme")]
    public class CreateModel : PageModel
    {
        private readonly PortafolioContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        [BindProperty]
        public JobExperience JobExperience { get; set; } = default!;
        public CreateModel(PortafolioContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult OnGet()
        {
        //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            JobExperience.UserId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (!ModelState.IsValid || _context.JobExperiences == null || JobExperience == null)
            {
                return Page();
            }

            _context.JobExperiences.Add(JobExperience);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Porfolios/Index");
        }
    }
}
