using System;
using System.Web;
using System.Web.Mvc;
using NosTRAILgic.DAL;
using NosTRAILgic.Services;
using NosTRAILgic.Models;
using NosTRAILgic.Libraries;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Linq;

namespace NosTRAILgic.Controllers
{
    /************************************************************************************
     * Description: This controller manages the handling of TrailMeetup View            *
     *                                                                                  *
     * Developer: Yun Yong                                                              *
     *                                                                                  *
     * Date: 17/02/2016                                                                 *
     ************************************************************************************/
    public class TrailMeetupController : GeneralController<TrailMeetup>
    {
        TrailMeetupMapper trailMeetupMapper = new TrailMeetupMapper();                  // New Mapper: TrailMeetup
        JoinTrailGateway joinTrailGateway = new JoinTrailGateway();                     // New Gateway: JoinTrail
        TrailMeetupLocationGateway trailMeetupLocationGateway = new TrailMeetupLocationGateway(); // New Gateway: TraiLMeetupLocation
        WeatherForecastGateway weatherService = new WeatherForecastGateway();           // New Gateway: WeatherForecastGateway
        WeatherGateway weatherGateway = new WeatherGateway();

        public ActionResult Index()                                                     // Display the Index Page
        {
            LogWriter.Instance.LogInfo("TrailMeetupController / Index");
            return View(trailMeetupMapper.getTrailsByDate());
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
            LogWriter.Instance.LogInfo("TrailMeetupController / GetLocation");
            var searchAutoComplete = trailMeetupMapper.getSearchAutoComplete(term);

            return Json(searchAutoComplete, JsonRequestBehavior.AllowGet);
        }

        /************************************************************************************
         * Description: This function handles the update of the user joining trails         *
         *                                                                                  *
         * Developer: Yun Yong                                                              *
         *                                                                                  *
         * Date: 13/03/2016                                                                 *
         ************************************************************************************/
        [Authorize]
        public ActionResult JoinTrail(int? id)
        {
            LogWriter.Instance.LogInfo("TrailMeetupController / JoinTrail");
            JoinTrail jointrail = new JoinTrail();

            if (User.Identity.Name == null || User.Identity.Name == "")                 // Check is the username valid
            {
                return HttpNotFound();
            }
            else
            {
                jointrail.TrailMeetupID = (int)id;                                  
                jointrail.UserID = User.Identity.Name;                              
            }

            joinTrailGateway.Insert(jointrail);                                         // Insert the new JoinTrail details into the DB                                       

            return RedirectToAction("Details_ViewModel", "TrailMeetup", new { id = id });
        }

        /************************************************************************************
         * Description: This function handles the update of the user leaving trails         *
         *                                                                                  *
         * Developer: Yun Yong                                                              *
         *                                                                                  *
         * Date: 29/03/2016                                                                 *
         ************************************************************************************/
        [Authorize]
        public ActionResult LeaveTrail(int id)
        {
            LogWriter.Instance.LogInfo("TrailMeetupController / LeaveTrail");
            JoinTrail jointrail = new JoinTrail();

            if (User.Identity.Name == null || User.Identity.Name == "")                 // Check is the username valid
            {
                return HttpNotFound();
            }
            else
            {
                // Delete user from the TrailMeetup
                joinTrailGateway.Delete(trailMeetupMapper.getUserJoinTrailForDelete(id, User.Identity.Name)); 
            }

            return RedirectToAction("Details_ViewModel", "TrailMeetup", new { id = id });
        }

