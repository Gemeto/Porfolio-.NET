using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplicationTest.Models.Domain;

namespace WebApplicationTest.Pages.Porfolios
{
    [Authorize(AuthenticationSchemes = "MyAuthScheme")]
    public class DeleteModel : PageModel
    {
        private readonly WebApplicationTest.Models.Context.PortafolioContext _context;

        public DeleteModel(WebApplicationTest.Models.Context.PortafolioContext context)
        {
            _context = context;
        }

        [BindProperty]
      public JobExperience JobExperience { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.JobExperiences == null)
            {
                return NotFound();
            }

            var jobexperience = await _context.JobExperiences.FirstOrDefaultAsync(m => m.Id == id);

            if (jobexperience == null)
            {
                return NotFound();
            }
            else 
            {
                JobExperience = jobexperience;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.JobExperiences == null)
            {
                return NotFound();
            }
            var jobexperience = await _context.JobExperiences.FindAsync(id);

            if (jobexperience != null)
            {
                JobExperience = jobexperience;
                _context.JobExperiences.Remove(JobExperience);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
