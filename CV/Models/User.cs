
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CV.Models
{
    public class User
    {

        public int UserId { get; set; }
 
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Gender { get; set; }

        public string Email{ get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        
        public Nationality Nationality { get; set; }
        
        public List<Skill> Skills { get; set; }=new List<Skill>();
        public int Grade { get; set; }
        public string ImagePath { get; set; }


    }
}
