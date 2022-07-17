namespace CV.Models
{
    public class Skill
    {
        public int SkillId { get; set; }
        public string SkillName { get; set; } 
        public IEnumerable<User> Users { get; set; } 
    }
}
