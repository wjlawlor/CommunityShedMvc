namespace CommunityToolShedMvc.Models
{
    public class CommunityRole
    {
        public int CommunityID { get; set; }
        public bool isApprover { get; set; }
        public bool isReviewer { get; set; }
        public bool isEnforcer { get; set; }
    }
}