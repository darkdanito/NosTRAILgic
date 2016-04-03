using NosTRAILgic.Models;
using NosTRAILgic.Libraries;
using System;
using System.Linq;

namespace NosTRAILgic.DAL
{
    /************************************************************************************
     * Description: Gateway for Weather                                                 *
     *                                                                                  *
     ************************************************************************************/
    public class WeatherGateway : DataGateway<Weather>
    {
        /************************************************************************************
         * Description: This function remove duplicate record of weather in database        *
         *              as in a event not retrieve the update information may cause         *              
         *              duplicate copy                                                      *
         *                                                                                  *
         * Developer: Elson                                                                 *
         *                                                                                  *
         * Date: 25/03/2016                                                                 *
         ************************************************************************************/
        public void removeDuplicateWeatherData()
        {
            LogWriter.Instance.LogInfo("WeatherGateway / removeDuplicateWeatherData");

            // Attempt get current DateTime without second and minute and millisecond
            DateTime currentDateTime = (from weather in db.Weathers
                                        orderby weather.LastUpdated descending
                                        select weather.LastUpdated).FirstOrDefault();
            currentDateTime = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, currentDateTime.Hour, 0, 0, 0);

            var LINQLocationWeather = (from weather in db.Weathers
                                       where weather.LastUpdated == currentDateTime
                                       select weather);

            LogWriter.Instance.LogInfo("WeatherGateway / " + LINQLocationWeather.Count());

            foreach (var weather in LINQLocationWeather)
            {
                db.Weathers.Remove(weather);
            }
            db.SaveChanges();
        }
    }
}