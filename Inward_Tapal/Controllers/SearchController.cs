using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Inward_Tapal.Models;

namespace Inward_Tapal.Controllers
{
    public class SearchController : Controller
    {
        private TapalsEntities db = new TapalsEntities();

        // GET: Search
        public ActionResult Index(string searchBy, string search,string searching, string searching1)
        {
            if (searchBy == "T_Dept")
            {
                return View(db.Inwards.Where(x => x.T_Dept.StartsWith(search) || search == null).ToList());
            }
            else if (searchBy == "T_Section")
            {
                return View(db.Inwards.Where(x => x.T_Section.StartsWith(search) || search == null).ToList());
            }
           else if(searchBy== "TPLNO")
          // else 
          {

                return View(db.Inwards.Where(x => x.TPLNO.StartsWith(search) || search == null).ToList());
            }
            else
            {
                DateTime searchfrom = Convert.ToDateTime(searching);
                DateTime searchto = Convert.ToDateTime(searching1);
                return View(db.Inwards.Where(c => c.Date_in>= searchfrom && c.Date_in < searchto));
            }
        }

        // GET: Search/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inward inward = db.Inwards.Find(id);
            if (inward == null)
            {
                return HttpNotFound();
            }
            return View(inward);
        }

    }
}
