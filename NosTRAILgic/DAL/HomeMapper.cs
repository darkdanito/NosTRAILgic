using NosTRAILgic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NosTRAILgic.DAL
{
    /************************************************************************************
     * Description: This the data mapper for HomeController                             *
     *                                                                                  *
     * Developer: Yun Yong                                                              *
     *                                                                                  *
     * Date: 24/03/2016                                                                 *
     ************************************************************************************/
    public class HomeMapper
    {
        private NosTRAILgicContext db = new NosTRAILgicContext();



        public IQueryable<string> getSearchAutoComplete(string term)
        {
            var result = from r in db.Locations
                         where r.Name.ToLower().StartsWith(term)
                         select r.Name;

            return result;
        }

        /************************************************************************************
         * Description: This function will take in the TrailMeetup ID and return            *
         *              Location table that the TrailMeetup contains by joining the DB      *
         *              TrailMeetup_Location and TrailMeetup_Location with TrailMeetup      *
         *                                                                                  *
         ************************************************************************************/
        public IQueryable<Location> getAllLocationInfo(string trailCategory)
        {
            IQueryable<Location> LINQAllLocationBasedOnCat;

            if (trailCategory == "All")
            {
                LINQAllLocationBasedOnCat = from a in db.Locations
                                            select a;
            }
            else {

                LINQAllLocationBasedOnCat = from a in db.Locations
                                            where a.Category == trailCategory
                                            select a;

            }
            
            return LINQAllLocationBasedOnCat;
        }
    }
}