using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//using System.Data; //
using System.Data.Entity; //
using TestWeb.Models; //
using System.Dynamic;//

namespace TestWeb.Controllers
{
    public class HomeController : Controller
    {
        // db
        private test_webEntities db = new test_webEntities();

        // test
        public ActionResult Index()
        {
            //dynamic mymodel = new ExpandoObject();
            var tbl_test = db.tbl_test.ToList();
            var tbl_member = db.tbl_member.ToList();

            var mymodel = new ResultViewModel { 
                _tbl_test = tbl_test, _tbl_member = tbl_member 
            };

            return View(mymodel);
        }

        // get datatble
        [HttpPost]
        //รับค่า form data ที่ post มาจาก datatable มาเก็บที่ Model
        public ActionResult GetDataTable(DatatableParam param)
        {
            int draw = param.draw;
            int offset = param.start; //query
            int limit = param.length;
            int column = 0; //param.order.Count() > 0 ? param.order[0].column : 0;
            string dir = null;

            if (param.order.Count() > 0)
            {
                column = param.order[0].column;
                dir = param.order[0].dir;
            }

            string search = param.search["value"]; //search

            //query
            var query = db.tbl_test.AsNoTracking().AsQueryable();
            //records_total
            int records_total = query.Count();

            //// if search query -> where
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(
                    c => c.name.ToLower().Contains(search.ToLower())
                    || c.lastname.ToLower().Contains(search.ToLower())
                    || c.email.ToLower().Contains(search.ToLower())
                );

                //records_total
                records_total = query.Count();
            }

            ////order by
            if (column == 1)
            {
                query = (dir == "asc") ? query.OrderBy(c => c.name) : query.OrderByDescending(c => c.name);
            }
            else if (column == 2)
            {
                query = (dir == "asc") ? query.OrderBy(c => c.lastname) : query.OrderByDescending(c => c.lastname);
            }
            else if (column == 3)
            {
                query = (dir == "asc") ? query.OrderBy(c => c.email) : query.OrderByDescending(c => c.email);
            }
            else
            {
                query = query.OrderBy(c => c.id);
            }

            //
            var dispaly_data = query.Skip(offset).Take(limit).ToList();
            return Json(new
            {
                column = column,
                dir = dir,
                draw = draw,
                search = search,
                recordsTotal = records_total,
                recordsFiltered = records_total,
                data = dispaly_data
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //ใช้ Request.Form get ค่า form data ที่ post มาจาก datatable
        public ActionResult GetDataTable2()
        {
            int draw = Convert.ToInt32(Request.Form["draw"]);
            int offset = Convert.ToInt32(Request.Form["start"]); //query
            int limit = Convert.ToInt32(Request.Form["length"]);

            int sortColIndex = Convert.ToInt32(Request.Form["order[0][column]"]); //sort
            string sortDirection = Request.Form["order[0][dir]"];

            string search = Request.Form["search[value]"]; //search
            string search1 = Request.Form.GetValues("search[value]").FirstOrDefault();

            //query
            var query = db.tbl_test.AsNoTracking().AsQueryable(); //.OrderBy(c => c.id)
            //records_total
            int records_total = query.Count();

            //// if search query -> where
            if (!string.IsNullOrEmpty(search))
            {
                query = db.tbl_test.AsNoTracking().Where(
                    c => c.name.ToLower().Contains(search.ToLower())
                    || c.lastname.ToLower().Contains(search.ToLower())
                    || c.email.ToLower().Contains(search.ToLower())
                ).AsQueryable();

                //records_total
                records_total = query.Count();
            }

            ////order by
            if (sortColIndex == 1)
            {
                query = sortDirection == "asc" ? query.OrderBy(c => c.name) : query.OrderByDescending(c => c.name);
            }
            else if (sortColIndex == 2)
            {
                query = sortDirection == "asc" ? query.OrderBy(c => c.lastname) : query.OrderByDescending(c => c.lastname);
            }
            else if (sortColIndex == 3)
            {
                query = sortDirection == "asc" ? query.OrderBy(c => c.email) : query.OrderByDescending(c => c.email);
            }
            else
            {
                //int aa = 5555;
                query = query.OrderBy(c => c.lastname);
            }

            //
            var dispaly_data = query.Skip(offset).Take(limit).ToList();

            return Json(new
            {
                //sortColIndex = sortColIndex,
                //sortDirection = sortDirection,
                //search = search,
                //offset = offset,
                //limit = limit,
                draw = draw,
                recordsTotal = records_total,
                recordsFiltered = records_total,
                data = dispaly_data,
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertData(string[] chkList)
        {
            string txt_name = Request.Form["txtName"];
            string txt_lastname = Request.Form["txtLastname"];
            string txt_email= Request.Form["txtEmail"];

            // test checkbox value
            var list = Request.Form["chkList"];
            var arr_list = Request.Form["chkList"].Split(',');
            var aa = Request.Form.GetValues("chkList").FirstOrDefault();
            var aaa = Request.Form["chkList"];

            // insert
            var data = new tbl_test
            {
                name = txt_name,
                lastname = txt_lastname,
                email = txt_email
            };
            db.tbl_test.Add(data);
            db.SaveChanges();

            //using (var context = new test_webEntities())
            //{ 
            //    var data = new tbl_test
            //    {
            //        name = txt_name,
            //        lastname = txt_lastname,
            //        email = txt_email
            //    };
            //    context.tbl_test.Add(data);
            //    context.SaveChanges();
            //}

            string msg = "บันทึกสำเร็จ";

            return Json(new
            {
                msg = msg,
                name = txt_name,
                lastname = txt_lastname,
                list = list,
                arr_list = arr_list,
                chkList = chkList
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteData(int user_id)
        {
            //int id1 = Convert.ToInt32(Request.Form["user_id"]);
            //tbl_test tbl_test = db.tbl_test.Find(user_id);
            tbl_test tbl_test = db.tbl_test
                .Where(a => a.id == user_id)
                .FirstOrDefault();

            db.tbl_test.Remove(tbl_test);
            db.SaveChanges();

            string msg = "ลบข้อมูลสำเร็จ";

            return Json(new
            {
                msg = msg
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}