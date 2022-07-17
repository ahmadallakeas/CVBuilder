using CV.Data;
using CV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CV.Pages
{
    public class CVDetailsModel : PageModel
    {
        private readonly CvDbContext _dbContext;
        public UserViewModel userViewModel { get; set; }
        [BindProperty(SupportsGet = true)]
        [FromQuery]
        public int id { get; set; }

        public CVDetailsModel(CvDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            User ?user = await _dbContext.User.Where(u => u.UserId == id)
                .Include(u => u.Nationality)
                .Include(u => u.Skills)
                .FirstOrDefaultAsync();

            if(user == null)
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
