using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NosTRAILgic.DAL;
using NosTRAILgic.Models;
using System.Xml;

namespace NosTRAILgic.Controllers
{
    public class UploadController : Controller
    {
        private NosTRAILgicContext db = new NosTRAILgicContext();

        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }

        // GET: Upload/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Upload/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Upload/Create
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase MuseumFile)
        {
            try
            {
                string path = Server.MapPath("~/Content/KML/" + MuseumFile.FileName);
                MuseumFile.SaveAs(path);
                System.Diagnostics.Debug.WriteLine(path);

                // TODO: Add insert logic here
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);
                XmlNodeList name = xmlDoc.GetElementsByTagName("name");
                XmlNodeList desc = xmlDoc.GetElementsByTagName("description");
                XmlNodeList corrdinate = xmlDoc.GetElementsByTagName("coordinates");
                //System.Diagnostics.Debug.WriteLine(desc[0].InnerText);
                System.Diagnostics.Debug.WriteLine(corrdinate[0].InnerText);

                //for (int i = 0; i < currencies.Count; i++)
                //{
                //    if (currencies[i].InnerText.Length > 0)
                //    {
                //        currencyItems.Add(new SelectListItem { Text = currencies[i].InnerText, Value = i.ToString(), Selected = false });
                //    }
                //}

                return RedirectToAction("Create");
            }
            catch
            {
                return View();
            }
        }

        // GET: Upload/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Upload/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Upload/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Upload/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
