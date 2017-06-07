using Inward_Tapal.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Inward_Tapal.Controllers
{
    public class InwardController : Controller
    {
        TapalsEntities user = new TapalsEntities();

        #region Create
        /// <summary>
        /// Allows user to create a new inward(Tapal entry)
        /// </summary>
        /// <returns></returns>
        public ActionResult Create(int? id)
        {
           // string aaa = "000";
            string slNo = "000";
            int TPLNO = id ?? 0;
            DateTime startDateTime = DateTime.Today; //Today at 00:00:00
            DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
            int count = user.Inwards.Where(c => c.Date_in >= startDateTime && c.Date_in < endDateTime).Count();
            if (count > 0)
            {
                slNo +=  (count + 1);
            }
            else
            {
                slNo +=  1;
            }
            slNo=slNo.Substring(slNo.Length - 3, 3);
            slNo = "I" + slNo;
            sample_User sampleUser = new sample_User();
           var subjectlist= user.Subjects.ToList();
            sampleUser.subList = subjectlist.Select(m => new SelectListItem()
            {
                Text = m.Subject_Name,
                Value = m.Subject_Name
            }).ToList();
            sampleUser.T_Sno = slNo;
            ViewBag.count1 = user.Inwards.Where(c => c.Date_in >= startDateTime && c.Date_in < endDateTime && c.T_Section == "Accounts").Count();
            ViewBag.count2 = user.Inwards.Where(c => c.Date_in >= startDateTime && c.Date_in < endDateTime && c.T_Section == "Purchase").Count();
            ViewBag.count3 = user.Inwards.Where(c => c.Date_in >= startDateTime && c.Date_in < endDateTime && c.T_Section == "Recruitment").Count();
            ViewBag.count4 = user.Inwards.Where(c => c.Date_in >= startDateTime && c.Date_in < endDateTime && c.T_Section == "Dean's Office" && !c.Issue).Count();
            ViewBag.count5 = user.Inwards.Where(c => c.Date_in >= startDateTime && c.Date_in < endDateTime && c.T_Section == "IPR Cell").Count();
            ViewBag.count6 = user.Inwards.Where(c => c.Date_in >= startDateTime && c.Date_in < endDateTime && c.T_Section == "ICSR Facilities").Count();
            ViewBag.count7 = user.Inwards.Where(c => c.Date_in >= startDateTime && c.Date_in < endDateTime && c.T_Section == "Office").Count();
            ViewBag.subject = new SelectList(user.Subjects, "Subject_Name", "Subject_Name");
            return View(sampleUser);
           
          // ViewBag.subject= new SelectList(user.Subjects,"")
        }
        #endregion


        #region Create
        /// <summary>
        /// Allows user to create a new inward(Tapal entry)
        /// </summary>
        /// <param name="samp"></param>
        /// <param name="frm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(sample_User samp, FormCollection frm)
        {

            using (var database = new TapalsEntities())
            {
                var chequeList = new List<in_Cheque>();
                if (frm.AllKeys.Any() && frm.AllKeys.Count() > 0)
                {
                    foreach (var key in frm.AllKeys.Where(hc => hc.Contains("MyKey")).ToList())
                    {
                        var myKey = key.Replace("MyKey", "");
                        in_Cheque check = new in_Cheque()
                        {
                            AMOUNT = frm["AMOUNT" + myKey],
                            BANK = frm["BANK" + myKey],
                            CQDATE = Convert.ToDateTime(frm["CQDATE" + myKey]),
                            CQNO = Convert.ToInt64(frm["CQNO" + myKey]),
                        };
                        chequeList.Add(check);
                    }
                }

                if (chequeList != null && chequeList.Any())
                {

                    foreach (var cheque in chequeList)
                    {
                        bool isCheckexist = false;
                        var dbCheckDetails = database.Cheques.SingleOrDefault(hc => hc.CQNO == cheque.CQNO);
                        if (dbCheckDetails != null)
                        {
                            isCheckexist = true;
                        }
                        else
                        {
                            dbCheckDetails = new Cheque();

                        }
                        dbCheckDetails.CQNO = Convert.ToInt32(cheque.CQNO);
                        dbCheckDetails.TPLNO = (String.Format("{0:ddMMyyyy}", DateTime.Now.Date).Replace(" ", String.Empty)) + (samp.T_Sno);
                        dbCheckDetails.CQDATE = cheque.CQDATE;
                        dbCheckDetails.BANK = cheque.BANK;
                        dbCheckDetails.AMOUNT = cheque.AMOUNT;
                        if (!isCheckexist)
                        {
                            database.Cheques.Add(dbCheckDetails);
                        }
                        database.SaveChanges();
                    }
                }
                bool isrecordexist = false;
                string TapalNo = string.Empty;
                var dbInWardDetail = database.Inwards.SingleOrDefault(hc => hc.TPLNO == samp.TPLNO);
                if (dbInWardDetail != null)
                {
                    isrecordexist = true;
                }
                else
                {
                    dbInWardDetail = new Inward();
                    TapalNo = (String.Format("{0:ddMMyyyy}", DateTime.Now.Date).Replace(" ", String.Empty)) + (samp.T_Sno);
                    dbInWardDetail.TPLNO = TapalNo;
                    dbInWardDetail.T_Sno = samp.T_Sno;
                    dbInWardDetail.T_Cheque = frm["Check1"] != null ? "Y" : "N";
                    dbInWardDetail.Date_in = DateTime.Now.Date;
                }
                dbInWardDetail.T_From = samp.T_From;
                dbInWardDetail.T_Dept = frm["hdndepartmentDropDown"];
                dbInWardDetail.T_Subject = samp.T_Subject;
                dbInWardDetail.T_Subject_Value = samp.T_Subject_Value;
                dbInWardDetail.T_Section = frm["hdnsectionDropdownId"];
                dbInWardDetail.T_User = "Barkavi";
                dbInWardDetail.T_Passout = samp.T_Passout;
                dbInWardDetail.wfcpno = samp.wfcpno;
                dbInWardDetail.Issue = false;
                if (!isrecordexist)
                {
                    database.Inwards.Add(dbInWardDetail);
                }
                database.SaveChanges();
                if (!string.IsNullOrEmpty(TapalNo))
                {
                    TempData["TapalNo"] = TapalNo;
                }
                return RedirectToAction("Create", "Inward");
            }
        }
        #endregion
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
        public PartialViewResult _partial(int id)
        {
            ViewData["id"] = id;
            return PartialView();
        }
        public ActionResult _ShowDetails()
        {
            using (var database = new TapalsEntities())
            {
                DateTime startDateTime = DateTime.Today; //Today at 00:00:00
                DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
                var dbSameUserList = (from hc in database.Inwards
                                      where hc.Date_in >= startDateTime && hc.Date_in < endDateTime && hc.Issue==false
                                      select new sample_User
                                      {
                                          Id = hc.Id,
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
                                          wfcpno = hc.wfcpno,
                                      }).ToList();
                return PartialView(dbSameUserList != null ? dbSameUserList : new List<sample_User>());
            }
        }
        
        public ActionResult edit(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

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
                int pageSize = 3;
                int pageNumber = (page ?? 1);
                return View(query.ToPagedList(pageNumber, pageSize));
            }

            //  return View(students.ToPagedList(pageNumber, pageSize));
            // return View()
        }

        public ActionResult EditTapal()
        {
            using (var db = new TapalsEntities())
            {
                List<TapalsEntities> str = new List<TapalsEntities>();
                var query = (from pro in db.Inwards
                             select new sample_User()
                             {
                                 Id = pro.Id,
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

        [HttpGet]
        public ActionResult Index(int? id)
        {
            using (var database = new TapalsEntities())
            {
                if (id > 0)
                {

                    var tapalDetail = (from a in database.Inwards
                                       where a.Id == id
                                       select new sample_User
                                       {
                                           Id = a.Id,
                                           Date_in = a.Date_in,
                                           T_Sno = a.T_Sno,
                                           T_From = a.T_From,
                                           T_Dept = a.T_Dept,
                                           T_Subject = a.T_Subject,
                                           T_Section = a.T_Section,
                                           T_Passout = a.T_Passout,
                                           TPLNO = a.TPLNO,
                                           T_Cheque = a.T_Cheque,
                                           T_User = a.T_User,
                                           wfcpno = a.wfcpno
                                       }).SingleOrDefault();

                     ViewData["Department"] = database.Departments.FirstOrDefault(de => de.Dept == tapalDetail.T_Dept).DeptOrCentre;
                    ViewBag.department = new SelectList(user.Departments, "Dept", "Dept", tapalDetail.T_Dept);
                    //ViewBag.department = user.Departments.Select(m => new SelectListItem()
                    //{
                    //    Text = m.Dept,
                    //    Value = m.Id.ToString()
                    //}).ToList();
                    //ViewBag.subject = new SelectList(user.Subjects, "Subject_Name", "Subject_Name");
                    return View(tapalDetail);
                }
            }
            return View(new sample_User());
        }

        public ActionResult acc_count()
        {
            DateTime startDateTime = DateTime.Today; //Today at 00:00:00
            DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
            int count = user.Inwards.Where(c => c.Date_in >= startDateTime && c.Date_in < endDateTime && c.T_Dept == "Accounts").Count();
            return View();
        }


        //// GET: Search/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Inward inward = db.Inwards.Find(id);
        //    if (inward == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(inward);
        //}

        //// POST: Search/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Inward inward = db.Inwards.Find(id);
        //    db.Inwards.Remove(inward);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}


        public ActionResult accountgrid()
        {
            DateTime startDateTime = DateTime.Today; //Today at 00:00:00
            DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
            int count = user.Inwards.Where(c => c.Date_in >= startDateTime && c.Date_in < endDateTime && c.T_Dept == "Accounts").Count();
            using (var db = new TapalsEntities())
            {
                List<TapalsEntities> str = new List<TapalsEntities>();
                var query = (from pro in db.Inwards
                             where pro.T_Section == "Accounts" && pro.Date_in >= startDateTime && pro.Date_in < endDateTime
                             select new sample_User()
                             {
                                 Id = pro.Id,
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
        public ActionResult Purchasegrid()
        {
            DateTime startDateTime = DateTime.Today; //Today at 00:00:00
            DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
            int count = user.Inwards.Where(c => c.Date_in >= startDateTime && c.Date_in < endDateTime && c.T_Dept == "Purchase").Count();
            using (var db = new TapalsEntities())
            {
                List<TapalsEntities> str = new List<TapalsEntities>();
                var query = (from pro in db.Inwards
                             where pro.T_Section == "Purchase" && pro.Date_in >= startDateTime && pro.Date_in < endDateTime
                             select new sample_User()
                             {
                                 Id = pro.Id,
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

        public ActionResult Recruitmentgrid()
        {
            DateTime startDateTime = DateTime.Today; //Today at 00:00:00
            DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
            int count = user.Inwards.Where(c => c.Date_in >= startDateTime && c.Date_in < endDateTime && c.T_Dept == "Recruitment").Count();
            using (var db = new TapalsEntities())
            {
                List<TapalsEntities> str = new List<TapalsEntities>();
                var query = (from pro in db.Inwards
                             where pro.T_Section == "Recruitment" && pro.Date_in >= startDateTime && pro.Date_in < endDateTime
                             select new sample_User()
                             {
                                 Id = pro.Id,
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

        public ActionResult Deanofficegrid()
        {
            DateTime startDateTime = DateTime.Today; //Today at 00:00:00
            DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
            int count = user.Inwards.Where(c => c.Date_in >= startDateTime && c.Date_in < endDateTime && c.T_Dept == "Dean's office").Count();
            using (var db = new TapalsEntities())
            {
                List<TapalsEntities> str = new List<TapalsEntities>();
                var query = (from pro in db.Inwards
                             where pro.T_Section == "Dean's office" && pro.Date_in >= startDateTime && pro.Date_in < endDateTime && !pro.Issue
                             select new sample_User()
                             {
                                 Id = pro.Id,
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

        public ActionResult IPRCellgrid()
        {
            DateTime startDateTime = DateTime.Today; //Today at 00:00:00
            DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
            int count = user.Inwards.Where(c => c.Date_in >= startDateTime && c.Date_in < endDateTime && c.T_Dept == "IPR Cell").Count();
            using (var db = new TapalsEntities())
            {
                List<TapalsEntities> str = new List<TapalsEntities>();
                var query = (from pro in db.Inwards
                             where pro.T_Section == "IPR Cell" && pro.Date_in >= startDateTime && pro.Date_in < endDateTime
                             select new sample_User()
                             {
                                 Id = pro.Id,
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

        public ActionResult ICSRFacilitygrid()
        {
            DateTime startDateTime = DateTime.Today; //Today at 00:00:00
            DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
            int count = user.Inwards.Where(c => c.Date_in >= startDateTime && c.Date_in < endDateTime && c.T_Dept == "ICSR Facility").Count();
            using (var db = new TapalsEntities())
            {
                List<TapalsEntities> str = new List<TapalsEntities>();
                var query = (from pro in db.Inwards
                             where pro.T_Section == "ICSR Facility" && pro.Date_in >= startDateTime && pro.Date_in < endDateTime
                             select new sample_User()
                             {
                                 Id = pro.Id,
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

        public ActionResult Officegrid()
        {
            DateTime startDateTime = DateTime.Today; //Today at 00:00:00
            DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
            int count = user.Inwards.Where(c => c.Date_in >= startDateTime && c.Date_in < endDateTime && c.T_Dept == "Office").Count();
            using (var db = new TapalsEntities())
            {
                List<TapalsEntities> str = new List<TapalsEntities>();
                var query = (from pro in db.Inwards
                             where pro.T_Section == "Office" && pro.Date_in >= startDateTime && pro.Date_in < endDateTime
                             select new sample_User()
                             {
                                 Id = pro.Id,
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
        public JsonResult AutocompleteSuggestions(string searchstring)
        {
    TapalsEntities db = new TapalsEntities();

        var suggestions = from s in db.Subjects
                          select s.Subject_Name;
        var namelist = suggestions.Where(n => n.Contains(searchstring)).ToList().Take(7);
    return Json(namelist, JsonRequestBehavior.AllowGet);
    }


    public JsonResult UpdateInwardDetail(string name, string chequeIssuedList)
        {
            using (var entities = new TapalsEntities())
            {
                if (!string.IsNullOrEmpty(chequeIssuedList))
                {
                 var chequeIdList = chequeIssuedList.Split(',');
                    foreach (var cheque in chequeIdList)
                    {
                        long inwardId = Convert.ToInt64(cheque);
                        if (inwardId > 0)
                        {
                            var dbData = (from hc in entities.Inwards
                                          where hc.Id == inwardId
                                          select hc).SingleOrDefault();
                            if (dbData != null)
                            {
                                dbData.IssueName = name;
                                dbData.Issue = true;
                                entities.SaveChanges();
                            }
                        }
                    }
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
