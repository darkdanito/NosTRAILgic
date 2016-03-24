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
                                            orderby a.LocationId ascending
                                            select a;
            }
            else {

                LINQAllLocationBasedOnCat = from a in db.Locations
                                            where a.Category == trailCategory
                                            orderby a.LocationId ascending
                                            select a;
            }
            
            return LINQAllLocationBasedOnCat;
        }

        public IQueryable<Location> getLocationInfo(string searchLocation) {

            var LINQAllLocationBasedOnCat = from a in db.Locations
                                            where a.Name == searchLocation
                                            select a;

            return LINQAllLocationBasedOnCat;
        }

        public IQueryable<Weather> getAllLocationWeather(string trailCategory)
        {
            //Attempt get current DateTime without second and minute and millisecond
            DateTime currentDateTime = DateTime.Now;
            currentDateTime = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, currentDateTime.Hour, 0, 0, 0);

            IQueryable<Weather> LINQAllWeatherBasedOnCat;

            if (trailCategory == "All")
            {
                LINQAllWeatherBasedOnCat = (from weather in db.Weathers
                                            join area in db.Areas on weather.Area equals area.AreaName
                                            join location in db.Locations on area.AreaCode equals location.AreaCode
                                            where weather.LastUpdated == currentDateTime
                                            orderby location.LocationId ascending
                                            select weather);
            }
            else
            {
                LINQAllWeatherBasedOnCat = (from weather in db.Weathers
                                            join area in db.Areas on weather.Area equals area.AreaName
                                            join location in db.Locations on area.AreaCode equals location.AreaCode
                                            where weather.LastUpdated == currentDateTime && location.Category == trailCategory
                                            orderby location.LocationId ascending
                                            select weather);
            }

            System.Diagnostics.Debug.WriteLine("GET WEATHER COUNT");
            System.Diagnostics.Debug.WriteLine(LINQAllWeatherBasedOnCat.Count());

            return LINQAllWeatherBasedOnCat;
        }

        public IQueryable<Weather> getLocationWeather(string searchLocation)
        {
            //Attempt get current DateTime without second and minute and millisecond
            DateTime currentDateTime = DateTime.Now;
            currentDateTime = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, currentDateTime.Hour, 0, 0, 0);

            var LINQLocationWeather = (from weather in db.Weathers
                                        join area in db.Areas on weather.Area equals area.AreaName
                                        join location in db.Locations on area.AreaCode equals location.AreaCode
                                        where weather.LastUpdated == currentDateTime && location.Name == searchLocation
                                       orderby location.LocationId ascending
                                        select weather);

            return LINQLocationWeather;
        }
    }
}