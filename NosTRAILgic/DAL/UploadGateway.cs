using NosTRAILgic.Models;
using NosTRAILgic.Libraries;
using System.Linq;

namespace NosTRAILgic.DAL
{
    /************************************************************************************
     * Description: Gateway for Upload Locations                                        *
     *                                                                                  *
     * Developer: Elson                                                                 *
     *                                                                                  *
     * Date: 14/03/2016                                                                 *
     ************************************************************************************/
    public class UploadGateway : DataGateway<Location>
    {
        public void removePreviousMuseum()
        {
            LogWriter.Instance.LogInfo("UploadGateway / removePreviousMuseum");
            // Delete all previous Museum
            var deleteLocation = db.Locations.Where(location => location.Category == "Museum");
            db.Locations.RemoveRange(deleteLocation);
            db.SaveChanges();
        }

        public void removePreviousMonument()
        {
            LogWriter.Instance.LogInfo("UploadGateway / removePreviousMonument");
            // Delete all previous Monument
            var deleteLocation = db.Locations.Where(location => location.Category == "Monument");
            db.Locations.RemoveRange(deleteLocation);
            db.SaveChanges();
        }

        public void removePreviousHistoricSite()
        {
            LogWriter.Instance.LogInfo("UploadGateway / removePreviousHistoricSite");
            // Delete all previous Historic Site
            var deleteLocation = db.Locations.Where(location => location.Category == "HistoricSite");
            db.Locations.RemoveRange(deleteLocation);
            db.SaveChanges();
        }
    }
}