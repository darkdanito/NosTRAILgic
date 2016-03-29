using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NosTRAILgic.DAL;
using NosTRAILgic.Models;
using System.Collections.Generic;
using System.Web.Script.Serialization;

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
        TrailMeetupMapper trailMeetupMapper = new TrailMeetupMapper();                  // New TrailMeetupMapper()

        JoinTrailGateway joinTrailGateway = new JoinTrailGateway();
        TrailMeetupLocationGateway trailMeetupLocationGateway = new TrailMeetupLocationGateway();

        TrailMeetup_Location trailMeetup_Location = new TrailMeetup_Location();         // New TrailMeetup_Location Model()

        public TrailMeetupController()
        {
            
            dataGateway = new TrailMeetupMapper();                                      // Tie to TrailMeetup
        }

        public ActionResult Index()                                                     // Display the Index Page
        {
            return View(dataGateway.SelectAll());
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
            var result = trailMeetupMapper.getSearchAutoComplete(term);

            return Json(result, JsonRequestBehavior.AllowGet);
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
            JoinTrail jointrail = new JoinTrail();

            if (User.Identity.Name == null || User.Identity.Name == "")             // Check is the username valid
            {
                return HttpNotFound();
            }
            else
            {
                jointrail.TrailMeetupID = (int)id;
                jointrail.UserID = User.Identity.Name;
            }

            joinTrailGateway.Insert(jointrail);

            return RedirectToAction("Details_ViewModel", "TrailMeetup", new { id = id });
        }

        public ActionResult Details_ViewModel(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            List<TrailMeetup_Details_ViewModel> listTrailMeetup_DetailsViewModel = new List<TrailMeetup_Details_ViewModel>();       // New List for TrailMeetup_Details_ViewModel()

            TrailMeetup_Details_ViewModel trailMeetup_DetailsViewModel = new TrailMeetup_Details_ViewModel();  // New TrailMeetup_Details_ViewModel()

            trailMeetup_DetailsViewModel.getTrailMeetup = dataGateway.SelectById(id);

            if (trailMeetup_DetailsViewModel.getTrailMeetup == null)                   // If the TraiMeetup cannot be found
            {
                return HttpNotFound();
            }

            // Convert the ID that was passed as a parameter into the function into INT as LINQ does not support such conversion
            int trailID = (int)id;

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
             * Description: This function handles the getting Locations Information from DB     *
             *              based on the Trail ID                                               *
             *                                                                                  *
             * Developer: Yun Yong                                                              *
             *                                                                                  *
             * Date: 21/03/2016                                                                 *
             ************************************************************************************/
            trailMeetup_DetailsViewModel.enumerableAllLocationFromTrail = trailMeetupMapper.getAllLocationInfoFromTrail(trailID);

            
            /************************************************************************************
             * Description: This function handles the getting Locations Information from DB     *
             *              based on the Trail ID                                               *
             *                                                                                  *
             * Developer: Khaleef                                                               *
             *                                                                                  *
             * Date: 26/03/2016                                                                 *
             ************************************************************************************/
            trailMeetup_DetailsViewModel.getNumberOfUsersInTrailMeetup = trailMeetupMapper.getTrailMeetupParticipantsCount(trailID);


            listTrailMeetup_DetailsViewModel.Add(trailMeetup_DetailsViewModel);                            // Update the objects into the ViewModel

            // Passing data to javascript
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            ViewBag.detailsViewModelJSON = oSerializer.Serialize(trailMeetup_DetailsViewModel);

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

            trail.Limit = 1;
            trail.Date = DateTime.Now;

            return View(trail);
        }

        // POST: TrailMeetup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TrailMeetupID,Name,Description,ImageLink,Date,TimeFrom,TimeTo,Limit")] TrailMeetup trailMeetup, HttpPostedFileBase file, String[] text)
        {
            if (ModelState.IsValid)
            {
                JoinTrail jointrail = new JoinTrail();

                string path = Server.MapPath("~/Content/Upload/" + file.FileName);
                file.SaveAs(path);

                trailMeetup.ImageLink = file.FileName;
                trailMeetup.CreatorID = User.Identity.Name;

                dataGateway.Insert(trailMeetup);
               
                // Get the Trail ID for the newly added Trail above
                int TrailID = trailMeetupMapper.getNewlyCreatedTrailID(trailMeetup.Name);

                if (text != null)
                {
                    // String for storing all the location that the user has inputted into the Location Input form
                    string parameterLocation = "";

                    for (int i = 0; i < text.Length; i++)
                    {
                        parameterLocation = text[i];

                        trailMeetup_Location.TrailMeetupID = TrailID;

                        // Convert the Location name to convert to the LocationID
                        trailMeetup_Location.LocationID = trailMeetupMapper.getLocationID(parameterLocation);

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
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            
            TrailMeetup trailMeetup = dataGateway.SelectById(id);

            if (trailMeetup == null)
            {
                return HttpNotFound();
            }

            return View(trailMeetup);
        }

        // POST: TrailMeetup/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TrailMeetupID,CreatorID,Name,Description,ImageLink,Date,TimeFrom,TimeTo,Limit")] TrailMeetup trailMeetup)
        {
            if (ModelState.IsValid)
            {
                dataGateway.Update(trailMeetup);

                return RedirectToAction("Index");
            }
            return View(trailMeetup);
        }
    }
}
