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
        List<SelectListItem> dropDownList = new List<SelectListItem>();

        public StatsController()
        {
            var firstItem = new SelectListItem { Text = "Year", Value = "0" };
            var secondItem = new SelectListItem { Text = "Month", Value = "1" };
            dropDownList.Add(firstItem);
            dropDownList.Add(secondItem);
            ViewBag.statsBySearchLocationddl = dropDownList;
        }
        public ActionResult Index(string val)
        {
            foreach (var item in ViewBag.statsBySearchLocationddl)
            {
                if (item.Value == val)
                {

                    item.Selected = true;
                }
            }
            if (val == "" || val == null)
            {
                ViewBag.statsBySearchLocationddlDate = "0";
            }
            else
            {
                ViewBag.statsBySearchLocationddlDate = val;
            }

            var statsByParticipants = db.Database.SqlQuery<Statistic>("select t.Name as Name,count(j.TrailMeetupID) as Number from JoinTrails j right join TrailMeetups t on t.TrailMeetupID = j.TrailMeetupID GROUP BY t.Name").ToList();
            var statsByCategory = db.Database.SqlQuery<Statistic>("select Category as Name, count(CheckInID) as Number from CheckIns c right join Locations l on l.LocationID = c.LocationID group by Category").ToList();
            var statsBySearch = db.Database.SqlQuery<Statistic>("select Keyword as Name, YEAR(CONVERT(Date, DATE)) as Number from Searches").ToList();

            ViewBag.statsByParticipants = statsByParticipants;
            ViewBag.statsByCategory = statsByCategory;
            ViewBag.statsBySearch = statsBySearch;

            var statsBySearchLocationYear = db.Database.SqlQuery<Statistic>("select s.Keyword as Name, COUNT(YEAR(CONVERT(Date, DATE))) As Number, YEAR(CONVERT(Date, DATE)) as Date from Searches s inner join Locations l on s.Keyword = l.Name group by s.Keyword, YEAR(CONVERT(Date, DATE))").ToList();
            var statsBySearchLocationMonth = db.Database.SqlQuery<Statistic>("select s.Keyword as Name, COUNT(MONTH(CONVERT(Date, DATE))) As Number, MONTH(CONVERT(Date, DATE)) as Date from Searches s inner join Locations l on s.Keyword = l.Name group by s.Keyword, MONTH(CONVERT(Date, DATE))").ToList();

            ViewBag.statsBySearchLocationYear = statsBySearchLocationYear;
            ViewBag.statsBySearchLocationMonth = statsBySearchLocationMonth;

            return View();
        }
    }
}