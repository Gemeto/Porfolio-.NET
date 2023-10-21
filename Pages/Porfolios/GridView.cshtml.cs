using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplicationTest.Models.Context;
using WebApplicationTest.Models.Domain;

namespace WebApplicationTest.Pages
{
    public class GridViewModel : PageModel
    {
        public List<JobExperience> jobExperiences;
        
        private readonly PortafolioContext _porfolioContext;

        public GridViewModel(PortafolioContext porfolioContext)
        {
            _porfolioContext = porfolioContext;
            jobExperiences = new List<JobExperience>();
        }

        public async Task OnGet()
        {
            jobExperiences = await _porfolioContext.JobExperiences.ToListAsync();
        }
    }
}
