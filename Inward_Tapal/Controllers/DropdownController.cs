using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inward_Tapal.Models;

namespace Inward_Tapal.Controllers
{
    public class DropdownController : Controller
    {
        // GET: Dropdown
        public ActionResult Index()
        {
            TapalsEntities te = new TapalsEntities();
            ViewBag.department = new SelectList(te.Departments, "Id", "Dept");
            return View();
        }
    }
}