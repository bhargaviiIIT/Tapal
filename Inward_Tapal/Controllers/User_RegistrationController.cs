using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inward_Tapal.Models;
namespace Inward_Tapal.Controllers
{
    public class User_RegistrationController : Controller
    {
        // GET: User_Registration
        public ActionResult Index(User_Registration reg)
        {
            using (var db = new TapalsEntities())
            {
                db.Registrations.Add(new Registration
                {
                  // Emp_ID=reg.EmpId,
                    Username = reg.Username,
                    Password=reg.Password,
                    Section=reg.Section,
                    Designation=reg.Designation
                });
                db.SaveChanges();
            }

            return View();
        }
    }
}