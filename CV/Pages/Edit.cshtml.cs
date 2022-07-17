using CV.Data;
using CV.Models;
using CV.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CV.Pages
{
    public class EditModel : PageModel
    {
        private readonly CvDbContext _dbContext;

        [BindProperty]
        public int idFromPost { get; set; }
        [BindProperty(SupportsGet = true)]

        public int id { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        [BindProperty]
        public int firstNumber { get; set; }
        [BindProperty]
        public int secondNumber { get; set; }
        public User user { get; set; }
        [BindProperty]
        [Required]
        public List<string> Skills { get; set; }

        public IEnumerable<Nationality> nationalities { get; set; }

        public IEnumerable<Skill> skills { get; set; }
        public List<Skill> mylist;
        public IEnumerable<SelectListItem> listItems;
        public string[] genderList = new[] { "Male", "Female" };
        private readonly CvDbContext _db;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly Grade gradeService;

        [BindProperty]
        public EditInputModel EditInputModel { get; set; }

        public EditModel(CvDbContext dbContext, IWebHostEnvironment webHostEnvironment, Grade GradeService)
        {
            this.webHostEnvironment = webHostEnvironment;
            gradeService = GradeService;
            _dbContext = dbContext;

        }
        public async Task<IActionResult> OnGetAsync()
        {
            User user = _dbContext.User.Where(u => u.UserId == id).FirstOrDefault();
            if (user == null)
                return NotFound();
            PopulateData();
            return Page();
        }
        public async Task<IActionResult> OnPostEdit()
        {
            int res = 0;
            if (!ModelState.IsValid)
            {

                PopulateData();
                return Page();
            }

            if (EditInputModel.Image != null)
            {
                if (!ValidateImage(EditInputModel.Image.FileName))
                {
                    ModelState.AddModelError("WrongFormat", "Please upload a suitable image");
                    res = -1;
                }
            }
            if (Convert.ToInt32(EditInputModel.Sum) != firstNumber + secondNumber)
            {
                ModelState.AddModelError("WrongSum", "Sum is incorrect");
                res = -1;
            }
            if (EditInputModel.Email != EditInputModel.ConfirmEmail)
            {
                ModelState.AddModelError("WrongEmail", "Emails are not equal");
                res = -1;
            }
            if (DateTime.Compare(DateTime.Now, EditInputModel.Birthdate) < 0)
            {
                ModelState.AddModelError("WrongDate", "Please select a correct date");
                res = -1;
            }

            if (res > -1)
            {
                res = await EditCV();

            }
            if (res > 0)
            {
                return Redirect("/SummaryCV?cvId=" + res);
            }

            PopulateData();
            return Page();

        }
        protected void PopulateData()
        {
            user = _dbContext.User.Where(u => u.UserId == id).Include(u => u.Skills).Include(u => u.Nationality).FirstOrDefault();
            Random rnd = new Random();
            x = rnd.Next(1, 20);
            y = rnd.Next(20, 50);
            nationalities = _dbContext.Nationality.ToList();
            listItems = nationalities.OrderBy(s => s.Country)
                .Select(i => new SelectListItem
                {

                    Value = i.NationalityId.ToString(),
                    Text = i.Country,


                });

            skills = _dbContext.Skill.ToList();
            PopulateEditModel();
        }
        private string ProcessUploadedFile()
        {
            string uniqueFileName = null;

            if (EditInputModel.Image != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(EditInputModel.Image.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    EditInputModel.Image.CopyTo(fileStream);
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
        protected void PopulateEditModel()
        {
            // user = _dbContext.User.Where(u => u.UserId == id).Include(u => u.Skills).Include(u => u.Nationality).FirstOrDefault();
            EditInputModel = new EditInputModel();
            EditInputModel.FirstName = user.FirstName;
            EditInputModel.LastName = user.LastName;
            EditInputModel.Birthdate = user.DateOfBirth;
            EditInputModel.Email = user.Email;
            EditInputModel.ConfirmEmail = user.Email;
            EditInputModel.Gender = user.Gender;
            EditInputModel.Nationality = user.Nationality.Country;
        }
        protected async Task<int> EditCV()
        {
            user = _dbContext.User.Where(u => u.UserId == id).Include(u => u.Skills).Include(u => u.Nationality).FirstOrDefault();
            user.Skills.Clear();
            foreach (var i in Skills)
            {
                user.Skills.Add(await _dbContext.Skill.Where(s => s.SkillId == Convert.ToInt32(i)).FirstOrDefaultAsync());
            }
            Nationality n = await _dbContext.Nationality.Where(i => i.NationalityId == Convert.ToInt32(EditInputModel.Nationality)).FirstOrDefaultAsync();
            user.Nationality = n;
            user.FirstName = EditInputModel.FirstName;
            user.LastName = EditInputModel.LastName;
            user.DateOfBirth = EditInputModel.Birthdate;
            user.Gender = EditInputModel.Gender;
            user.Email = EditInputModel.Email;
            user.Grade = gradeService.CaluclateGrade(user);
            if (EditInputModel.Image != null)
            {
                string filePath = Path.Combine(webHostEnvironment.WebRootPath,
                    "images", user.ImagePath);
                System.IO.File.Delete(filePath);
                user.ImagePath = ProcessUploadedFile();
            }
            // Save the new photo in wwwroot/images folder and update
            // PhotoPath property of the employee object

            _dbContext.Update(user);
            await _dbContext.SaveChangesAsync();
            return user.UserId;

        }
    }
    public class EditInputModel
    {
        [DisplayName("First Name")]
        [Required]
        [BindProperty]
        [MinLength(2, ErrorMessage = "Please Enter a longer name")]
        [MaxLength(20, ErrorMessage = "Name is too long,Please enter a shorter name")]
        public string FirstName { get; set; }

        [MinLength(2, ErrorMessage = "Please Enter a longer name")]
        [MaxLength(20, ErrorMessage = "Name is too long,Please enter a shorter name")]
        [DisplayName("Last Name")]
        [Required]
        [BindProperty]
        public string LastName { get; set; }

        [DisplayName("Nationality")]
        [Required]
        [BindProperty]
        public string Nationality { get; set; }

        [Required]
        [BindProperty]
        [DisplayName("Birth Date")]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Email Address")]
        [Required]
        public string Email { get; set; }
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Confirm Email Address")]
        [Required]
        public string ConfirmEmail { get; set; }
        [DisplayName("Gender")]
        [Required]
        [BindProperty]
        public string Gender { get; set; }

        public IFormFile? Image { get; set; }
        [BindProperty]
        [Required]
        public string Sum { get; set; }
    }
}







