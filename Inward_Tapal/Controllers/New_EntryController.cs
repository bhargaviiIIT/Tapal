using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Inward_Tapal.Models;
using System.Data.SqlClient;

namespace Inward_Tapal.Controllers
{
    public class New_EntryController : Controller
    {
        TapalsEntities user = new TapalsEntities();


        [HttpGet]
        public ActionResult Index(int? id)
        {
            //  string I;
            ViewBag.department = new SelectList(user.Departments, "Dept", "Dept");

            using (var database = new TapalsEntities())
            {
                //var count = database.Inwards.Select(hc => hc.TPLNO).Count() + 1;
                // ViewBag.count1 ="I"+ count;
                // ViewBag.tplno = database.Inwards.Max(hc => hc.TPLNO) + 1;
                //DateTime tpl = DateTime.Parse(database.Inwards
                //              .OrderByDescending(p => p.TPLNO)
                //              .Select(r => r.Date_in)
                //              .First().ToString());
                //if (tpl != DateTime.Now.Date)
                //{
                //    ViewBag.count1 = "I" + 01;
                //}
                //else
                //{
                //    var tpl2 = DateTime.Parse(database.Inwards
                //             .OrderByDescending(p => p.TPLNO)
                //             .Select(r => r.Date_in)
                //             .First().ToString());
                //    ViewBag.count1 = "I" + tpl2;
                //}
                //int TPLNO = id ?? 0;
                if (id > 0)
                {

                    var tapalDetail = (from a in database.Inwards
                                       select new sample_User
                                       {
                                       }).SingleOrDefault();
                    return View(tapalDetail);
                }
            }
            return View(new sample_User());
        }


        public ActionResult edit()
        {
            using (var db = new TapalsEntities())
            {
                List<TapalsEntities> str = new List<TapalsEntities>();
                var query = (from pro in db.Inwards
                             select new sample_User()
                             {
                                 Date_in = pro.Date_in,
                                 T_Sno = pro.T_Sno,
                                 T_From = pro.T_From,
                                 T_Dept = pro.T_Dept,
                                 T_Subject = pro.T_Subject,
                                 T_Section = pro.T_Section,
                                 T_Passout = pro.T_Passout,
                                 TPLNO = pro.TPLNO,
                                 T_Cheque = pro.T_Cheque,
                                 T_User = pro.T_User,
                                 wfcpno = pro.wfcpno
                             }).ToList();
                return View(query);
            }

        }
        [HttpPost]
        public ActionResult updatedetails(sample_User samp, FormCollection frm)
        {

            using (var database = new TapalsEntities())
            {
                // var tplno = database.Inwards.Max(hc => hc.TPLNO) + 1;
                var chequeList = new List<in_Cheque>();

                foreach (var key in frm.AllKeys.Where(hc => hc.Contains("MyKey")).ToList())
                {
                    var myKey = key.Replace("MyKey", "");
                    in_Cheque check = new in_Cheque()
                    {
                        AMOUNT = frm["AMOUNT" + myKey],
                        BANK = frm["BANK" + myKey],
                        CQDATE = (Convert.ToDateTime(frm["CQDATE" + myKey])),
                        CQNO = Convert.ToInt64(frm["CQNO" + myKey]),
                        //TPLNO = Convert.ToInt32(tplno + myKey)
                        // TPLNO = tplno + myKey
                    };
                    chequeList.Add(check);
                }

                if (chequeList != null && chequeList.Any())
                {
                    using (var dbobj = new TapalsEntities())
                    {
                        foreach (var cheque in chequeList)
                        {
                            bool isrecordexist = false;
                            var dbtable = dbobj.Cheques.SingleOrDefault(hc => hc.CQNO == cheque.CQNO);
                            if (dbtable != null)
                            {
                                isrecordexist = true;
                            }
                            else
                            {
                                dbtable = new Cheque();

                            }

                            dbtable.AMOUNT = cheque.AMOUNT;
                            dbtable.BANK = cheque.BANK;
                            dbtable.CQDATE = cheque.CQDATE;
                            dbtable.CQNO = Convert.ToInt32(cheque.CQNO);
                            //dbtable.TPLNO = tplno1;
                            if (!isrecordexist)
                            {
                                dbobj.Cheques.Add(dbtable);
                            }
                            dbobj.SaveChanges();
                        }
                    }
                }

                using (var dbobj = new TapalsEntities())
                {
                    bool isrecordexist = false;
                    var dbtable = dbobj.Inwards.SingleOrDefault(hc => hc.TPLNO == samp.TPLNO);
                    if (dbtable != null)
                    {


                    }
                    else
                    {
                        string dep = frm["Department"];
                        dbtable = new Inward();
                        dbtable.Date_in = DateTime.Now;
                        dbtable.T_Sno = samp.T_Sno;
                        dbtable.T_From = samp.T_From;
                        dbtable.T_Dept = frm["Department"];
                        dbtable.T_Subject = samp.T_Subject;
                        dbtable.T_Section = samp.T_Section;
                        dbtable.T_Passout = samp.T_Passout;
                        dbtable.TPLNO = "I1526";
                        dbtable.T_Cheque = "Y";
                        //  dbtable.T_User = Session["UserName"].ToString();
                        dbtable.T_User = "Bharkavi";
                          dbtable.wfcpno = samp.wfcpno;



                    }
                    if (!isrecordexist)
                    {
                        dbobj.Inwards.Add(dbtable);
                    }

                    dbobj.SaveChanges();

                }
                return RedirectToAction("Index", "New_Entry");
            }
        }

