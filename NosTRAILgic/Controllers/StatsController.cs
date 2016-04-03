using NosTRAILgic.DAL;
using NosTRAILgic.Models;
using NosTRAILgic.Libraries;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace NosTRAILgic.Controllers
{
    /************************************************************************************
     * Description: This controller manages the handling of Stats View                  *
     *                                                                                  *
     * Developer: Khaleef                                                               *
     *                                                                                  *
     * Date: 21/03/2016                                                                 *
     ************************************************************************************/
    public class StatsController : GeneralController<Statistic>
    {
        StatsMapper statsMapper = new StatsMapper();

        // GET: Stats
        List<SelectListItem> dropDownList_Month = new List<SelectListItem>();
        List<SelectListItem> dropDownList_Category = new List<SelectListItem>();

        public StatsController()
        {
            // Populate months to dropdownlist
            var janItem = new SelectListItem { Text = "January", Value = "1" };
            var febItem = new SelectListItem { Text = "February", Value = "2" };
            var marchItem = new SelectListItem { Text = "March", Value = "3" };
            var aprilItem = new SelectListItem { Text = "April", Value = "4" };
            var mayItem = new SelectListItem { Text = "May", Value = "5" };
            var junItem = new SelectListItem { Text = "June", Value = "6" };
            var julItem = new SelectListItem { Text = "July", Value = "7" };
            var augItem = new SelectListItem { Text = "August", Value = "8" };
            var sepItem = new SelectListItem { Text = "September", Value = "9" };
            var octItem = new SelectListItem { Text = "October", Value = "10" };
            var novItem = new SelectListItem { Text = "November", Value = "11" };
            var decItem = new SelectListItem { Text = "December", Value = "12" };

            dropDownList_Month.Add(janItem);
            dropDownList_Month.Add(febItem);
            dropDownList_Month.Add(marchItem);
            dropDownList_Month.Add(aprilItem);
            dropDownList_Month.Add(mayItem);
            dropDownList_Month.Add(junItem);
            dropDownList_Month.Add(julItem);
            dropDownList_Month.Add(augItem);
            dropDownList_Month.Add(sepItem);
            dropDownList_Month.Add(octItem);
            dropDownList_Month.Add(novItem);
            dropDownList_Month.Add(decItem);

            ViewBag.ddl_Month = dropDownList_Month;

            // Populate category to dropdownlist
            var museumItem = new SelectListItem { Text = "Museum", Value = "1" };
            var monumentItem = new SelectListItem { Text = "Monument", Value = "2" };
            var siteItem = new SelectListItem { Text = "Historic Site", Value = "3" };

            dropDownList_Category.Add(museumItem);
            dropDownList_Category.Add(monumentItem);
            dropDownList_Category.Add(siteItem);

            ViewBag.ddl_Category = dropDownList_Category;
        }
        public ActionResult Index(string valMonth, string searchKeyword, string valCategory)
        {
            LogWriter.Instance.LogInfo("StatsController /  Index");

            string category = "Museum";
            foreach (var item in ViewBag.ddl_Month)
            {
                if (item.Value == valMonth)
                {

                    item.Selected = true;
                }
            }

            foreach (var item in ViewBag.ddl_Category)
            {
                if (item.Value == valCategory)
                {

                    item.Selected = true;
                }
            }

            if (valMonth == "" || valMonth == null)
            {
                valMonth = "1";
            }

            if (valCategory == "" || valCategory == null || valCategory == "1")
            {
                category = "Museum";
            }
            else if (valCategory == "2")
            {
                category = "Monument";
            }
            else if (valCategory == "3")
            {
                category = "HistoricSite";
            }

            // Top Searched Location
            LogWriter.Instance.LogInfo("StatsController /  Top Searched Location");
            var statsByTopSearchLocation = statsMapper.GetStatsByTopSearchLocation(valMonth);
            ViewBag.statsByTopSearchLocation = statsByTopSearchLocation;

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            ViewBag.statsByTopSearchLocationJSON = oSerializer.Serialize(statsByTopSearchLocation);

            // Top Creator Name
            LogWriter.Instance.LogInfo("StatsController /  Top Creator Name");
            var statsByTopTrailContributor = statsMapper.GetStatsByTopTrailContributor();
            ViewBag.statsByTopTrailContributor = statsByTopTrailContributor;

            oSerializer = new JavaScriptSerializer();
            ViewBag.statsByTopTrailContributorJSON = oSerializer.Serialize(statsByTopTrailContributor);

            // Search Location
            LogWriter.Instance.LogInfo("StatsController /  Search Location");
            var searchLocationDay = statsMapper.GetSearchLocationDay(searchKeyword);
            ViewBag.searchLocationDay = searchLocationDay;

            oSerializer = new JavaScriptSerializer();
            ViewBag.searchLocationDayJSON = oSerializer.Serialize(searchLocationDay);

            // Checkin per Category based on current and previous year       
            LogWriter.Instance.LogInfo("StatsController /  Current Year");
            var statsByCheckInCurrentYear = statsMapper.GetStatsByCheckInCurrentYear(category);
            ViewBag.statsByCheckInCurrentYear = statsByCheckInCurrentYear;
            oSerializer = new JavaScriptSerializer();
            ViewBag.statsByCheckInCurrentYearJSON = oSerializer.Serialize(statsByCheckInCurrentYear);

            LogWriter.Instance.LogInfo("StatsController /  Previous Year");
            var statsByCheckInPreviousYear = statsMapper.GetStatsByCheckInPreviousYear(category);
            ViewBag.statsByCheckInPreviousYear = statsByCheckInPreviousYear;
            oSerializer = new JavaScriptSerializer();
            ViewBag.statsByCheckInPreviousYearJSON = oSerializer.Serialize(statsByCheckInPreviousYear);

            return View();
        }


        public ActionResult GetLocation(string term)
        {
            var result = statsMapper.GetAutoCompleteResult(term);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}