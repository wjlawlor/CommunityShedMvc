namespace CommunityToolShedMvc.Models
{
    public class Tool
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int OwnerID { get; set; }
        public string OwnerName { get; set; }
        public int ConditionID { get; set; }
        public string ConditionName { get; set; }
        public string Warnings { get; set; }
    }
}