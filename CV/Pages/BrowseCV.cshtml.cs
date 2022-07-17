using CV.Data;
using CV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CV.Pages
{
    public class BrowseCVModel : PageModel
    {
        private readonly CvDbContext _dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        [BindProperty]
        public int id { get; set; }
        public List<User> UsersList { get; set; }
        public BrowseCVModel(CvDbContext DbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = DbContext;
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> OnGet()
        {
            UsersList = await _dbContext.User.Include(u => u.Nationality).Include(u => u.Skills).ToListAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostDelete()
        {
            User user = await _dbContext.User.Where(u => u.UserId == id).FirstOrDefaultAsync();
            string filePath = Path.Combine(webHostEnvironment.WebRootPath,
             "images", user.ImagePath);
            System.IO.File.Delete(filePath);
            _dbContext.Remove(user);
            _dbContext.SaveChanges();
            return RedirectToPage("BrowseCV");
        }
    }
}
