using NosTRAILgic.Models;
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
            // Delete all previous Museum
            var deleteLocation = db.Locations.Where(location => location.Category == "Museum");
            db.Locations.RemoveRange(deleteLocation);
            db.SaveChanges();
        }

        public void removePreviousMonument()
        {
            // Delete all previous Monument
            var deleteLocation = db.Locations.Where(location => location.Category == "Monument");
            db.Locations.RemoveRange(deleteLocation);
            db.SaveChanges();
        }

        public void removePreviousHistoricSite()
        {
            // Delete all previous Historic Site
            var deleteLocation = db.Locations.Where(location => location.Category == "HistoricSite");
            db.Locations.RemoveRange(deleteLocation);
            db.SaveChanges();
        }
    }
}