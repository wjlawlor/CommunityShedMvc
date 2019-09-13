﻿using System.Collections.Generic;

namespace CommunityToolShedMvc.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public List<CommunityRole> Roles { get; set; }
    }
}