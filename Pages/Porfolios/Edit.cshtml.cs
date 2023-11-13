using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PorfolioWeb.Widgets;
using PorfolioWeb.Services.Context;
using PorfolioWeb.Models;

namespace PorfolioWeb.Pages.Porfolios
{
    [Authorize(AuthenticationSchemes = "MyAuthScheme")]
    public class EditModel : PageModel
    {
        private readonly PortafolioContextService _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public JobExperience JobExperience { get; set; }

        [BindProperty]
        public IFormFile? mainImage { get; set; }

        public EditModel(PortafolioContextService context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.JobExperiences == null)
            {
                return NotFound();
            }

            JobExperience =  await _context.JobExperiences.Include(jexp => jexp.Image).FirstOrDefaultAsync(m => m.Id == id);
            if (JobExperience == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            JobExperience? jobExperience = await _retrieveModel(id);
            if (jobExperience == null || !ModelState.IsValid)
            {
                return Page();
            }

            jobExperience.Title = JobExperience.Title;
            jobExperience.Description = JobExperience.Description;
            jobExperience.Link = JobExperience.Link;

            if (mainImage != null)
            {
                if(jobExperience.Image != null)
                {
                    FileInfo file = new FileInfo(_webHostEnvironment.ContentRootPath + "/wwwroot" + jobExperience.Image.Path);
                    file.Delete();
                    _context.Images.Remove(jobExperience.Image);
                }

                FileUploader<Image> fileUploader = new FileUploader<Image>(mainImage,
                mainImage.FileName,
                    Path.Combine(_webHostEnvironment.ContentRootPath,
                    "wwwroot/uploads",
                    mainImage.FileName
                    )
                );

                jobExperience.Image = (Image?)await fileUploader.UploadFileAsync();
            }

            _context.Attach(jobExperience).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        private async Task<JobExperience?> _retrieveModel(int id)
        {
            if (id > 0)
            {
                return await _context.JobExperiences.Include(jexp => jexp.Image).FirstOrDefaultAsync(j => j.Id == id);
            }

            return null;
        }
    }
}
