using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NosTRAILgic.DAL;
using NosTRAILgic.Models;
using System.Collections.Generic;

namespace NosTRAILgic.Controllers
{
    /************************************************************************************
     * Description: This controller manages the handling of TrailMeetup View            *
     *                                                                                  *
     * Developer: Yun Yong                                                              *
     *                                                                                  *
     * Date: 17/02/2016                                                                 *
     ************************************************************************************/
    public class TrailMeetupController : Controller
    {
        private NosTRAILgicContext db = new NosTRAILgicContext();

        JoinTrail jointrail = new JoinTrail();
        Location location = new Location();

        TrailMeetup_Location trailMeetup_Location = new TrailMeetup_Location();


        TrailMeetupMapper trailMeetupMapper = new TrailMeetupMapper();                  // New TrailMeetupMapper()

        // GET: TrailMeetup
        public ActionResult Index()
        {
            return View(db.Trails.ToList());
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
            if (User.Identity.Name == null || User.Identity.Name == "")             // Check is the username valid
            {
                return HttpNotFound();
            }
            else
            {
                jointrail.TrailMeetupID = (int)id;
                jointrail.UserID = User.Identity.Name;
            }

            db.JoinTrails.Add(jointrail);
            db.SaveChanges();

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

            trailMeetup_DetailsViewModel.getTrailMeetup = db.Trails.Find(id);          // Find TrailMeetup by id

            if (trailMeetup_DetailsViewModel.getTrailMeetup == null)                   // If the TraiMeetup cannot be found
            {
                return HttpNotFound();
            }

            // Convert the ID that was passed as a parameter into the function into INT
            // as LINQ does not support such conversion
            int trailID = (int)id;

            /************************************************************************************
             * Description: This function check the database for any participants who have join *
             *              the trails and return it to the ViewModel                           *
             *                                                                                  *
             * Developer: Yun Yong                                                              *
             *                                                                                  *
             * Date: 13/03/2016                                                                 *
             ************************************************************************************/
            var LINQtoList = trailMeetupMapper.getTrailParticipants(trailID);
            trailMeetup_DetailsViewModel.enumerableTrailParticipants = LINQtoList;


            /************************************************************************************
             * Description: This function check the JoinTrail Table to check has the user       *
             *              joined the TrailMeetup                                              *
             *                                                                                  *
             * Developer: Yun Yong                                                              *
             *                                                                                  *
             * Date: 13/03/2016                                                                 *
             ************************************************************************************/
            var LINQIsUserInTrailQuery = trailMeetupMapper.isUserInTrail(trailID, User.Identity.Name);

            ViewBag.linqUserExistTest = LINQIsUserInTrailQuery;

            /************************************************************************************
             * Description: This function handles the getting Locations Information from DB     *
             *              based on the Trail ID                                               *
             *                                                                                  *
             * Developer: Yun Yong                                                              *
             *                                                                                  *
             * Date: 21/03/2016                                                                 *
             ************************************************************************************/
            var LINQAllLocationQuery = trailMeetupMapper.getAllLocationInfoFromTrail(trailID);

            trailMeetup_DetailsViewModel.enumerableAllLocationFromTrail = LINQAllLocationQuery;

            listTrailMeetup_DetailsViewModel.Add(trailMeetup_DetailsViewModel);                            // Update the objects into the ViewModel

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
                string path = Server.MapPath("~/Content/Upload/" + file.FileName);
                file.SaveAs(path);

                trailMeetup.ImageLink = file.FileName;
                trailMeetup.CreatorID = User.Identity.Name;

                db.Trails.Add(trailMeetup);
                db.SaveChanges();

                var LINQCreatedTrailIDQuery = trailMeetupMapper.getNewlyCreatedTrailID(trailMeetup.Name);

                int TrailID = 0;

                foreach (var Lat in LINQCreatedTrailIDQuery)
                {
                    TrailID = Lat;
                }

                if (text != null)
                {
                    // String for storing all the location that the user has inputted into the Location Input form
                    string parameterLocation = "";

                    for (int i = 0; i < text.Length; i++)
                    {
                        parameterLocation = text[i];

                        trailMeetup_Location.TrailMeetupID = TrailID;
                        trailMeetup_Location.LocationID = (from a in db.Locations where a.Name == parameterLocation select a.LocationId).FirstOrDefault();

                        db.TrailMeetup_Location.Add(trailMeetup_Location);
                        db.SaveChanges();
                    }

                }

                jointrail.TrailMeetupID = TrailID;
                jointrail.UserID = User.Identity.Name;
                db.JoinTrails.Add(jointrail);
                db.SaveChanges();

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

            TrailMeetup trailMeetup = db.Trails.Find(id);

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
                db.Entry(trailMeetup).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(trailMeetup);
        }

        // GET: TrailMeetup/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            TrailMeetup trailMeetup = db.Trails.Find(id);

            if (trailMeetup == null)
            {
                return HttpNotFound();
            }

            return View(trailMeetup);
        }

        // POST: TrailMeetup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TrailMeetup trailMeetup = db.Trails.Find(id);
            db.Trails.Remove(trailMeetup);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