        public ActionResult Details_ViewModel(int? id)
        {
            LogWriter.Instance.LogInfo("TrailMeetupController / Details_ViewModel");
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            // Convert the ID that was passed as a parameter into the function into INT as LINQ does not support such conversion
            int trailID = (int)id;

            // New List for Model: TrailMeetup_Details_ViewModel
            List<TrailMeetup_Details_ViewModel> listTrailMeetup_DetailsViewModel = new List<TrailMeetup_Details_ViewModel>();

            // New Model: TrailMeetup_Details_ViewModel
            TrailMeetup_Details_ViewModel trailMeetup_DetailsViewModel = new TrailMeetup_Details_ViewModel();  

            // Select the TrailMeetup by ID
            trailMeetup_DetailsViewModel.getTrailMeetup = trailMeetupMapper.SelectById(id);

            if (trailMeetup_DetailsViewModel.getTrailMeetup == null)                   // If the TraiMeetup cannot be found
            {
                return HttpNotFound();
            }

            /************************************************************************************
             * Description: This function check the database for any participants who have join *
             *              the trails and return it to the ViewModel                           *
             *                                                                                  *
             * Developer: Yun Yong                                                              *
             *                                                                                  *
             * Date: 13/03/2016                                                                 *
             ************************************************************************************/
            trailMeetup_DetailsViewModel.enumerableTrailParticipants = trailMeetupMapper.getTrailParticipants(trailID);


            /************************************************************************************
             * Description: This function check the JoinTrail Table to check has the user       *
             *              joined the TrailMeetup                                              *
             *                                                                                  *
             * Developer: Yun Yong                                                              *
             *                                                                                  *
             * Date: 13/03/2016                                                                 *
             ************************************************************************************/
            trailMeetup_DetailsViewModel.getUserinTrailMeetup = trailMeetupMapper.isUserInTrail(trailID, User.Identity.Name);


            /************************************************************************************
             * Description: This function handles the getting Locations and weather Information *
             *              from DB based on the Trail ID                                       *
             *                                                                                  *
             * Developer: Yun Yong                                                              *
             *                                                                                  *
             * Date: 21/03/2016                                                                 *
             ************************************************************************************/
            trailMeetup_DetailsViewModel.enumerableAllLocationFromTrail = trailMeetupMapper.getAllLocationInfoFromTrail(trailID);

            /************************************************************************************
             * Description: This function handles the getting Locations and weather Information *
             *              from DB based on the Trail ID                                       *
             *                                                                                  *
             * Developer: Yun Yong                                                              *
             *                                                                                  *
             * Date: 21/03/2016                                                                 *
             ************************************************************************************/
            trailMeetup_DetailsViewModel.enumerableAllWeatherFromTrail = validateTrailWeatherData(trailID);

            /************************************************************************************
             * Description: This function handles the getting Locations Information from DB     *
             *              based on the Trail ID                                               *
             *                                                                                  *
             * Developer: Khaleef                                                               *
             *                                                                                  *
             * Date: 26/03/2016                                                                 *
             ************************************************************************************/
            trailMeetup_DetailsViewModel.getNumberOfUsersInTrailMeetup = trailMeetupMapper.getTrailMeetupParticipantsCount(trailID);


            listTrailMeetup_DetailsViewModel.Add(trailMeetup_DetailsViewModel);         // Update the objects into the ViewModel

            // Passing data to javascript
            /************************************************************************************
             * Description: This function pass data to the javascript via ViewBag               *
             *                                                                                  *
             * Developer: Elson                                                                 *
             *                                                                                  *
             * Date: 29/03/2016                                                                 *
             ************************************************************************************/
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            ViewBag.detailsViewModelJSON = oSerializer.Serialize(trailMeetup_DetailsViewModel);
            if (User.Identity.IsAuthenticated)
                ViewBag.loginStatus = "Login";
            else
                ViewBag.loginStatus = "Not Login";

            return View(listTrailMeetup_DetailsViewModel);
        }

        /************************************************************************************
         * Description: This function handles the displaying of create view details         *
         *                                                                                  *
         *              This will return the create view to the user                        *
         *              It will default the parameter of the Number of Participants and     *
         *              the date of the Trail to be the current date                        *
         *                                                                                  *
         * Developer: Yun Yong                                                              *
         *                                                                                  *
         * Date: 13/03/2016                                                                 *
         ************************************************************************************/
        [Authorize]
        public ActionResult Create()
        {
            TrailMeetup trail = new TrailMeetup();

            trail.Limit = 1;                                                        // Set the default TrailMeetup Limit to be 1
            trail.Date = DateTime.Now;                                              // Set the default TrailMeetup Date to be today

            return View(trail);
        }

        // POST: TrailMeetup/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TrailMeetupID,Name,Description,ImageLink,Date,TimeFrom,TimeTo,Limit")] TrailMeetup trailMeetup, HttpPostedFileBase file, String[] text)
        {
            if (ModelState.IsValid)
            {
                LogWriter.Instance.LogInfo("TrailMeetupController / Create");

                JoinTrail jointrail = new JoinTrail();
                TrailMeetup_Location trailMeetup_Location = new TrailMeetup_Location();         // New TrailMeetup_Location Model()

                string path = Server.MapPath("~/Content/Upload/" + file.FileName);
                file.SaveAs(path);

                trailMeetup.ImageLink = file.FileName;
                trailMeetup.CreatorID = User.Identity.Name;

                DateTime currentTimeFrom = new DateTime(trailMeetup.Date.Year, trailMeetup.Date.Month,trailMeetup.Date.Day, trailMeetup.TimeFrom.Hour, trailMeetup.TimeFrom.Minute, 0,0);
                trailMeetup.TimeFrom = currentTimeFrom;
                DateTime currentTimeTo = new DateTime(trailMeetup.Date.Year, trailMeetup.Date.Month, trailMeetup.Date.Day, trailMeetup.TimeTo.Hour, trailMeetup.TimeTo.Minute, 0, 0);
                trailMeetup.TimeTo = currentTimeTo;

                trailMeetupMapper.Insert(trailMeetup);                                        // Insert the newly created TrailMeetup into DB
               
                // Get the Trail ID for the newly added Trail above
                int TrailID = trailMeetupMapper.getNewlyCreatedTrailID(trailMeetup.Name);

                if (text != null)
                {
                    for (int i = 0; i < text.Length; i++)
                    {
                        trailMeetup_Location.TrailMeetupID = TrailID;

                        // Convert the Location name to convert to the LocationID
                        //trailMeetup_Location.LocationID = trailMeetupMapper.getLocationID(text[i]);
                        trailMeetup_Location.LocationName = text[i];

                        trailMeetupLocationGateway.Insert(trailMeetup_Location);
                    }
                }

                // TrailMeetup Creator is automatically enrolled a TrailMeetup Participant 
                jointrail.TrailMeetupID = TrailID;
                jointrail.UserID = User.Identity.Name;
                joinTrailGateway.Insert(jointrail);

                return RedirectToAction("Index");
            }

            return View(trailMeetup);
        }

