using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NosTRAILgic.Models
{
    /************************************************************************************
     * Description: Model for TrailMeetup_Location.                                     *
     *                                                                                  *
     * Developer: Yun Yong                                                              *
     *                                                                                  *
     * Date: 12/03/2016                                                                 *
     ************************************************************************************/
    public class TrailMeetup_Location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TrailMeetup_LocationID { get; set; }

        public int TrailMeetupID { get; set; }

        public string LocationName { get; set; }
    }
}