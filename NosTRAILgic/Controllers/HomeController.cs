using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NosTRAILgic.DAL;
using NosTRAILgic.Models;
using System.Web.Helpers;

namespace NosTRAILgic.Controllers
{
    public class HomeController : Controller
    {
        private NosTRAILgicContext db = new NosTRAILgicContext();

        JoinTrail jointrail = new JoinTrail();

        public ActionResult Index(string Selection)
        {
            if (!String.IsNullOrEmpty(Selection))
            {
                if (Selection == "1")
                {
                    ViewBag.modSitesName = "https://www.dropbox.com/s/dxd7uqm8tmztwyg/MUSEUM.kml?dl=1";
                }
                else if (Selection == "2")
                {
                    ViewBag.modSitesName = "https://www.dropbox.com/s/uqatxl7gxiz9lzl/HISTORICSITES.kml?dl=1";
                }
                else if (Selection == "3")
                {
                    ViewBag.modSitesName = "https://www.dropbox.com/s/pg72q12826s6et2/MONUMENTS.kml?dl=1";
                }

            }
            else
            {
                ViewBag.modSitesName = "https://www.dropbox.com/s/dxd7uqm8tmztwyg/MUSEUM.kml?dl=1";
            }

            return View();
        }

        public ActionResult Stats()
        {
            return View();
        }

        public ActionResult GetLocation(string term)
        {
            var result = from r in db.Locations
                         where r.Name.ToLower().StartsWith(term)
                         select r.Name;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult necrodiver()
        {
            //    var LINQTrailParticipantsQuery = from p in db.JoinTrails where p.TrailMeetupID == 1 select p.UserID;
            //    var LINQTrailParticipantsQuery = from p in db.JoinTrails select p.TrailMeetupID ;

            var orders = (from c in db.JoinTrails
                          group c by c.TrailMeetupID into g
                          select new { TrailMeetup = g.Key, Number = g.Count() });



            var bytes = new Chart(width: 600, height: 400)
                       .AddTitle("Orders")
                       .DataBindTable(dataSource: orders, xField: "PaymentTypeID")
                       .GetBytes("png");

            return File(bytes, "image/bytes");
        }

        public ActionResult CreateBar()
        {
            //Create bar chart
            var chart = new Chart(width: 300, height: 200)
            .AddSeries(chartType: "bar",
                            xValue: new[] { "10 ", "50", "30 ", "70" },
                            yValues: new[] { "50", "70", "90", "110" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreatePie()
        {
            //Create bar chart
            var chart = new Chart(width: 300, height: 200)
            .AddSeries(chartType: "pie",
                            xValue: new[] { "10 ", "50", "30 ", "70" },
                            yValues: new[] { "50", "70", "90", "110" })
                            .GetBytes("png");
            return File(chart, "image/bytes");
        }

        public ActionResult CreateLine()
        {
            //Create bar chart
            var chart = new Chart(width: 600, height: 200)
            .AddSeries(chartType: "line",
                            xValue: new[] { "18 ", "19", "20 ", "21" },
                            yValues: new[] { "50", "60", "70", "75" })
                            .GetBytes("png");

            return File(chart, "image/bytes");
        }



    }
}