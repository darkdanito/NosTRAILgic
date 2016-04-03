using NosTRAILgic.DAL;
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

        virtual public ActionResult Index()
        {
            return View(dataGateway.SelectAll());
        }
    }
}