using CommunityToolShedMvc.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CommunityToolShedMvc.ViewModels
{
    public class ToolPersonCommunityConditions
    {
        public ToolPersonCommunityConditions() { }

        public ToolPersonCommunityConditions(List<ConditionType> conditionTypes, Person person, Community community)
        {
            SetConditionTypes(conditionTypes);
            Person = person;
            Community = community;
        }

        public Tool Tool { get; set; }

        public Person Person { get; set; }

        public Community Community { get; set; }

        public SelectList TypeSelectList { get; private set; }

        public void SetConditionTypes(List<ConditionType> conditionTypes)
        {
            TypeSelectList = new SelectList(conditionTypes, "ID", "Name");
        }
    }
}