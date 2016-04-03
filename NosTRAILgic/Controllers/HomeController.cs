using System;
using System.Linq;
using System.Web.Mvc;
using NosTRAILgic.DAL;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using NosTRAILgic.Models;
using NosTRAILgic.Services;

namespace NosTRAILgic.Controllers
{
    /************************************************************************************
     * Description: This controller manages the handling of Home View                   *
     *                                                                                  *
     * Developer: Yun Yong                                                              *
     *                                                                                  *
     * Date: 17/02/2016                                                                 *
     ************************************************************************************/
    public class HomeController : Controller
    {
        WeatherForecastGateway weatherService = new WeatherForecastGateway();           // New Gateway: WeatherForecastGateway
        SearchGateway searchGateway = new SearchGateway();                              // New Gateway: SearchGateway

        HomeMapper homeMapper = new HomeMapper();                                       // New Mapper: HomeMapper
        

        public ActionResult Index(string Selection)
        {
            List<Home_ViewModel> listHomeViewModel = new List<Home_ViewModel>();        // New List: for Home_ViewModel

            Home_ViewModel homeViewModel = new Home_ViewModel();                        // New ViewModel: Home_ViewModel

            homeViewModel.enumerableAllLocation = homeMapper.getAllLocationInfo("All");
            // getAllLocationWeather(Category , is to take current hours -1 data?)
            homeViewModel.enumerableAllWeather = validateCategoryWeatherData("All");            
            listHomeViewModel.Add(homeViewModel);

            //Passing data to javascript
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            ViewBag.homeViewModelJSON = oSerializer.Serialize(homeViewModel);

            if (User.Identity.IsAuthenticated)                                              
                ViewBag.loginStatus = "Login";
            else
                ViewBag.loginStatus = "Not Login";

            return View(listHomeViewModel);
        }

        [HttpPost]
        public ActionResult Index(string Selection, string searchKeyword)
        {
            List<Home_ViewModel> listHomeViewModel = new List<Home_ViewModel>();        // New List: for Home_ViewModel

            Home_ViewModel homeViewModel = new Home_ViewModel();                        // New ViewModel: Home_ViewModel

            if (searchKeyword != null && searchKeyword != "")
            {
                if (homeMapper.getLocationInfo(searchKeyword).Count() != 0)
                {
                    homeViewModel.enumerableAllLocation = homeMapper.getLocationInfo(searchKeyword);
                    homeViewModel.enumerableAllWeather = validateSearchWeatherData(searchKeyword);

                    listHomeViewModel.Add(homeViewModel);

                    /************************************************************************************
                     * Description: This function update theh search DB with the keyword that the       *
                     *              users has searched                                                  *
                     *                                                                                  *
                     * Developer: Yun Yong                                                              *
                     *                                                                                  *
                     * Date: 01/04/2016                                                                 *
                     ************************************************************************************/
                    Search searchModel = new Search();
                    searchModel.Keyword = searchKeyword;
                    searchModel.Date = DateTime.Now;

                    searchGateway.Insert(searchModel);
                }
            }
            else {
                if (!String.IsNullOrEmpty(Selection))
                {
                    if (Selection == "1")                                               // Musuem selected 
                    {
                        homeViewModel.enumerableAllLocation = homeMapper.getAllLocationInfo("Museum");
                        homeViewModel.enumerableAllWeather = validateCategoryWeatherData("Museum");
                        listHomeViewModel.Add(homeViewModel);
                    }
                    else if (Selection == "2")                                          // HistoricSites selected
                    {
                        homeViewModel.enumerableAllLocation = homeMapper.getAllLocationInfo("Monument");
                        homeViewModel.enumerableAllWeather = validateCategoryWeatherData("Monument");
                        listHomeViewModel.Add(homeViewModel);
                    }
                    else if (Selection == "3")                                          // Monuments selected
                    {
                        homeViewModel.enumerableAllLocation = homeMapper.getAllLocationInfo("HistoricSite");
                        homeViewModel.enumerableAllWeather = validateCategoryWeatherData("HistoricSite");
                        listHomeViewModel.Add(homeViewModel);
                    }
                    else
                    {
                        homeViewModel.enumerableAllLocation = homeMapper.getAllLocationInfo("All");
                        homeViewModel.enumerableAllWeather = validateCategoryWeatherData("All");
                        listHomeViewModel.Add(homeViewModel);
                    }
                }
                else
                {
                    homeViewModel.enumerableAllLocation = homeMapper.getAllLocationInfo("All");
                    homeViewModel.enumerableAllWeather = validateCategoryWeatherData("All");
                    listHomeViewModel.Add(homeViewModel);
                }
            }

            //Passing data to javascript
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            ViewBag.homeViewModelJSON = oSerializer.Serialize(homeViewModel);

            if (User.Identity.IsAuthenticated)
                ViewBag.loginStatus = "Login";
            else
                ViewBag.loginStatus = "Not Login";

            return View(listHomeViewModel);
        }

