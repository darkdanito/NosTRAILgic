using NosTRAILgic.DAL;
using System.Net;
using System.Web.Mvc;

namespace NosTRAILgic.Controllers
{
    public class GeneralController<T> : Controller where T :class
    {
        internal IDataGateway<T> dataGateway;

        virtual public ActionResult Index(int? id)
        {
            return View(dataGateway.SelectAll());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T obj = dataGateway.SelectById(id);
            if (obj == null)
            {
                return HttpNotFound();
            }
            return View(obj);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T obj = dataGateway.SelectById(id);
            if (obj == null)
            {
                return HttpNotFound();
            }
            return View(obj);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T obj = dataGateway.SelectById(id);
            if (obj == null)
            {
                return HttpNotFound();
            }
            return View(obj);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            dataGateway.Delete(id);
            return RedirectToAction("Index");
        }

    }
}