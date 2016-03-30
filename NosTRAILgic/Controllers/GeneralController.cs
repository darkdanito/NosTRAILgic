using NosTRAILgic.DAL;
using System.Net;
using System.Web.Mvc;
using System.Linq;
using NosTRAILgic.Models;

namespace NosTRAILgic.Controllers
{
    public class GeneralController<T> : Controller where T :class
    {
        internal IDataGateway<T> dataGateway;

        //private NosTRAILgicContext db = new NosTRAILgicContext();

        //virtual public ActionResult Index(int? id)
        //{
        //    return View(dataGateway.SelectAll());
        //}

        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    T obj = dataGateway.SelectById(id);
        //    if (obj == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(obj);
        //}

        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    T obj = dataGateway.SelectById(id);
        //    if (obj == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(obj);
        //}

//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return RedirectToAction("Index");
////              return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }

//            T obj = dataGateway.SelectById(id);

//            if (obj == null)
//            {
//                return HttpNotFound();
//            }

//            return View(obj);
//        }


        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    dataGateway.Delete(id);

        //    // Delete all row from Join Trails where trailMeetupID = ID
        //    var recordsToDeleteFromJoinTrails = (from r in db.JoinTrails
        //                                         where r.TrailMeetupID == id
        //                                         select r).ToList<JoinTrail>();

        //    if (recordsToDeleteFromJoinTrails.Count > 0)
        //    {
        //        foreach (var record in recordsToDeleteFromJoinTrails)
        //        {
        //            db.JoinTrails.Remove(record);
        //            db.SaveChanges();
        //        }
        //    }


        //    // Delete all row from TrailMeetup_Locations where trailMeetupID = ID
        //    var recoardsToDeleteFromTrailMeetupLocations = (from d in db.TrailMeetup_Location
        //                                                    where d.TrailMeetupID == id
        //                                                    select d).ToList<TrailMeetup_Location>();

        //    if (recoardsToDeleteFromTrailMeetupLocations.Count > 0)
        //    {
        //        foreach (var record in recoardsToDeleteFromTrailMeetupLocations)
        //        {
        //            db.TrailMeetup_Location.Remove(record);
        //            db.SaveChanges();
        //        }
        //    }

        //    return RedirectToAction("Index");
        //}

    }
}