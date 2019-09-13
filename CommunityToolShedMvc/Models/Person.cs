using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CommunityToolShedMvc.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Display(Name = "User's Name")]
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public List<CommunityRole> Roles { get; set; }
    }
}