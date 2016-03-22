using System.Collections.Generic;

namespace NosTRAILgic.Models
{
    /************************************************************************************
     * Description: This ViewModel manages the handling of display of                   *
     *              TrailMeetup/View/Details_ViewModel                                  *
     *                                                                                  *
     * Developer: Yun Yong                                                              *
     *                                                                                  *
     * Date: 21/03/2016                                                                 *
     ************************************************************************************/
    public class TrailMeetup_Details_ViewModel
    {
        public TrailMeetup getTrailMeetup { get; set; }
        public JoinTrail getJoinTrail { get; set; }
        public TrailMeetup_Location getTrailMeetup_Location { get; set; }
        public Location getLocation { get; set; }

        public IEnumerable<string> enumerableTrailParticipants { get; set; }        // Enumerable that hold all the participants for 
                                                                                    // the trails

        public IEnumerable<Location> enumerableAllLocationFromTrail { get; set; }   // Enumerable that hold all the Location informationn
                                                                                    // for the trail
    }
}