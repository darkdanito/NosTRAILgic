using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NosTRAILgic.Controllers
{
    public class StatsController : Controller
    {
        // GET: Stats
        public ActionResult Index()
        {
            var numberOfParticipants = "SELECT TrailMeetupID, COUNT(*) AS count FROM JoinTrails GROUP BY TrailMeetupID";
            return View();
        }
    }
}