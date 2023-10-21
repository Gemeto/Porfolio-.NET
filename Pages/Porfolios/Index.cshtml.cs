using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplicationTest.Models.Context;
using WebApplicationTest.Models.Domain;

namespace WebApplicationTest.Pages.Porfolios
{
    [Authorize(AuthenticationSchemes = "MyAuthScheme")]
    public class IndexModel : PageModel
    {
        private readonly PortafolioContext _context;

        public IndexModel(PortafolioContext context)
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
