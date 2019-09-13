using CommunityToolShedMvc.Security;
using System.Web.Mvc;

namespace CommunityToolShedMvc.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            CustomPrincipal user = (CustomPrincipal)User;

            bool isApprover = user.IsInRole(1, "Approver");
            //bool isApprover = user.IsInRole(2, "Approver");

            return View();
        }
    }
}