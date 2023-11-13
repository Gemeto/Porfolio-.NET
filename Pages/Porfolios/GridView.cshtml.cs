using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PorfolioWeb.Models;
using PorfolioWeb.Services.Context;

namespace PorfolioWeb.Pages
{
    public class GridViewModel : PageModel
    {
        public List<JobExperience> jobExperiences;
        public WebUser? mainUser;

        private readonly PortafolioContextService _porfolioContext;

        public GridViewModel(PortafolioContextService porfolioContext)
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
                mainUser = await _porfolioContext.Users.Include(u => u.MainImage).FirstOrDefaultAsync(x => x.Id == userId);

            }
            jobExperiences = await _porfolioContext.JobExperiences.Where(x => x.UserId == mainUser.Id).Include( jexp => jexp.Image).ToListAsync();
        }
    }
}
