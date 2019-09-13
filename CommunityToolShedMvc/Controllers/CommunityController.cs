using CommunityToolShedMvc.Data;
using CommunityToolShedMvc.Models;
using CommunityToolShedMvc.Security;
using CommunityToolShedMvc.ViewModels;
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

        public ActionResult Overview(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

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

            List<Person> members = DatabaseHelper.Retrieve<Person>(@"
                    SELECT Person.ID, FirstName, LastName, isApprover, isReviewer, isEnforcer
                    FROM CommunityMembers
                    JOIN Person ON Person.ID = CommunityMembers.PersonID
                    WHERE CommunityMembers.CommunityID = @ID
					ORDER BY LastName
                ",
                    new SqlParameter("@ID", id)
                );

            var viewModel = new CommunityMembersTools();
            viewModel.Community = community;
            viewModel.Members = members;

            return View(viewModel);
        }

        public ActionResult Create()
        {
            List<CommunityType> communityTypes = DatabaseHelper.Retrieve<CommunityType>(@"
                    SELECT ID, Type
                    FROM CommunityType
                    ORDER BY ID
                ");


            var viewModel = new CommunityWithTypes(communityTypes);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(CommunityWithTypes viewModel)
        {
            if (ModelState.IsValid)
            {
                //var id = DatabaseHelper.Insert(@"
                //    INSERT INTO Community (Name, OwnerID, TypeID)
                //    VALUES (@Name, @OwnerID, @TypeID)
                //",
                //    new SqlParameter("@Name", viewModel.Community.Name),
                //    new SqlParameter("@OwnerID", ((CustomPrincipal)User).Person.Id),
                //    new SqlParameter("@TypeID",viewModel.Community.TypeID)
                //);

                var id = DatabaseHelper.ExecuteScalar<int>(@"
                BEGIN TRAN;

                INSERT INTO Community (Name, OwnerID, TypeID)
                VALUES (@CommunityName, @OwnerID, @TypeID);

                DECLARE @CommunityID int;
                SET @CommunityId = cast(scope_identity() as int);

                INSERT INTO CommunityMembers (CommunityID, PersonID, isApprover, isReviewer, isEnforcer)
                VALUES (@CommunityID, @PersonID, @isApprover, @isReviewer, @isEnforcer);

                SELECT @CommunityID;

                COMMIT TRAN;
            ",
                new SqlParameter("@CommunityName", viewModel.Community.Name),
                new SqlParameter("@OwnerID", ((CustomPrincipal)User).Person.Id),
                new SqlParameter("@TypeID", viewModel.Community.TypeID),
                new SqlParameter("@PersonID", ((CustomPrincipal)User).Person.Id),
                new SqlParameter("@isApprover", true),
                new SqlParameter("@isReviewer", true),
                new SqlParameter("@isEnforcer", true)
            );

                return RedirectToAction("Overview", new { id = id });
            }

            List<CommunityType> communityTypes = DatabaseHelper.Retrieve<CommunityType>(@"
                    SELECT ID, Type
                    FROM CommunityType
                    ORDER BY ID
                ");

            viewModel.SetCommunityTypes(communityTypes);
            return View(viewModel);
        }
    }
}