using CommunityToolShedMvc.Models;
using System.Collections.Generic;

namespace CommunityToolShedMvc.ViewModels
{
    public class CommunityMembersTools
    {
        public Community Community { get; set; }
        public List<Person> Members { get; set; }
        public List<Tool> Tools { get; set; }
    }
}