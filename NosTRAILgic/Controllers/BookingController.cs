using System;
using System.Web.Mvc;
using NosTRAILgic.DAL;
using NosTRAILgic.Models;

namespace NosTRAILgic.Controllers
{
    public class BookingController : GeneralController<Booking>
    {
        public BookingController()
        {
            dataGateway = new BookingGateway();
        }

        // GET: Booking/Create
        //public ActionResult Create(Tour tour)
        //{
        //    if (tour.Id == 0)
        //        return RedirectToAction("Index", "Tour");
        //    Booking booking = new Booking();
        //    booking.TourID = tour.Id;
        //    booking.TourName = tour.Name;
        //    booking.ClientID = User.Identity.Name;
        //    booking.DepartureDate = DateTime.Now;
        //    return View(booking);
        //}

        // POST: Booking/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookingID,TourID,TourName,ClientID,DepartureDate,NumberofPeople,FullName,Email,ContactNo,SpecialRequest")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                dataGateway.Insert(booking);
                return RedirectToAction("Index");
            }

            return View(booking);
        }

        // POST: Booking/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookingID,TourID,TourName,ClientID,DepartureDate,NumberofPeople,FullName,Email,ContactNo,SpecialRequest")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                dataGateway.Update(booking);
                return RedirectToAction("Index");
            }
            return View(booking);
        }
    }
}
