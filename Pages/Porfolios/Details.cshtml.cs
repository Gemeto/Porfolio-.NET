using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PorfolioWeb.Models.Context;
using PorfolioWeb.Models.Domain;

namespace PorfolioWeb.Pages.Porfolios
{
    [Authorize(AuthenticationSchemes = "MyAuthScheme")]
    public class DetailsModel : PageModel
    {
        private readonly PortafolioContext _context;

        public DetailsModel(PortafolioContext context)
        {
            _context = context;
        }

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
    }
}
