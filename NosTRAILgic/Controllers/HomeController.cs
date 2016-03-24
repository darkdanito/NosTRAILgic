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

        HomeMapper homeMapper = new HomeMapper();                                   // New HomeMapper()

        public ActionResult Index(string Selection)
        {
            List<Home_ViewModel> listHomeViewModel = new List<Home_ViewModel>();     // New List for Home_ViewModel()

            Home_ViewModel homeViewModel = new Home_ViewModel();                   // New Home_ViewModel()

            //if (!String.IsNullOrEmpty(Selection))
            //{
            //    if (Selection == "1")                                               // Musuem     
            //    {
            //        homeViewModel.enumerableAllLocation = homeMapper.getAllLocationInfo("Musuem");
            //        listHomeViewModel.Add(homeViewModel);
            //    }
            //    else if (Selection == "2")                                          // HistoricSites
            //    {
            //        homeViewModel.enumerableAllLocation = homeMapper.getAllLocationInfo("Monument");
            //        listHomeViewModel.Add(homeViewModel);
            //    }
            //    else if (Selection == "3")                                          // Monuments
            //    {
            //        homeViewModel.enumerableAllLocation = homeMapper.getAllLocationInfo("HistoricSite");
            //        listHomeViewModel.Add(homeViewModel);
            //    }
            //    else
            //    {
            //        homeViewModel.enumerableAllLocation = homeMapper.getAllLocationInfo("All");
            //        listHomeViewModel.Add(homeViewModel);
            //    }
            //}
            //else
            //{
                
            //}

            homeViewModel.enumerableAllLocation = homeMapper.getAllLocationInfo("All");
            listHomeViewModel.Add(homeViewModel);

            return View(listHomeViewModel);
        }

        [HttpPost]
        public ActionResult Index(string Selection, string searchKeyword)
        {
            List<Home_ViewModel> listHomeViewModel = new List<Home_ViewModel>();     // New List for Home_ViewModel()

            Home_ViewModel homeViewModel = new Home_ViewModel();                   // New Home_ViewModel()

            if (searchKeyword != null && searchKeyword != "")
            {
                homeViewModel.enumerableAllLocation = homeMapper.getLocationInfo(searchKeyword);
                listHomeViewModel.Add(homeViewModel);
            }
            else {
                if (!String.IsNullOrEmpty(Selection))
                {
                    if (Selection == "1")                                               // Musuem     
                    {
                        homeViewModel.enumerableAllLocation = homeMapper.getAllLocationInfo("Musuem");
                        listHomeViewModel.Add(homeViewModel);
                    }
                    else if (Selection == "2")                                          // HistoricSites
                    {
                        homeViewModel.enumerableAllLocation = homeMapper.getAllLocationInfo("Monument");
                        listHomeViewModel.Add(homeViewModel);
                    }
                    else if (Selection == "3")                                          // Monuments
                    {
                        homeViewModel.enumerableAllLocation = homeMapper.getAllLocationInfo("HistoricSite");
                        listHomeViewModel.Add(homeViewModel);
                    }
                    else
                    {
                        homeViewModel.enumerableAllLocation = homeMapper.getAllLocationInfo("All");
                        listHomeViewModel.Add(homeViewModel);
                    }
                }
                else
                {
                    homeViewModel.enumerableAllLocation = homeMapper.getAllLocationInfo("All");
                    listHomeViewModel.Add(homeViewModel);
                }
            }


            

            return View(listHomeViewModel);
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