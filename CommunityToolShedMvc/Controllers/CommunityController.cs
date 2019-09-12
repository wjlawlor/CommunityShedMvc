using CommunityToolShedMvc.Data;
using CommunityToolShedMvc.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CommunityToolShedMvc.Controllers
{
    public class CommunityController : Controller
    {
        public ActionResult Index()
        {
            List<Community> communities = DatabaseHelper.Retrieve<Community>(@"
                SELECT Community.ID, Community.Name, Community.OwnerID, CONCAT(Person.FirstName,' ',Person.LastName) AS OwnerName, 
                    Community.TypeID, CommunityType.Type AS TypeName
                FROM Community
                JOIN CommunityType ON Community.TypeID = CommunityType.ID
                JOIN Person ON Community.OwnerID = Person.ID");

            return View(communities);
        }

        public ActionResult Overview(int id)
        {
            Community community = DatabaseHelper.RetrieveSingle<Community>(@"
                    SELECT Community.ID, Community.Name, Community.OwnerID, CONCAT(Person.FirstName,' ',Person.LastName) AS OwnerName, 
                        Community.TypeID, CommunityType.Type AS TypeName
                    FROM Community
                    JOIN CommunityType ON Community.TypeID = CommunityType.ID
                    JOIN Person ON Community.OwnerID = Person.ID
                    WHERE Community.ID = @ID
                ",
                    new SqlParameter("@ID", id)
                );

            return View(community);
        }
    }
}