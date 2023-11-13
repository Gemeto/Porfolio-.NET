using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PorfolioWeb.Models;
using PorfolioWeb.Services.Context;

namespace PorfolioWeb.Pages.Porfolios
{
    [Authorize(AuthenticationSchemes = "MyAuthScheme")]
    public class IndexModel : PageModel
    {
        private readonly PortafolioContextService _context;

        public IndexModel(PortafolioContextService context)
        {
            _context = context;
        }

        public IList<JobExperience> JobExperience { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.JobExperiences != null)
            {
                JobExperience = await _context.JobExperiences
                .Include(j => j.User).ToListAsync();
            }
        }
    }
}
