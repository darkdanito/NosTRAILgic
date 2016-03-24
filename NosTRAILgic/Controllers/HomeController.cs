using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using NosTRAILgic.DAL;

namespace NosTRAILgic.Controllers
{
    public class HomeController : Controller
    {
        private NosTRAILgicContext db = new NosTRAILgicContext();

        HomeMapper homeMapper = new HomeMapper();                  // Data Mapper

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


        public ActionResult GetLocation(string term)
        {
            var result = homeMapper.getSearchAutoComplete(term);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}