using CommunityToolShedMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityToolShedMvc.ViewModels
{
    public class CommunityMembersTools
    {
        public Community Community { get; set; }
        public List<Person> Members { get; set; }
    }
}