        public ActionResult delete(int? id)
        {
            using (var dbobj = new TapalsEntities())
            {
                //from a in database.Inwards
                //where a.Id == id
                // var dbtab = dbobj.Inwards.SingleOrDefault(hc => (hc.Id = id));
                var dbtab = dbobj.Inwards.SingleOrDefault(hc => (hc.Id == id));

                if (dbtab != null)
                {

                    dbobj.Inwards.Remove(dbtab);
                    dbobj.SaveChanges();
                }
            }
            TempData["student"] = "ÿgffy";

            return RedirectToAction("edit", "New_Entry");
        }

        //public ActionResult delete(int? id)
        //{
        //    using (var database = new TapalsEntities())
        //    {
        //        if (id > 0)
        //        {

        //            var tapalDetail = (from a in database.Inwards
        //                               where a.Id == id
        //                               select new sample_User
        //                               {
        //                                   Id = a.Id,
        //                                   Date_in = a.Date_in,
        //                                   T_Sno = a.T_Sno,
        //                                   T_From = a.T_From,
        //                                   T_Dept = a.T_Dept,
        //                                   T_Subject = a.T_Subject,
        //                                   T_Section = a.T_Section,
        //                                   T_Passout = a.T_Passout,
        //                                   TPLNO = a.TPLNO,
        //                                   T_Cheque = a.T_Cheque,
        //                                   T_User = a.T_User,
        //                                   wfcpno = a.wfcpno
        //                               }).SingleOrDefault();
        //            ViewBag.department = new SelectList(user.Departments, "Dept", "Dept", tapalDetail.T_Dept);
        //            return View(tapalDetail);
        //        }
        //    }
        //    return View(new sample_User());
        //}
        public PartialViewResult _partial(int id)
        {
            ViewData["id"] = id;
            return PartialView();
        }
        public JsonResult GetDropDownValue(string department)
        {
            using (var database = new TapalsEntities())
            {
                var dbList = (from hc in database.Departments
                              where hc.DeptOrCentre == department
                              select new
                              {
                                  Text = hc.Dept,
                                  value = hc.Id
                              }).ToList();
                return Json(dbList, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult _ShowDetails()
        {
            using (var database = new TapalsEntities())
            {
                var dbSameUserList = (from hc in database.Inwards
                            select new sample_User
                            {
                                Date_in = hc.Date_in,
                                T_Sno = hc.T_Sno,
                                T_From = hc.T_From,
                                T_Dept = hc.T_Dept,
                                T_Subject = hc.T_Subject,
                                T_Section = hc.T_Section,
                                T_Passout = hc.T_Passout,
                                TPLNO = hc.TPLNO,
                                T_Cheque = hc.T_Cheque,
                                T_User = hc.T_User,
                                wfcpno = hc.wfcpno
                            }).ToList();
                return PartialView(dbSameUserList !=null ? dbSameUserList : new List<sample_User>());
            }
        }
    }
}

