using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CV.Pages
{
    public class SuccessModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        [FromQuery]
        public string Operation { get; set; }
        public void OnGet()
        {

        }
    }
}
