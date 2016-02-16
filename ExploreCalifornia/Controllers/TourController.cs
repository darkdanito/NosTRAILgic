using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExploreCalifornia.DAL;
using ExploreCalifornia.Models;

namespace ExploreCalifornia.Controllers
{
    public class TourController : GeneralController<Tour>
    {

        public TourController()
        {
            dataGateway = new TourGateway();
        }

        // GET: Tour
        public override ActionResult Index(int? id)
        {
            return View(dataGateway.SelectAll());
        }

        public ActionResult IndexSortByPrice(int? currencyIndex)
        {
            return View("Index",((TourGateway)dataGateway).SelectAllSortByPrice());
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: Tour/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Length,Price,Rating,IncludesMeals")] Tour tour)
        {
            if (ModelState.IsValid)
            {
                dataGateway.Insert(tour);
                return RedirectToAction("Index");
            }

            return View(tour);
        }

        // POST: Tour/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Length,Price,Rating,IncludesMeals")] Tour tour)
        {
            if (ModelState.IsValid)
            {
                dataGateway.Update(tour);
                return RedirectToAction("Index");
            }
            return View(tour);
        }
    }
}
