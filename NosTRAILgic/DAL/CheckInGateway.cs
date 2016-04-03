using NosTRAILgic.Models;
using NosTRAILgic.Libraries;
using System;
using System.Linq;

namespace NosTRAILgic.DAL
{
    /************************************************************************************
     * Description: Gateway for Check In                                                *
     *                                                                                  *
     * Developer: Elson                                                                 *
     *                                                                                  *
     * Date: 02/04/2016                                                                 *
     ************************************************************************************/
    public class CheckInGateway : DataGateway<CheckIn>
    {
        //Check whether user had checkin today at at specific location
        public bool isUserCheckIn(string userName, string locationName)
        {
            LogWriter.Instance.LogInfo("CheckInGateway / isUserCheckIn");
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