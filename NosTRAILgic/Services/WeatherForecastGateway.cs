using System;
using System.Net;
using System.Net.Cache;
using System.Xml;
using NosTRAILgic.Models;
using NosTRAILgic.Libraries;
using NosTRAILgic.DAL;

namespace NosTRAILgic.Services
{
    /************************************************************************************
     * Description: This weatherforecastgateway uses the REST API to get weather from   *
     *              NEA website and insert into database                                *
     *                                                                                  *
     * Developer: Elson                                                                 *
     *                                                                                  *
     * Date: 24/03/2016                                                                 *
     ************************************************************************************/
    public class WeatherForecastGateway : IWeatherForecastGateway
    {
        NosTRAILgicContext db = new NosTRAILgicContext();

        public void GetNowcast()
        {
            LogWriter.Instance.LogInfo("WeatherForecastGateway / GetNowcast");

            string URL = "http://www.nea.gov.sg/api/WebAPI/?dataset=nowcast&keyref=781CF461BB6606ADC4A6A6217F5F2AD610E9D42F3AA8BF6D";

            // Create the request
            WebRequest request = WebRequest.Create(URL);

            // Define a cache policy for this request only. Ensures that the request is not cached.
            HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            request.CachePolicy = noCachePolicy;
            request.Method = "GET";
            WebResponse response = request.GetResponse();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(response.GetResponseStream());

            // Parse XML to get relevant details
            // Last Update Information
            string issueDateTime = xmlDoc.GetElementsByTagName("issue_datentime").Item(0).InnerText;
            string[] issueParts = issueDateTime.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            int timeLastUpdate = 0;
            int yearLastUpdate = 0;
            int monthLastUpdate = 0;
            int dayLastUpdate = 0;
            for (int i = 0; i < issueParts.Length; i++)
            {
                // Attempt to capture last updated time
                if (issueParts[i].Equals("at"))
                {
                    string[] timeParts = issueParts[i + 1].Split(':');
                    timeLastUpdate = Int32.Parse(timeParts[0]);
                    //Convert to 24Hours format
                    if (timeLastUpdate == 12)
                        timeLastUpdate = 0;
                    if (issueParts[i + 2].Equals("PM"))
                    {
                        timeLastUpdate = timeLastUpdate + 12;

                    }
                }
                // Attempt to capture last updated date
                if (issueParts[i].Equals("on"))
                {
                    string[] dateParts = issueParts[i + 1].Split('-');
                    dayLastUpdate = Int32.Parse(dateParts[0]);
                    monthLastUpdate = Int32.Parse(dateParts[1]);
                    yearLastUpdate = Int32.Parse(dateParts[2]);                    
                }
            }

            Console.WriteLine("");
            // DateTime(Year, Month, Day, Hour, Mintue, Second, Millisecond)
            DateTime lastUpdated = new DateTime(yearLastUpdate, monthLastUpdate,dayLastUpdate, timeLastUpdate, 0, 0, 0);
            
            XmlNodeList areaList = xmlDoc.GetElementsByTagName("area");
            for (int i = 0; i < areaList.Count; i++)
            {
                Weather weather = new Weather();

                weather.Area = areaList[i].Attributes["name"].Value;
                weather.Forecast = areaList[i].Attributes["forecast"].Value;
                weather.Icon = "/images/" + areaList[i].Attributes["icon"].Value + ".png";
                weather.Region = areaList[i].Attributes["zone"].Value;
                weather.LastUpdated = lastUpdated;

                // Add to Database
                db.Weathers.Add(weather);
                db.SaveChanges();
            }       
        }
    }
}