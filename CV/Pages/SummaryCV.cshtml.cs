using CV.Data;
using CV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;
using Microsoft.AspNetCore.Http.Extensions;


namespace CV.Pages
{
    public class SummaryCVModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        [FromQuery]
        public int cvId { get; set; }
        public UserViewModel userViewModel { get; set; }

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
        public void GeneratePDF()
        {
            //var Pdf = new GcPdfDocument();
            //var Page = Pdf.Pages.Add();
            //var Graphics = Page.Graphics;
            //Graphics.DrawHtml(System.IO.File.ReadAllText(hostEnvironment.ContentRootPath+"/Pages/Index.cshtml"), 72, 72, new HtmlToPdfFormat(false), out SizeF size);
            ////Save the PDF Document
            //Pdf.Save("Invoice.pdf");
            //var fn = @"webpage.pdf";

            ////Specify the url to be used for PDF conversion
            //var uri = new Uri(@HttpContext.Request.GetDisplayUrl());
            ////Create a GcHtmlRenderer instance.
            //using (var re = new GcHtmlRenderer(uri))
            //{
            //    //The PdfSettings instance is created to specify the pdf related settings that will show up in the generated PDF.
            //    var pdfSettings = new PdfSettings()
            //    {
            //        PageRanges = "1-100",
            //        Margins = new Margins(0.2f), // narrow margins all around
            //        IgnoreCSSPageSize = false, // try to ensure that we print the pages as we want
            //        Landscape = false
            //    };

            //    //Use the RenderToPdf method of RenderingExtension to generate the pdf file
            //    re.RenderToPdf(fn, pdfSettings);

            //}
          ;
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
