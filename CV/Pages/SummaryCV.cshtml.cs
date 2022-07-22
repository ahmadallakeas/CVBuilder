using CV.Data;
using CV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;
using Microsoft.AspNetCore.Http.Extensions;
using GrapeCity.Documents.Html;
using GrapeCity.Documents.Pdf;

namespace CV.Pages
{
    public class SummaryCVModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        [FromQuery]
        public int cvId { get; set; }
        public UserViewModel userViewModel { get; set; }
        public object MimeTypes { get; private set; }

        private readonly CvDbContext _dbContext;
        private readonly IHostEnvironment hostEnvironment;

        public SummaryCVModel(CvDbContext dbContext, IHostEnvironment hostEnvironment)
        {
            this._dbContext = dbContext;
            this.hostEnvironment = hostEnvironment;
        }
        public async Task<IActionResult> OnGetAsync()
        {

            User? user = await _dbContext.User
                .Include(u => u.Skills).Include(u => u.Nationality)
                .Where(u => u.UserId == cvId)
                .FirstOrDefaultAsync();
            if (user == null)
                return NotFound();
            PopulateViewModel(user);

            return Page();
        }
       
        protected void PopulateViewModel(User user)
        {
            userViewModel = new UserViewModel();
            userViewModel.FullName = user.FirstName + " " + user.LastName;
            userViewModel.Birthdate = user.DateOfBirth.ToShortDateString();
            userViewModel.Nationality = user.Nationality.Country;
            userViewModel.Gender = user.Gender;
            userViewModel.Image = user.ImagePath;
            userViewModel.Grade = user.Grade;
            foreach (var skill in user.Skills)
            {
                userViewModel.Skills.Add(skill.SkillName);
            }

        }
    }
}
