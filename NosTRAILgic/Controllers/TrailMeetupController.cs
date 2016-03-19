using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NosTRAILgic.DAL;
using NosTRAILgic.Models;

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

        // GET: TrailMeetup
        public ActionResult Index()
        {
            return View(db.Trails.ToList());
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
                jointrail.TrailMeetupID = (int)id;
                jointrail.UserID = "testing_empty_username";
            }
            else
            {
                jointrail.TrailMeetupID = (int)id;
                jointrail.UserID = User.Identity.Name;
            }
                
            db.JoinTrails.Add(jointrail);
            db.SaveChanges();

            return RedirectToAction("Details", "TrailMeetup", new { id = id });
        }

        /************************************************************************************
         * Description: This function handles the displaying of details view details        *
         *                                                                                  *
         *              It also will check the database for any participants who have join  *
         *              the trails and return it to the viewbag: XXXXXX to be rendered      *
         *              by the view                                                         *
         *                                                                                  *
         *              As the TrailMeetups DB store the location via ID, it will join      *
         *              TrailMeetups DB with TrailMeetup_Location DB and Locations DB       *
         *              to get the location names that the user have added to the trail.    *
         *              This will be passed to the view via viewbag: XXXXX and passed to    *
         *              Google API for rendering of the specific location                   *
         *                                                                                  *
         * Developer: Yun Yong                                                              *
         *                                                                                  *
         * Date: 13/03/2016                                                                 *
         ************************************************************************************/
        public ActionResult Details(int? id)
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


            /************************************************************************************
             * Description: This function check the database for any participants who have join *
             *              the trails and return it to the viewbag: XXXXXX to be rendered      *
             *              by the view                                                         *
             *                                                                                  *
             * Developer: Yun Yong                                                              *
             *                                                                                  *
             * Date: 13/03/2016                                                                 *
             ************************************************************************************/

            // LINQ Query to query who are the participants in the trails
            var LINQTrailParticipantsQuery = from p in db.JoinTrails where p.TrailMeetupID == id select p.UserID;

            // Var to store the Trail Participants in string
            var TrailParticipants = "";

            // Loop through the LINQ Query results and append to the TrailParticipants
            foreach (var p in LINQTrailParticipantsQuery)
            {
                TrailParticipants += p;
                TrailParticipants += ",";
            }

            // Update the View Bag so that it can be passed to the view
            ViewBag.participants = TrailParticipants;





           


            /************************************************************************************
             * Description: This function handles the displaying of getting location names      *
             *                                                                                  *
             *              As the TrailMeetups DB store the location via ID, it will join      *
             *              TrailMeetups DB with TrailMeetup_Location DB and Locations DB       *
             *              to get the location names that the user have added to the trail.    *
             *              This will be passed to the view via viewbag: XXXXX and passed to    *
             *              Google API for rendering of the specific location                   *
             *                                                                                  *
             * Developer: Yun Yong                                                              *
             *                                                                                  *
             * Date: 13/03/2016                                                                 *
             ************************************************************************************/

            // Convert the ID that was passed as a parameter into the function into INT
            // as LINQ does not support suchh conversion
            int trailID = (int)id;

            // LINQ Query to query the location name given the trail ID
            var LINQLocationQuery = from y in db.Trails
                         join x in db.TrailMeetup_Location on y.TrailMeetupID equals x.TrailMeetupID
                         join w in db.Locations on x.LocationID equals w.LocationId
                         where y.TrailMeetupID == trailID
                         select w.Name;

            // Var to store the Locations name into a string
            var AllLocation = "";

            // Loop through the LINQ Query results and append to the AllLocation
            foreach (var l in LINQLocationQuery)
            {
                AllLocation += l;
                AllLocation += ",";
            }

            // Update the View Bag so that it can be passed to the view
            ViewBag.linqLocationTest = AllLocation;





            var LINQLatQuery = from y in db.Trails
                                    join x in db.TrailMeetup_Location on y.TrailMeetupID equals x.TrailMeetupID
                                    join w in db.Locations on x.LocationID equals w.LocationId
                                    where y.TrailMeetupID == trailID
                                    select w.Latitude;

            var LINQLongQuery = from y in db.Trails
                                    join x in db.TrailMeetup_Location on y.TrailMeetupID equals x.TrailMeetupID
                                    join w in db.Locations on x.LocationID equals w.LocationId
                                    where y.TrailMeetupID == trailID
                                    select w.Longitude;


            var AllLat = "";
            var AllLong = "";

            foreach (var Lat in LINQLatQuery)
            {
                AllLat += Lat;
                AllLat += ",";
            }

            foreach (var Long in LINQLongQuery)
            {
                AllLong += Long;
                AllLong += ",";
            }

            ViewBag.linqLatTest = AllLat;
            ViewBag.linqLongTest = AllLong;





            string userName = User.Identity.Name;
            var LINQIsUserInTrailQuery = from p in db.JoinTrails where p.UserID == userName && p.TrailMeetupID == trailID select p.UserID;
            var userExist = "";
            foreach (var ex in LINQIsUserInTrailQuery)
            {
                userExist += ex;
            }
            ViewBag.linqUserExistTest = userExist;

            return View(trailMeetup);
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
                
                string inputTrailName = trailMeetup.Name;

                var LINQCreatedTrailIDQuery = from c in db.Trails where c.Name == inputTrailName select c.TrailMeetupID;

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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
