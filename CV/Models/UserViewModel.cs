namespace CV.Models
{
    public class UserViewModel
    {

        public string FullName { get; set; }
        public string Birthdate { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string Image { get; set; }
        public int Grade { get; set; }
        public List<string> Skills { get; set; } = new List<string>();

    }
}
