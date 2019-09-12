using CommunityToolShedMvc.Data;
using CommunityToolShedMvc.Models;
using CommunityToolShedMvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CommunityToolShedMvc.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Register()
        {
            var viewModel = new RegisterViewModel();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string password = viewModel.Password;

                DatabaseHelper.Insert(@"
                    INSERT INTO Person (FirstName, LastName, Email, Password, Address1, Address2, City, State, Zip)
                    VALUES (@FirstName, '', @Email, @Password, '', '', '', '', '' )
                ",
                    new SqlParameter("@FirstName", viewModel.FirstName),
                    new SqlParameter("@Email", viewModel.Email),
                    new SqlParameter("@Password", viewModel.Password));

                FormsAuthentication.SetAuthCookie(viewModel.Email, false);
                return RedirectToAction("Index", "Home");
            }

            return View(viewModel);
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            var viewModel = new LoginViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValidField("Email") && ModelState.IsValidField("Password"))
            {
                Person person = DatabaseHelper.RetrieveSingle<Person>(@"
                    SELECT Password
                    FROM Person
                    WHERE Email = @Email
                ",
                    new SqlParameter("@Email", viewModel.Email));

                if (person == null || viewModel.Password != person.Password)
                {
                    ModelState.AddModelError("", "Login failed.");
                }
            }


            if (ModelState.IsValid)
            {
                FormsAuthentication.SetAuthCookie(viewModel.Email, false);
                return RedirectToAction("Index", "Home");
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}