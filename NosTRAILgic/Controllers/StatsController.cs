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
    /************************************************************************************
     * Description: This controller manages the handling of Stats View                  *
     *                                                                                  *
     * Developer: Khaleef                                                               *
     *                                                                                  *
     * Date: 21/03/2016                                                                 *
     ************************************************************************************/
    public class StatsController : Controller
    {
        private NosTRAILgicContext db = new NosTRAILgicContext();
        // GET: Stats
        public ActionResult Index()
        {
            var statsByParticipants = db.Database.SqlQuery<Statistic>("select t.Name as Name,count(j.TrailMeetupID) as Number from JoinTrails j right join TrailMeetups t on t.TrailMeetupID = j.TrailMeetupID GROUP BY t.Name").ToList();
            var statsByCategory = db.Database.SqlQuery<Statistic>("select Category as Name, count(CheckInID) as Number from CheckIns c right join Locations l on l.LocationID = c.LocationID group by Category").ToList();
            var statsBySearch = db.Database.SqlQuery<Statistic>("select Keyword as Name, YEAR(CONVERT(Date, DATE)) as Number from Searches").ToList();

            ViewBag.statsByCategory = statsByCategory;
            ViewBag.statsBySearch = statsBySearch;

            return View(statsByParticipants);
        }
    }
}