using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//using System.Data; //
using System.Data.Entity; //
using TestWeb.Models; //

namespace TestWeb.Controllers
{
    public class HomeController : Controller
    {
        // db
        private test_webEntities db = new test_webEntities();

        // test
        public ActionResult Index()
        {
            return View();
        }

        // get datatble
        [HttpPost]
        public ActionResult GetDataTable(DatatableParam param)
        {
            int draw = param.draw;
            int offset = param.start;
            int limit = param.length;
            string search = param.search["value"];
            string search1 = Request.Form.GetValues("search[value]").FirstOrDefault();
            string search2= Request.Form["search[value]"];

            var data_table = db.tbl_test.AsNoTracking()
                .OrderBy(c => c.id)
                .Skip(offset).Take(limit);

            var records_total = db.tbl_test.Count();

            return Json(new
            {
                draw = draw,
                search = search,
                search1 = search1,
                search2 = search2,
                recordsTotal = records_total,
                recordsFiltered = records_total,
                data = data_table,
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetDataTable2()
        {
            int draw = Convert.ToInt32(Request.Form["draw"]);
            int offset = Convert.ToInt32(Request.Form["start"]);
            int limit = Convert.ToInt32(Request.Form["length"]);


            string sortDirection = Request.Form["order[0][dir]"];
            int sortColIndex = Convert.ToInt32(Request.Form["order[0][column]"]);

            string search = Request.Form["search[value]"];
            string search1 = Request.Form.GetValues("search[value]").FirstOrDefault();

            //query
            var data_table = db.tbl_test.AsNoTracking().OrderBy(c => c.id); //.OrderBy(c => c.id)
            //records_total
            var records_total = data_table.Count();

            // where
            if (!string.IsNullOrEmpty(search))
            {
                //query
                data_table = db.tbl_test.AsNoTracking().Where(
                    c => c.name.ToLower().Contains(search.ToLower())
                    || c.lastname.ToLower().Contains(search.ToLower())
                    || c.email.ToLower().Contains(search.ToLower())
                ).OrderBy(c => c.id);

                //records_total
                records_total = data_table.Count();
            }

            // order by
            if (sortColIndex == 1)
            {
                data_table = sortDirection == "asc" ? data_table.OrderBy(c => c.name) : data_table.OrderByDescending(c => c.name);
            }
            else if (sortColIndex == 2)
            {
                data_table = sortDirection == "asc" ? data_table.OrderBy(c => c.lastname) : data_table.OrderByDescending(c => c.lastname);
            }
            else if (sortColIndex == 3)
            {
                data_table = sortDirection == "asc" ? data_table.OrderBy(c => c.email) : data_table.OrderByDescending(c => c.email);
            }
            else
            {
                int aa = 5555;
                data_table = data_table.OrderBy(c => c.id);
            }

            //
            var dispaly_data = data_table.Skip(offset).Take(limit).ToList();


            return Json(new
            {
                sortColIndex = sortColIndex,
                sortDirection = sortDirection,
                draw = draw,
                search = search,
                offset = offset,
                limit = limit,
                recordsTotal = records_total,
                recordsFiltered = records_total,
                data = dispaly_data,
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