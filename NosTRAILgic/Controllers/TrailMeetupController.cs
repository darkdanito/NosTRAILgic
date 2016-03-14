﻿using System;
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

            //location.LocationId = 9000;
            //location.Name = "Boon Lay MRT";
            //location.AreaCode = 8888;
            //location.PostalCode = 9002;
            //location.Latitude = 01.33948;
            //location.Longitude = 103.70580;

            //db.Locations.Add(location);
            //db.SaveChanges();

            //location.LocationId = 9000;
            //location.Name = "Lakeside MRT";
            //location.AreaCode = 8887;
            //location.PostalCode = 9001;
            //location.Latitude = 01.34455;
            //location.Longitude = 103.72100;

            //db.Locations.Add(location);
            //db.SaveChanges();

            //location.LocationId = 9000;
            //location.Name = "Chinese Garden MRT";
            //location.AreaCode = 8886;
            //location.PostalCode = 9002;
            //location.Latitude = 01.34209;
            //location.Longitude = 103.73260;

            //db.Locations.Add(location);
            //db.SaveChanges();

            return RedirectToAction("Index", "Home");
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
                TrailParticipants += " , ";
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

            return View(trailMeetup);
        }

        // GET: TrailMeetup/Create
        public ActionResult Create()
        {
            TrailMeetup trail = new TrailMeetup();

            if (User.Identity.Name == null || User.Identity.Name == "")
            {
                trail.CreatorID = "Anonymous Creator";
            }
            else
            {
                trail.CreatorID = User.Identity.Name;
            }

            trail.Limit = 1;
            trail.Date = DateTime.Now;

            return View(trail);
        }

        // POST: TrailMeetup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TrailMeetupID,CreatorID,Name,Description,ImageLink,Date,TimeFrom,TimeTo,Limit")] TrailMeetup trailMeetup, HttpPostedFileBase file, String[] text)
//        public ActionResult Create([Bind(Include = "TrailMeetupID,CreatorID,Name,Description,Location,ImageLink,Date,TimeFrom,TimeTo,Limit")] TrailMeetup trailMeetup, HttpPostedFileBase file, String[] text)
        {
            if (ModelState.IsValid)
            {
                string path = Server.MapPath("~/Content/Upload/" + file.FileName);
                file.SaveAs(path);

                if (text != null)
                {
                    // String for storing all the location that the user has inputted into the Location Input form
                    string combinedLocation = "";
                    string parameterLocation = "";

                    for (int i = 0; i < text.Length; i++)
                    {
                        combinedLocation += text[i] + ",";

                        var idCount = db.Trails.OrderByDescending(r => r.TrailMeetupID).FirstOrDefault();

                        //var linqParticipantsQuery = 
                        //    (from p in db.Locations where p.Name == text[i] select p.LocationId).FirstOrDefault();


                        parameterLocation = text[i];

                        //int location = 0;

                        //foreach (var p in linqParticipantsQuery)
                        //{
                        //    location = p;
                        //}

                        trailMeetup_Location.TrailMeetupID = idCount.TrailMeetupID + 1;
                        trailMeetup_Location.LocationID = (from a in db.Locations where a.Name == parameterLocation select a.LocationId).FirstOrDefault();

                        db.TrailMeetup_Location.Add(trailMeetup_Location);
                        db.SaveChanges();
                    }

                    // Update the dynamic location input + the original input for the location
                //    trailMeetup.Location = combinedLocation + trailMeetup.Location;
                }
                trailMeetup.ImageLink = file.FileName;

                db.Trails.Add(trailMeetup);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(trailMeetup);
        }

        // GET: TrailMeetup/Edit/5
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
//        public ActionResult Edit([Bind(Include = "TrailMeetupID,CreatorID,Name,Description,Location,ImageLink,Date,TimeFrom,TimeTo,Limit")] TrailMeetup trailMeetup)
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
