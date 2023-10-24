using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using PorfolioWeb.Models.Context;
using PorfolioWeb.Models.Domain;

namespace PorfolioWeb.Pages.Porfolios
{
    [Authorize(AuthenticationSchemes = "MyAuthScheme")]
    public class EditModel : PageModel
    {
        private readonly PortafolioContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EditModel(PortafolioContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
        public JobExperience JobExperience { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.JobExperiences == null)
            {
                return NotFound();
            }

            var jobexperience =  await _context.JobExperiences.FirstOrDefaultAsync(m => m.Id == id);
            if (jobexperience == null)
            {
                return NotFound();
            }
            JobExperience = jobexperience;
           ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            JobExperience.UserId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(JobExperience).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExperienceExists(JobExperience.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool JobExperienceExists(int id)
        {
          return (_context.JobExperiences?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
