using NosTRAILgic.Models;
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
        private NosTRAILgicContext db = new NosTRAILgicContext();

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
         * Description: This function take in the TrailMeetupID and Username                *
         *              and check to see is the User in the Trail or not                    *
         *                                                                                  *
         * Developer: Yun Yong                                                              *
         *                                                                                  *
         * Date: 13/03/2016                                                                 *
         ************************************************************************************/
        public string isUserInTrail(int trailID, string userName)
        {
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
            var LINQAllLocationQuery = (from y in db.Trails
                                        join x in db.TrailMeetup_Location on y.TrailMeetupID equals x.TrailMeetupID
                                        join w in db.Locations on x.LocationID equals w.LocationId
                                        where y.TrailMeetupID == trailID
                                        select w);

            return LINQAllLocationQuery;
        }

        /************************************************************************************
         * Description: This function handles the count of the number of participants       *
         *              that join the TrailMeetup                                           *
         *                                                                                  *
         * Developer: Khaleef                                                               *
         ************************************************************************************/
        public int getTrailMeetupParticipantsCount(int trailID)
        {
            int LINQTrailMeetupParticipantsCount = (from c in db.JoinTrails
                                                    where c.TrailMeetupID == trailID
                                                    select c).Count();

            return LINQTrailMeetupParticipantsCount;
        }

    }
}