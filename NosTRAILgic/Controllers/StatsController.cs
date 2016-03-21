using Newtonsoft.Json;
using NosTRAILgic.DAL;
using NosTRAILgic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace NosTRAILgic.Controllers
{
    public class StatsController : Controller
    {
        private NosTRAILgicContext db = new NosTRAILgicContext();
        // GET: Stats
        public ActionResult Index()
        {
            var stats = db.Database.SqlQuery<Statistic>("select t.Name as TrailName,count(j.TrailMeetupID) as NumberOfParticipant from JoinTrails j right join TrailMeetups t on t.TrailMeetupID = j.TrailMeetupID GROUP BY t.Name").ToList();
            return View(stats);
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