using NosTRAILgic.Models;
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
         * Description: This function XXXXXXX [Elson do remember to fill in]                *
         *                                                                                  *
         * Developer: Elson                                                                 *
         *                                                                                  *
         * Date: 25/03/2016                                                                 *
         ************************************************************************************/
        public void removeDuplicateWeatherData()
        {
            // Attempt get current DateTime without second and minute and millisecond
            DateTime currentDateTime = (from weather in db.Weathers
                                        orderby weather.LastUpdated descending
                                        select weather.LastUpdated).FirstOrDefault();
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