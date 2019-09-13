using CommunityToolShedMvc.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CommunityToolShedMvc.ViewModels
{
    public class CommunityOwnerTypes
    {
        public CommunityOwnerTypes() { }

        public CommunityOwnerTypes(List<CommunityType> communityTypes, Person person)
        {
            SetCommunityTypes(communityTypes);
            Person = person;

            //var selectListItems = new SelectListItem[]
            //{
            //    new SelectListItem() { Value = "1", Text = "Item 1" },
            //    new SelectListItem() { Value = "2", Text = "Item 2" },
            //    new SelectListItem() { Value = "3", Text = "Item 3" },
            //};
        }

        public Community Community { get; set; }

        public Person Person { get; set; }

        public SelectList TypeSelectList { get; set; }

        public void SetCommunityTypes(List<CommunityType> communityTypes)
        {
            TypeSelectList = new SelectList(communityTypes, "ID", "Type");
        }
    }
}