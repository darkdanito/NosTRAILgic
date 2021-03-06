﻿using NosTRAILgic.Models;
using NosTRAILgic.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NosTRAILgic.DAL
{
    /************************************************************************************
     * Description: This the data mapper for TrailMeetup                                *
     *                                                                                  *
     ************************************************************************************/
    public class TrailMeetupMapper : DataGateway<TrailMeetup>
    {
        /************************************************************************************
         * Description: This function take in the Trail ID and return the participants      *
         *              of the TrailMeetup                                                  *
         *                                                                                  *
         * Developer: Yun Yong                                                              *
         *                                                                                  *
         * Date: 13/03/2016                                                                 *
         ************************************************************************************/
        public List<string> getTrailParticipants(int trailID)
        {
            LogWriter.Instance.LogInfo("TrailMeetupMapper / getTrailParticipants");
            var trailParticipants = (from p in db.JoinTrails
                                     where p.TrailMeetupID == trailID
                                     select p.UserID).ToList();

            return trailParticipants;
        }

        /************************************************************************************
         * Description: This function take in the keyword that the user input               *
         *              and return a auto compelete list based on what the user             *
         *              has inputted                                                        *
         *                                                                                  *
         * Developer: Yun Yong                                                              *
         *                                                                                  *
         * Date: 13/03/2016                                                                 *
         ************************************************************************************/
        public IQueryable<string> getSearchAutoComplete(string term)
        {
            var result = from r in db.Locations
                         where r.Name.ToLower().StartsWith(term)
                         select r.Name;

            return result;
        }

        /************************************************************************************
         * Description: This function will get the Date.Time.Now and compare to the         *
         *              list of TrailMeetup in the DB and only return TrailMeetup that is   *
         *              occur later then the cucrrent datetime                              *
         *                                                                                  *
         * Developer: Elson & Yun Yon                                                       *
         *                                                                                  *
         * Date: 13/03/2016                                                                 *
         ************************************************************************************/
        public IQueryable<TrailMeetup> getTrailsByDate()
        {
            LogWriter.Instance.LogInfo("TrailMeetupMapper / getTrailsByDate");
            var trailMeetupByDate = from i in db.Trails
                                    where i.TimeFrom.CompareTo(DateTime.Now) >= 0
                                    select i;

            return trailMeetupByDate;
        }

        /************************************************************************************
         * Description: This function take in the TrailMeetupID and Username                *
         *              and check to see is the User in the Trail or not                    *
         *                                                                                  *
         * Developer: Yun Yong                                                              *
         *                                                                                  *
         * Date: 13/03/2016                                                                 *
         ************************************************************************************/
        public string isUserInTrail(int trailID, string userName)
        {
            LogWriter.Instance.LogInfo("TrailMeetupMapper / isUserInTrail");
            string isUserNameInTrail = (from p in db.JoinTrails
                                        where p.UserID == userName 
                                        && p.TrailMeetupID == trailID
                                        select p.UserID).FirstOrDefault();

            return isUserNameInTrail;
        }

        /************************************************************************************
         * Description: This function take in the name of the new TrailMeetup               *
         *              the user created and return the TrailMeetup ID                      *
         *                                                                                  *
         * Developer: Yun Yong                                                              *
         *                                                                                  *
         * Date: 13/03/2016                                                                 *
         ************************************************************************************/
        public int getNewlyCreatedTrailID(string newTrailName)
        {
            LogWriter.Instance.LogInfo("TrailMeetupMapper / getNewlyCreatedTrailID");
            int newlyCreatedTrailID = (from c in db.Trails
                                       where c.Name == newTrailName
                                       select c.TrailMeetupID).FirstOrDefault();

            return newlyCreatedTrailID;
        }

        /************************************************************************************
         * Description: This function take in the name of the location and return the       *
         *              location ID for insertion into TrailMeetup_Location DB              *
         *                                                                                  *
         * Developer: Yun Yong                                                              *
         *                                                                                  *
         * Date: 13/03/2016                                                                 *
         ************************************************************************************/
        public int getLocationID(string locationName)
        {
            LogWriter.Instance.LogInfo("TrailMeetupMapper / getLocationID");
            int getLocationID = (from a in db.Locations
                                 where a.Name == locationName
                                 select a.LocationId).FirstOrDefault();

            return getLocationID;
        }

        /************************************************************************************
         * Description: This function will take in the TrailMeetup ID and return            *
         *              Location table that the TrailMeetup contains by joining the DB      *
         *              TrailMeetup_Location and TrailMeetup_Location with TrailMeetup      *
         *                                                                                  *
         * Developer: Yun Yong                                                              *
         *                                                                                  *
         * Date: 13/03/2016                                                                 *
         ************************************************************************************/
        public IQueryable<Location> getAllLocationInfoFromTrail(int trailID)
        {
            LogWriter.Instance.LogInfo("TrailMeetupMapper / getAllLocationInfoFromTrail");
            var LINQAllLocationQuery = (from y in db.Trails
                                        join x in db.TrailMeetup_Location on y.TrailMeetupID equals x.TrailMeetupID
                                        join w in db.Locations on x.LocationName equals w.Name
                                        where y.TrailMeetupID == trailID
                                        select w);

            return LINQAllLocationQuery;
        }

        /************************************************************************************
         * Description: This function will take in the TrailMeetup ID and return            *
         *              Location table that the TrailMeetup contains by joining the DB      *
         *              TrailMeetup_Location and TrailMeetup_Location with TrailMeetup      *
         *                                                                                  *
         * Developer: Elson                                                             *
         *                                                                                  *
         * Date: 01/04/2016                                                                 *
         ************************************************************************************/
        public IQueryable<Weather> getAllWeatherInfoFromTrail(int trailID, bool olderData)
        {
            LogWriter.Instance.LogInfo("TrailMeetupMapper / getAllWeatherInfoFromTrail");
            // Attempt get current DateTime without second and minute and millisecond
            DateTime currentDateTime = DateTime.Now;
            if (olderData)
            {
                currentDateTime = (from weather in db.Weathers
                                   orderby weather.LastUpdated descending
                                   select weather.LastUpdated).FirstOrDefault();
            }
            currentDateTime = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, currentDateTime.Hour, 0, 0, 0);

            var LINQAllWeatherBasedOnTrail = (from trail in db.Trails
                                        join meetupLocation in db.TrailMeetup_Location on trail.TrailMeetupID equals meetupLocation.TrailMeetupID
                                        join location in db.Locations on meetupLocation.LocationName equals location.Name
                                        join area in db.Areas on location.AreaCode equals area.AreaCode
                                        join weather in db.Weathers on area.AreaName equals weather.Area
                                        where trail.TrailMeetupID == trailID && weather.LastUpdated == currentDateTime
                                        select weather);

            System.Diagnostics.Debug.WriteLine("GET WEATHER COUNT");
            System.Diagnostics.Debug.WriteLine(LINQAllWeatherBasedOnTrail.Count());

            return LINQAllWeatherBasedOnTrail;
        }

        /************************************************************************************
         * Description: This function will take in the trail ID and return the trails       *
         *              that the user has created                                           *
         *                                                                                  *
         * Developer: Yun Yong                                                              *
         *                                                                                  *
         * Date: 13/03/2016                                                                 *
         ************************************************************************************/
        public List<int> getTrailUserCreated (int trailID)
        {
            LogWriter.Instance.LogInfo("TrailMeetupMapper / getTrailUserCreated");
            var trailUserCreated = (from r in db.JoinTrails
                                    where r.TrailMeetupID == trailID
                                    select r.JoinTrailID).ToList<int>();

            return trailUserCreated;
        }

        /************************************************************************************
         * Description: This function will take in the trail ID and return the trails       *
         *              that the user has joined                                            *
         *                                                                                  *
         * Developer: Yun Yong                                                              *
         *                                                                                  *
         * Date: 13/03/2016                                                                 *
         ************************************************************************************/
        public List<int> getTrailUserJoined(int trailID)
        {
            LogWriter.Instance.LogInfo("TrailMeetupMapper / getTrailUserJoined");
            var trailUserCreated = (from d in db.TrailMeetup_Location
                                    where d.TrailMeetupID == trailID
                                    select d.TrailMeetup_LocationID).ToList<int>();

            return trailUserCreated;
        }

        /************************************************************************************
         * Description: This function handles the count of the number of participants       *
         *              that join the TrailMeetup                                           *
         *                                                                                  *
         * Developer: Khaleef                                                               *
         ************************************************************************************/
        public int getTrailMeetupParticipantsCount(int trailID)
        {
            LogWriter.Instance.LogInfo("TrailMeetupMapper / getTrailMeetupParticipantsCount");
            int LINQTrailMeetupParticipantsCount = (from c in db.JoinTrails
                                                    where c.TrailMeetupID == trailID
                                                    select c).Count();

            return LINQTrailMeetupParticipantsCount;
        }

        /************************************************************************************
         * Description: This function handles the count of the number of participants       *
         *              that join the TrailMeetup                                           *
         *                                                                                  *
         * Developer: Khaleef                                                               *
         ************************************************************************************/
        public int getUserJoinTrailForDelete (int trailID, string userName)
        {
            LogWriter.Instance.LogInfo("TrailMeetupMapper / getUserJoinTrailForDelete");
            var LINQGetUserJoinTrail = (from s in db.JoinTrails
                                       where s.UserID == userName
                                       && s.TrailMeetupID == trailID
                                       select s.JoinTrailID).FirstOrDefault();

            return LINQGetUserJoinTrail;
        }
    }
}