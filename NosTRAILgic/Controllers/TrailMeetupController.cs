using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NosTRAILgic.DAL;
using NosTRAILgic.Models;

namespace NosTRAILgic.Controllers
{
    public class TrailMeetupController : Controller
    {
        private NosTRAILgicContext db = new NosTRAILgicContext();

        // GET: TrailMeetup
        public ActionResult Index()
        {
            return View(db.Trails.ToList());
        }

        // GET: TrailMeetup/Details/5
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

            return View(trailMeetup);
        }

        // GET: TrailMeetup/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TrailMeetup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TrailMeetupID,CreatorID,Name,Description,Location,ImageLink,Date,TimeFrom,TimeTo,Limit")] TrailMeetup trailMeetup, HttpPostedFileBase file, String[] text)
        {
            if (ModelState.IsValid)
            {
                string path = Server.MapPath("~/Content/Upload/" + file.FileName);
                file.SaveAs(path);

                if (text != null)
                {
                    // String for storing all the location that the user has inputted into the Location Input form
                    string combinedLocation = "";

                    for (int i = 0; i < text.Length; i++)
                    {
                        combinedLocation += text[i] + ",";
                    }

                    // Update the dynamic location input + the original input for the location
                    trailMeetup.Location = combinedLocation + trailMeetup.Location;
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
        public ActionResult Edit([Bind(Include = "TrailMeetupID,CreatorID,Name,Description,Location,ImageLink,Date,TimeFrom,TimeTo,Limit")] TrailMeetup trailMeetup)
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
