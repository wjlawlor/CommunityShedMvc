using CommunityToolShedMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CommunityToolShedMvc.ViewModels
{
    public class CommunityWithTypes
    {
        public CommunityWithTypes() { }

        public CommunityWithTypes(List<CommunityType> communityTypes)
        {
            SetCommunityTypes(communityTypes);

            //var selectListItems = new SelectListItem[]
            //{
            //    new SelectListItem() { Value = "1", Text = "Item 1" },
            //    new SelectListItem() { Value = "2", Text = "Item 2" },
            //    new SelectListItem() { Value = "3", Text = "Item 3" },
            //};
        }

        public Community Community { get; set; }

        public SelectList TypeSelectList { get; set; }

        public void SetCommunityTypes(List<CommunityType> communityTypes)
        {
            TypeSelectList = new SelectList(communityTypes, "ID", "Type");
        }
    }
}