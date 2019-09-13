using System.ComponentModel.DataAnnotations;

namespace CommunityToolShedMvc.Models
{
    public class Tool
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int OwnerID { get; set; }

        [Display(Name = "Owner's Name")]
        public string OwnerName { get; set; }

        [Required]
        public int CommunityID { get; set; }

        [Display(Name = "Community Name")]
        public string CommunityName { get; set; }

        [Required]
        [Display(Name = "Condition")]
        public int ConditionID { get; set; }
        public string ConditionName { get; set; }
        public string Warnings { get; set; }
    }
}