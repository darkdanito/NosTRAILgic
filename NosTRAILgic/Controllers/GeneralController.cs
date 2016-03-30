using NosTRAILgic.DAL;
using System.Web.Mvc;

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

    }
}