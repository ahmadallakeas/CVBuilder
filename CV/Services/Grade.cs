using CV.Models;

namespace CV.Services
{
    public class Grade
    {
        public int CaluclateGrade(User user)
        {
            int grade = 0;
            grade=user.Skills.Count*10;
            if(user.Gender=="Male")
            {
                grade += 5;
            }
            else
            {
                grade += 10;
            }
            return grade;
        }
    }
}
