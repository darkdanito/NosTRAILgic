using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Web;
using System.Xml;

namespace NosTRAILgic.Services
{
    public class WeatherForecastGateway : IWeatherForecastGateway
    {
        public void updateNowcast()
        {
            string URL = "http://www.nea.gov.sg/api/WebAPI/?dataset=nowcast&keyref=781CF461BB6606ADC4A6A6217F5F2AD610E9D42F3AA8BF6D";

            //Create the request
            WebRequest request = WebRequest.Create(URL);
            //Define a cache policy for this request only.
            //ensures that the request is not cached.
            HttpRequestCachePolicy noCachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            request.CachePolicy = noCachePolicy;
            request.Method = "GET";
            WebResponse response = request.GetResponse();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(response.GetResponseStream());

            //Parse XML to get relevant details
            //Last Update Information
            string issueDateTime = xmlDoc.GetElementsByTagName("issue_datentime").Item(0).InnerText;
            string[] issueParts = issueDateTime.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            int timeLastUpdate = 0;
            int yearLastUpdate = 0;
            int monthLastUpdate = 0;
            int dayLastUpdate = 0;
            for (int i = 0; i < issueParts.Length; i++)
            {
                //Attempt to capture last updated time
                if (issueParts[i].Equals("at"))
                {
                    string[] timeParts = issueParts[i + 1].Split(':');
                    timeLastUpdate = Int32.Parse(timeParts[0]);
                    //Convert to 24Hours format
                    if(issueParts[i + 2].Equals("PM"))
                    {
                        timeLastUpdate = timeLastUpdate + 12;
                    }
                    else
                    {
                        //If 12AM
                        if(timeLastUpdate == 12)
                        {
                            timeLastUpdate = 0;
                        }
                    }
                }
                //Attempt to capture last updated date
                if (issueParts[i].Equals("on"))
                {
                    string[] dateParts = issueParts[i + 1].Split('-');
                    dayLastUpdate = Int32.Parse(dateParts[0]);
                    monthLastUpdate = Int32.Parse(dateParts[1]);
                    yearLastUpdate = Int32.Parse(dateParts[2]);                    
                }
            }
            // DateTime(Year, Month, Day, Hour, Mintue, Second)
            DateTime lastUpdated = new DateTime(yearLastUpdate, monthLastUpdate,dayLastUpdate, timeLastUpdate, 0, 0);
            System.Diagnostics.Debug.WriteLine(lastUpdated);

            

            XmlNodeList elemList = xmlDoc.GetElementsByTagName("area");
            for (int i = 0; i < elemList.Count; i++)
            {                
                //Area name
                System.Diagnostics.Debug.WriteLine(elemList[i].Attributes["name"].Value);
                //Forecast
                System.Diagnostics.Debug.WriteLine(elemList[i].Attributes["forecast"].Value);
                //Icon
                System.Diagnostics.Debug.WriteLine(elemList[i].Attributes["icon"].Value);
                //Region
                System.Diagnostics.Debug.WriteLine(elemList[i].Attributes["zone"].Value);

            }

            //Test Display
            //System.Diagnostics.Debug.WriteLine(location.Name);
            //System.Diagnostics.Debug.WriteLine(location.Description);
            //System.Diagnostics.Debug.WriteLine(location.HyperLink);
            //System.Diagnostics.Debug.WriteLine(location.ImageLink);
            //System.Diagnostics.Debug.WriteLine(location.AreaCode.ToString());
            //System.Diagnostics.Debug.WriteLine(location.PostalCode.ToString());
            //System.Diagnostics.Debug.WriteLine(location.Latitude.ToString());
            //System.Diagnostics.Debug.WriteLine(location.Longitude.ToString());
            //System.Diagnostics.Debug.WriteLine(location.Category);

        }
    }
}