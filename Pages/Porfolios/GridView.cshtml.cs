using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApplicationTest.Models.Context;
using WebApplicationTest.Models.Domain;

namespace WebApplicationTest.Pages
{
    public class GridViewModel : PageModel
    {
        public List<JobExperience> jobExperiences;
        public User mainUser;

        private readonly PortafolioContext _porfolioContext;

        public GridViewModel(PortafolioContext porfolioContext)
        {
            _porfolioContext = porfolioContext;
            jobExperiences = new List<JobExperience>();
        }

        public async Task OnGet(int? userId)
        {
            if (userId == null)
            {
                mainUser = await _porfolioContext.Users.FirstOrDefaultAsync();
            }
            else
            {
                mainUser = await _porfolioContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            }
            jobExperiences = await _porfolioContext.JobExperiences.Where(x => x.UserId == mainUser.Id).ToListAsync();
        }
    }
}
