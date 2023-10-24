using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PorfolioWeb.Models.Context;
using PorfolioWeb.Models.Domain;

namespace PorfolioWeb.Pages
{
    public class GridViewModel : PageModel
    {
        public List<JobExperience> jobExperiences;
        public User? mainUser;

        private readonly PortafolioContext _porfolioContext;

        public GridViewModel(PortafolioContext porfolioContext)
        {
            _porfolioContext = porfolioContext;
            jobExperiences = new List<JobExperience>();
            mainUser = null;
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
