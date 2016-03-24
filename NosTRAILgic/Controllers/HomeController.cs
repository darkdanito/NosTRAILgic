using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using NosTRAILgic.DAL;
using System.Collections.Generic;
using NosTRAILgic.Models;

namespace NosTRAILgic.Controllers
{
    public class HomeController : Controller
    {
        private NosTRAILgicContext db = new NosTRAILgicContext();

        HomeMapper homeMapper = new HomeMapper();                                   // Home Data Mapper

        public ActionResult Index(string Selection)
        {
            List<Home_ViewModel> homeALLViewModel = new List<Home_ViewModel>();

            Home_ViewModel home_ViewModel = new Home_ViewModel();

            if (!String.IsNullOrEmpty(Selection))
            {
                if (Selection == "1")                                               // Musuem     
                {
                    home_ViewModel.enumerableAllLocation = homeMapper.getAllLocationInfo("Musuem");
                    homeALLViewModel.Add(home_ViewModel);
                }
                else if (Selection == "2")                                          // HistoricSites
                {
                    home_ViewModel.enumerableAllLocation = homeMapper.getAllLocationInfo("Monument");
                    homeALLViewModel.Add(home_ViewModel);
                }
                else if (Selection == "3")                                          // Monuments
                {
                    home_ViewModel.enumerableAllLocation = homeMapper.getAllLocationInfo("HistoricSite");
                    homeALLViewModel.Add(home_ViewModel);
                }
                else
                {
                    home_ViewModel.enumerableAllLocation = homeMapper.getAllLocationInfo("All");
                    homeALLViewModel.Add(home_ViewModel);
                }
            }
            else
            {
            //    ViewBag.modSitesName = "https://www.dropbox.com/s/dxd7uqm8tmztwyg/MUSEUM.kml?dl=1";

                home_ViewModel.enumerableAllLocation = homeMapper.getAllLocationInfo("All");
                homeALLViewModel.Add(home_ViewModel);
            }



            return View(homeALLViewModel);
        }

        /************************************************************************************
         * Description: This function handles the auto complete for the Location input      *
         *              box when user input the locations                                   *
         *                                                                                  *
         * Developer: Elson & Yun Yong                                                      *
         *                                                                                  *
         * Date: 22/03/2016                                                                 *
         ************************************************************************************/
        public ActionResult GetLocation(string term)
        {
            var result = homeMapper.getSearchAutoComplete(term);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}