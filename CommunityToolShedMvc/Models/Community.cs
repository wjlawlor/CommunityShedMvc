using System.ComponentModel.DataAnnotations;

namespace CommunityToolShedMvc.Models
{
    public class Community
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
        public int OwnerID { get; set; }
        public string OwnerName { get; set; }
        public int TypeID { get; set; }
        public string TypeName { get; set; }

        // Maybe
        // public List<int> MemberList { get; set; }
    }
}