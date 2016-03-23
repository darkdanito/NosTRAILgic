﻿using NosTRAILgic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NosTRAILgic.DAL
{
    /************************************************************************************
     * Description: This function                                                       *
     *                                                                                  *
     * Developer: Yun Yong                                                              *
     *                                                                                  *
     * Date: 13/03/2016                                                                 *
     ************************************************************************************/
    public class TrailMeetupMapper
    {
        private NosTRAILgicContext db = new NosTRAILgicContext();

        /************************************************************************************
         * Description: This function                                                       *
         *                                                                                  *
         ************************************************************************************/
        public List<string> getTrailParticipants(int trailID)
        {
            var trailParticipants = (from p in db.JoinTrails where p.TrailMeetupID == trailID select p.UserID).ToList();

            return trailParticipants;
        }

        /************************************************************************************
         * Description: This function                                                       *
         *                                                                                  *
         ************************************************************************************/
        public IQueryable<string> getSearchAutoComplete(string term)
        {
            var result = from r in db.Locations
                         where r.Name.ToLower().StartsWith(term)
                         select r.Name;

            return result;
        }

        /************************************************************************************
         * Description: This function                                                       *
         *                                                                                  *
         ************************************************************************************/
        public IQueryable<Location> getAllLocationInfoFromTrail(int trailID)
        {
            var LINQAllLocationQuery = (from y in db.Trails
                                        join x in db.TrailMeetup_Location on y.TrailMeetupID equals x.TrailMeetupID
                                        join w in db.Locations on x.LocationID equals w.LocationId
                                        where y.TrailMeetupID == trailID
                                        select w);

            return LINQAllLocationQuery;
        }
    }
}