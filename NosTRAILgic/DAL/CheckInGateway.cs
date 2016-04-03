using NosTRAILgic.Models;
using System;
using System.Linq;

namespace NosTRAILgic.DAL
{
    /************************************************************************************
     * Description: Gateway for Check In                                                *
     *                                                                                  *
     ************************************************************************************/
    public class CheckInGateway : DataGateway<CheckIn>
    {
        public bool isUserCheckIn(string userName, string locationName)
        {
            DateTime today = DateTime.Today;

            var LINQGetUserLocationCheckIn = 
                from checkIn in db.CheckIns
                where checkIn.UserName == userName && checkIn.Date == today && checkIn.LocationName == locationName
                select checkIn;

            if(LINQGetUserLocationCheckIn.Count() == 0)
            {
                return false;
            }
            return true;
        }
    }
}