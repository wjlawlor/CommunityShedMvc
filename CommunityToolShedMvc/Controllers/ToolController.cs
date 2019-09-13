using CommunityToolShedMvc.Data;
using CommunityToolShedMvc.Models;
using CommunityToolShedMvc.Security;
using CommunityToolShedMvc.ViewModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace CommunityToolShedMvc.Controllers
{
    [Authorize]
    public class ToolController : Controller
    {
        public ActionResult Index()
        {
            List<Tool> tools = DatabaseHelper.Retrieve<Tool>(@"
                    SELECT Tool.ID, Tool.[Name], OwnerID, CONCAT(Person.FirstName, ' ', Person.LastName) AS OwnerName, 
                        ConditionID, Condition.[Name] AS ConditionName, Warnings
                    FROM Tool
                    JOIN Shed ON Shed.ToolID = Tool.ID
                    JOIN Person ON Person.ID = Tool.OwnerID
                    JOIN Condition ON Condition.ID = Tool.ConditionID
                ");

            return View(tools);
        }

        public ActionResult Add(int communityid)
        {
            List<ConditionType> conditionTypes = DatabaseHelper.Retrieve<ConditionType>(@"
                    SELECT ID, Name
                    FROM Condition
                    ORDER BY ID
                ");

            Person person = DatabaseHelper.RetrieveSingle<Person>(@"
                    SELECT CONCAT(FirstName,' ',LastName) AS FullName
                    FROM Person
                    WHERE ID = @ID
                ",
                    new SqlParameter("@ID", ((CustomPrincipal)User).Person.Id)
                );

            Community community = DatabaseHelper.RetrieveSingle<Community>(@"
                    SELECT Name, ID
                    FROM Community
                    WHERE ID = @ID
                ",
                    new SqlParameter("@ID", communityid)
                );

            var viewModel = new ToolPersonCommunityConditions(conditionTypes, person, community);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Add(int communityid, ToolPersonCommunityConditions viewModel)
        {
            if (ModelState.IsValid)
            {
                var id = DatabaseHelper.ExecuteScalar<int>(@"
                        BEGIN TRAN;

                        INSERT INTO Tool (OwnerID, Name, ConditionID, Warnings)
                        VALUES (@OwnerID, @Name, @ConditionID, @Warnings);

                        DECLARE @ToolID int;
                        SET @ToolId = cast(scope_identity() as int);

                        INSERT INTO Shed (ToolID, CommunityID)
                        VALUES (@ToolID, @CommunityID);

                        SELECT @CommunityID;

                        COMMIT TRAN;
                    ",
                        new SqlParameter("@OwnerID", ((CustomPrincipal)User).Person.Id),
                        new SqlParameter("@Name", viewModel.Tool.Name),
                        new SqlParameter("@ConditionID", viewModel.Tool.ConditionID),
                        new SqlParameter("@Warnings", viewModel.Tool.Warnings),
                        new SqlParameter("@CommunityID", communityid)
                    );

                return RedirectToRoute("Default", new { controller = "Community", action = "Overview", id = communityid });
            }

            List<ConditionType> conditionTypes = DatabaseHelper.Retrieve<ConditionType>(@"
                    SELECT ID, Name
                    FROM Condition
                    ORDER BY ID
                ");

            viewModel.SetConditionTypes(conditionTypes);
            return View(viewModel);
        }


        public ActionResult Edit(int communityid, int id)
        {
            List<ConditionType> conditionTypes = DatabaseHelper.Retrieve<ConditionType>(@"
                    SELECT ID, Name
                    FROM Condition
                    ORDER BY ID
                ");

            Person person = DatabaseHelper.RetrieveSingle<Person>(@"
                    SELECT CONCAT(FirstName,' ',LastName) AS FullName
                    FROM Person
                    WHERE ID = @ID
                ",
                    new SqlParameter("@ID", ((CustomPrincipal)User).Person.Id)
                );

            Community community = DatabaseHelper.RetrieveSingle<Community>(@"
                    SELECT Name, ID
                    FROM Community
                    WHERE ID = @ID
                ",
                    new SqlParameter("@ID", communityid)
                );

            var viewModel = new ToolPersonCommunityConditions(conditionTypes, person, community);

            viewModel.Tool = DatabaseHelper.RetrieveSingle<Tool>(@"
                    SELECT Name, ConditionID, Warnings
                    FROM Tool
                    WHERE ID = @ID
                ",
                    new SqlParameter("@ID", id)
                );

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(int communityid, int id, ToolPersonCommunityConditions viewModel)
        {
            if (ModelState.IsValid)
            {
                DatabaseHelper.Update(@"
                        UPDATE Tool SET
                            Name = @Name,
                            ConditionID = @ConditionID,
                            Warnings = @Warnings
                        WHERE ID = @ID
                    ",

                        new SqlParameter("@Name", viewModel.Tool.Name),
                        new SqlParameter("@ConditionID", viewModel.Tool.ConditionID),
                        new SqlParameter("@Warnings", viewModel.Tool.Warnings),
                        new SqlParameter("@ID", id)
                    );

                return RedirectToRoute("Default", new { controller = "Community", action = "Overview", id = communityid });
            }

            List<ConditionType> conditionTypes = DatabaseHelper.Retrieve<ConditionType>(@"
                    SELECT ID, Name
                    FROM Condition
                    ORDER BY ID
                ");

            Person person = DatabaseHelper.RetrieveSingle<Person>(@"
                    SELECT CONCAT(FirstName,' ',LastName) AS FullName
                    FROM Person
                    WHERE ID = @ID
                ",
                    new SqlParameter("@ID", ((CustomPrincipal)User).Person.Id)
                );

            Community community = DatabaseHelper.RetrieveSingle<Community>(@"
                    SELECT Name
                    FROM Community
                    WHERE ID = @ID
                ",
                    new SqlParameter("@ID", communityid)
                );

            viewModel = new ToolPersonCommunityConditions(conditionTypes, person, community);

            viewModel.Tool = DatabaseHelper.RetrieveSingle<Tool>(@"
                    SELECT Name, ConditionID, Warnings
                    FROM Tool
                    WHERE ID = @ID
                ",
                    new SqlParameter("@ID", id)
                );

            return View(viewModel);
        }
    }
}