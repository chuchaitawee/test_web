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
        public ActionResult GetDataTable(DatatableParam param)
        {
            var draw = param.draw;
            var offset = param.start;
            var limit = param.length;
            //string ss = Request.Form.GetValues("search[value]").FirstOrDefault();
            var search = param.search;

            var data_table = db.tbl_test.AsNoTracking()
                .OrderBy(c => c.id)
                .Skip(offset).Take(limit);

            var records_total = db.tbl_test.Count();

            return Json(new
            {
                draw = draw,
                recordsTotal = records_total,
                recordsFiltered = records_total,
                data = data_table,
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