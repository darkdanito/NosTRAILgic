using NosTRAILgic.DAL;
using System.Net;
using System.Web.Mvc;

namespace NosTRAILgic.Controllers
{
    /************************************************************************************
     * Description: This is the General controller for NosTRAILgic                      *
     *                                                                                  *
     * Developer: Yun Yong                                                              *
     *                                                                                  *
     * Date: 17/02/2016                                                                 *
     ************************************************************************************/
    public class GeneralController<T> : Controller where T :class
    {
        internal IDataGateway<T> dataGateway;

        //virtual public ActionResult Index()
        //{
        //    return View(dataGateway.SelectAll());
        //}

        public virtual ActionResult Edit(int? id)
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

        public virtual ActionResult Delete(int? id)
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
        public virtual ActionResult DeleteConfirmed(int id)
        {
            dataGateway.Delete(id);
            return RedirectToAction("Index");
        }
    }
}