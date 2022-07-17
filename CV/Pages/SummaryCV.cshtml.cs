using CV.Data;
using CV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace CV.Pages
{
    public class SummaryCVModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        [FromQuery]
        public int cvId { get; set; }
        public UserViewModel userViewModel { get; set; }

        private readonly CvDbContext _dbContext;


        public SummaryCVModel(CvDbContext dbContext)
        {
            this._dbContext = dbContext;


        }
        public async Task<IActionResult> OnGetAsync()
        {

            User ?user = await _dbContext.User
                .Include(u => u.Skills).Include(u => u.Nationality)
                .Where(u => u.UserId == cvId)
                .FirstOrDefaultAsync();
            if (user == null)
                return NotFound();
            PopulateViewModel(user);


            return Page();
        }
        //public IActionResult OnPostGeneratePDF()
        //{
        //   HtmlToPdfConverter converter= new HtmlToPdfConverter();
        //    WebKitConverterSettings settings= new WebKitConverterSettings();
        //    settings.WebKitPath = Path.Combine(_hostingEnvironmet.ContentRootPath, "QtBinariesWindows");
        //    converter.ConverterSettings = settings;
        //    string s = "<div><form asp-page=\"SummaryCV\" asp-page-handler=\"GeneratePDF\" ><input type = \"Submit\" class=\"form-control\" value=\"GeneratePDF\"/></form></div>";
        //    PdfDocument document = converter.Convert(s, string.Empty);
        //    MemoryStream ms = new MemoryStream();
        //    document.Save(ms);
        //    document.Close(true);
        //    ms.Position = 0;
        //    FileStreamResult result = new FileStreamResult(ms,"application/pdf");
        //    result.FileDownloadName = "result.pdf";
        //    return result;
        //}
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