        public IEnumerable<Weather> validateCategoryWeatherData(string category)
        {
            IEnumerable<Weather> enumerableAllWeather = homeMapper.getAllLocationWeather(category, false);
            
            // If not updated weather forecast
            if (enumerableAllWeather.Count() == 0)
            {
                // Update database with lastest forecast
                weatherService.getNowcast();
                // Retrieve from database again
                enumerableAllWeather = homeMapper.getAllLocationWeather(category, false);

                if (enumerableAllWeather.Count() == 0)
                {
                    // Delete Duplicate weather forecast in database (a job for mapper or service)
                    homeMapper.removeDuplicateWeatherData();
                    // Update database with lastest forecast
                    weatherService.getNowcast();
                    // Retrieve from database again
                    enumerableAllWeather = homeMapper.getAllLocationWeather(category, true);
                }
            }       

            return enumerableAllWeather;
        }

        public IEnumerable<Weather> validateSearchWeatherData(string search)
        {
            IEnumerable<Weather> enumerableWeather = homeMapper.getLocationWeather(search, false);
            // If not updated weather forecast
            if (enumerableWeather.Count() == 0)
            {
                // Update database with lastest forecast
                weatherService.getNowcast();
                // Retrieve from database again
                enumerableWeather = homeMapper.getLocationWeather(search, false);
                if (enumerableWeather.Count() == 0)
                {
                    // Delete Duplicate weather forecast in database (a job for mapper or service)
                    homeMapper.removeDuplicateWeatherData();
                    // Update database with lastest forecast
                    weatherService.getNowcast();
                    // Retrieve from database again
                    enumerableWeather = homeMapper.getLocationWeather(search, true);
                }
            }
            return enumerableWeather;
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
            var searchAutoComplete = homeMapper.getSearchAutoComplete(term);

            return Json(searchAutoComplete, JsonRequestBehavior.AllowGet);
        }

        /************************************************************************************
         * Description: This function handles profile of the user account                   *
         *                                                                                  *
         * Developer: Yun Yong                                                              *
         *                                                                                  *
         * Date: 26/03/2016                                                                 *
         ************************************************************************************/
        [Authorize]
        public ActionResult UserProfile()
        {
            List<ProfileViewModel> listProfileViewModel = new List<ProfileViewModel>();       // New List: for ProfileViewModel

            ProfileViewModel profileViewModel = new ProfileViewModel();                       // New ViewModel: ProfileViewModel

            profileViewModel.enumerableJoinedTrails = homeMapper.getJoinedTrails(User.Identity.Name);

            profileViewModel.enumerableCreatedTrails = homeMapper.getCreatedTrails(User.Identity.Name);

            listProfileViewModel.Add(profileViewModel);

            return View(listProfileViewModel);
        }

        /************************************************************************************
         * Description: This function handles CheckIn                                       *
         *                                                                                  *
         * Developer: Elson                                                                 *
         *                                                                                  *
         * Date: 02/04/2016                                                                 *
         ************************************************************************************/
        public ActionResult CheckIn()
        {
            //Increase Checkin count
            //If user did not checkin for today

            //else increase the number
            System.Diagnostics.Debug.WriteLine("CHECKIN");
            return RedirectToAction("Index", "Home");
        }

    }
}