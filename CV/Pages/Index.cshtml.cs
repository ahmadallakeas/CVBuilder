using CV.Data;
using CV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace CV.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        [FileExtensions(Extensions = "png,jpg,jpeg")]
        public IFormFile Image { get; set; }
        public readonly CvDbContext _DBContext;
        private readonly IWebHostEnvironment webHostEnvironment;

      
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(CvDbContext dBContext, IWebHostEnvironment webHostEnvironment)
        {
            _DBContext = dBContext;
            this.webHostEnvironment = webHostEnvironment;
        }

        public void OnGet()
        {


        }
        public void OnPostTest()
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please upload an image");
            }
            ProcessUploadedFile();
        }
        private string ProcessUploadedFile()
        {
            string uniqueFileName = null;

            if (Image != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString()+ Path.GetExtension(Image.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Image.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
        bool ValidateImage(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            switch (ext.ToLower())
            {
              
                case ".jpg":
                    return true;
                case ".jpeg":
                    return true;
                case ".png":
                    return true;
                default:
                    return false;
            }
        }
    }
}