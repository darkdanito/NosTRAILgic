using NosTRAILgic.Models;
using System;
using System.Linq;

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

        /************************************************************************************
         * Description: This function will take what the user has key into the search box   *
         *              and pass the location results back to the model to display          *
         *                                                                                  *
         ************************************************************************************/
        public IQueryable<Location> getLocationInfo(string searchLocation) {

            var LINQAllLocationBasedOnCat = from a in db.Locations
                                            where a.Name == searchLocation
                                            select a;

            return LINQAllLocationBasedOnCat;
        }


        /************************************************************************************
         * Description: This function will take in the user's account name and              *
         *              return all the TrailMeetup that he has joined                       *
         *                                                                                  *
         ************************************************************************************/
        public IQueryable<TrailMeetup> getJoinedTrails(string userName)
        {
            var LINQGetAllJoinedTrails = from j in db.JoinTrails
                                         join k in db.Trails on j.TrailMeetupID equals k.TrailMeetupID
                                            where j.UserID == userName
                                            select k;

            return LINQGetAllJoinedTrails;
        }

        /************************************************************************************
         * Description: This function will take in the user's account name and              *
         *              return all the TrailMeetup that he has created                      *
         *                                                                                  *
         ************************************************************************************/
        public IQueryable<TrailMeetup> getCreatedTrails(string userName)
        {
            var LINQGetAllCreatedTrails = from c in db.Trails
                                          where c.CreatorID == userName
                                          select c;

            return LINQGetAllCreatedTrails;
        }

        /************************************************************************************
         * Description: This function XXXXXXX [Elson do remember to fill in]                *
         *                                                                                  *
         * Developer: Elson                                                                 *
         *                                                                                  *
         * Date: 25/03/2016                                                                 *
         ************************************************************************************/
        public IQueryable<Weather> getAllLocationWeather(string trailCategory, Boolean olderData)
        {
            //Attempt get current DateTime without second and minute and millisecond
            DateTime currentDateTime = DateTime.Now;
            if (olderData)
            {
                currentDateTime = currentDateTime.AddHours(-1); // -1 hours
            }
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

        /************************************************************************************
         * Description: This function XXXXXXX [Elson do remember to fill in]                *
         *                                                                                  *
         * Developer: Elson                                                                 *
         *                                                                                  *
         * Date: 25/03/2016                                                                 *
         ************************************************************************************/
        public IQueryable<Weather> getLocationWeather(string searchLocation, Boolean olderData)
        {
            //Attempt get current DateTime without second and minute and millisecond
            DateTime currentDateTime = DateTime.Now;
            if (olderData)
            {
                currentDateTime = currentDateTime.AddHours(-1); // -1 hours
            }
            currentDateTime = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, currentDateTime.Hour, 0, 0, 0);

            var LINQLocationWeather = (from weather in db.Weathers
                                        join area in db.Areas on weather.Area equals area.AreaName
                                        join location in db.Locations on area.AreaCode equals location.AreaCode
                                        where weather.LastUpdated == currentDateTime && location.Name == searchLocation
                                       orderby location.LocationId ascending
                                        select weather);

            return LINQLocationWeather;
        }

        /************************************************************************************
         * Description: This function XXXXXXX [Elson do remember to fill in]                *
         *                                                                                  *
         * Developer: Elson                                                                 *
         *                                                                                  *
         * Date: 25/03/2016                                                                 *
         ************************************************************************************/
        public void removeDuplicateWeatherData()
        {
            //Attempt get current DateTime without second and minute and millisecond
            DateTime currentDateTime = DateTime.Now;
            currentDateTime = currentDateTime.AddHours(-1); // -1 hours
            currentDateTime = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, currentDateTime.Hour, 0, 0, 0);

            var LINQLocationWeather = (from weather in db.Weathers
                                       where weather.LastUpdated == currentDateTime
                                       select weather);

            System.Diagnostics.Debug.WriteLine("DELETE DUPLICATE DATA");
            System.Diagnostics.Debug.WriteLine(LINQLocationWeather.Count());

            foreach (var weather in LINQLocationWeather)
            {
                db.Weathers.Remove(weather);                
            }
            db.SaveChanges();
        }
    }
}