        // GET: TrailMeetup/Edit/5
        [Authorize]
        public override ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            
            TrailMeetup trailMeetup = trailMeetupMapper.SelectById(id);

            if (trailMeetup == null)
            {
                return HttpNotFound();
            }

            return View(trailMeetup);
        }

        // POST: TrailMeetup/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TrailMeetupID,Name,Description,ImageLink,Date,TimeFrom,TimeTo,Limit")] TrailMeetup trailMeetup)
        {
            if (ModelState.IsValid)
            {
                LogWriter.Instance.LogInfo("TrailMeetupController / Edit");

                trailMeetup.CreatorID = User.Identity.Name;

                trailMeetupMapper.Update(trailMeetup);

                return RedirectToAction("Index");
            }
            return View(trailMeetup);
        }

        // GET: TrailMeetup/Delete/5
        [Authorize]
        public override ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            TrailMeetup trailMeetup = trailMeetupMapper.SelectById(id);

            if (trailMeetup == null)
            {
                return HttpNotFound();
            }

            return View(trailMeetup);
        }

        /************************************************************************************
         * Description: This function handles the [POST: TrailMeetup/Delete]. It will       *
         *              delete the trailMeetup that the user choose to delete and will      *
         *              delete the data in JoinTrail and TrailMeetupLocation DB as well     *
         *                                                                                  *
         * Developer: Yun Yong                                                              *
         *                                                                                  *
         * Date: 30/03/2016                                                                 *
         ************************************************************************************/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public override ActionResult DeleteConfirmed(int id)
        {
            LogWriter.Instance.LogInfo("TrailMeetupController / Delete");

            TrailMeetup trailMeetup = trailMeetupMapper.SelectById(id);

            trailMeetupMapper.Delete(id);

            /************************************************************************************
             * Description: This function will get the list of users that has joined the trail  *
             *              and delete them from the database since the TrailMeeetup            *
             *              has been deleted                                                    *
             *                                                                                  *
             * Developer: Yun Yong                                                              *
             *                                                                                  *
             * Date: 30/03/2016                                                                 *
             ************************************************************************************/
            var recordsToDeleteFromJoinTrails = trailMeetupMapper.getTrailUserCreated(id);

            if (recordsToDeleteFromJoinTrails.Count > 0)
            {
                foreach (var record in recordsToDeleteFromJoinTrails)
                {
                    joinTrailGateway.Delete(record);
                }
            }
            
            /************************************************************************************
             * Description: This function will get the list of locations that was linked        *
             *              to the TrailMeetup and will be deleted from the database            *
             *              since the trailMeetup is deleted                                    *
             *                                                                                  *
             * Developer: Yun Yong                                                              *
             *                                                                                  *
             * Date: 30/03/2016                                                                 *
             ************************************************************************************/
            var recoardsToDeleteFromTrailMeetupLocations = trailMeetupMapper.getTrailUserJoined(id);

            if (recoardsToDeleteFromTrailMeetupLocations.Count > 0)
            {
                foreach (var record in recoardsToDeleteFromTrailMeetupLocations)
                {
                    trailMeetupLocationGateway.Delete(record);
                }
            }

            return RedirectToAction("Index");
        }

        /************************************************************************************
         * Description: This function handles logic behind retrieve latest weather forecast *
         *              (Validate and return weather based on Trail ID)                     *                                                                                  *
         * Developer: Elson                                                                 *
         *                                                                                  *
         * Date: 02/04/2016                                                                 *
         ************************************************************************************/
        public IEnumerable<Weather> validateTrailWeatherData(int trailID)
        {
            LogWriter.Instance.LogInfo("TrailMeetupController / validateTrailWeatherData");

            IEnumerable<Weather> enumerableAllWeather = trailMeetupMapper.getAllWeatherInfoFromTrail(trailID, false);

            // If not updated weather forecast
            if (enumerableAllWeather.Count() == 0)
            {
                // Update database with lastest forecast
                weatherService.GetNowcast();
                // Retrieve from database again
                enumerableAllWeather = trailMeetupMapper.getAllWeatherInfoFromTrail(trailID, false);

                if (enumerableAllWeather.Count() == 0)
                {
                    // Delete Duplicate weather forecast in database (a job for mapper or service)
                    weatherGateway.removeDuplicateWeatherData();
                    // Update database with lastest forecast
                    weatherService.GetNowcast();
                    // Retrieve from database again
                    enumerableAllWeather = trailMeetupMapper.getAllWeatherInfoFromTrail(trailID, true);
                }
            }

            return enumerableAllWeather;
        }
    }
}
