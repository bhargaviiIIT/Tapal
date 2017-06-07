using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inward_Tapal.Models;

namespace Inward_Tapal.Controllers
{
    public class IssuingController : Controller
    {
        // GET: Issuing
        public ActionResult Issue()
        {
            return View();
        }
        //public ActionResult Issue()
        //{
        //    TapalsEntities user = new TapalsEntities();
        //    DateTime startDateTime = DateTime.Today; //Today at 00:00:00
        //    DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
        //    int count = user.Inwards.Where(c => c.Date_in >= startDateTime && c.Date_in < endDateTime && c.T_Dept == "Accounts").Count();
        //    using (var db = new TapalsEntities())
        //    {
        //        List<TapalsEntities> str = new List<TapalsEntities>();
        //        var query = (from pro in db.Inwards
        //                     where pro.T_Section == "Accounts" && pro.Date_in >= startDateTime && pro.Date_in < endDateTime
        //                     select new sample_User()
        //                     {
        //                         Id = pro.Id,
        //                         Date_in = pro.Date_in,
        //                         T_Sno = pro.T_Sno,
        //                         T_From = pro.T_From,
        //                         T_Dept = pro.T_Dept,
        //                         T_Subject = pro.T_Subject,
        //                         T_Section = pro.T_Section,
        //                         T_Passout = pro.T_Passout,
        //                         TPLNO = pro.TPLNO,
        //                         T_Cheque = pro.T_Cheque,
        //                         T_User = pro.T_User,
        //                         wfcpno = pro.wfcpno
        //                     }).ToList();
        //        return View(query);
        //    }

        //}

        }

    }
