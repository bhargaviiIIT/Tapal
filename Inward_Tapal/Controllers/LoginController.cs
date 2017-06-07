using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inward_Tapal.Models;

namespace Inward_Tapal.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
                public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login login)
        {
            if (ModelState.IsValid)
            {
                TapalsEntities db = new TapalsEntities();
                var user = (from Inward_Tapal in db.Registrations
                            where Inward_Tapal.Username == login.UserName && Inward_Tapal.Password == login.Password
                            select new
                            {
                                // Inward_Tapal.UserID,
                                Inward_Tapal.Username
                            }).ToList();
                if (user.FirstOrDefault() != null)
                {
                    //   Session["UserName"] = user.FirstOrDefault().Username;
                    Session["UserName"] = user.FirstOrDefault().Username.ToString();
                    //Session["UserID"]=user.FirstOrDefault().UserID;  
                    string test = Session["UserName"].ToString();
                    return Redirect("/Inward/Create");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login credentials.");
                }
            }
            return View(login);
        }

        public ActionResult WelcomePage()
        {
            return View();
        }
    }
}

   