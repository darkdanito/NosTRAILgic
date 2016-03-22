using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NosTRAILgic.Models
{
    /************************************************************************************
     * Description: Model for the Join Trail                                            *
     *                                                                                  *
     * Developer: Yun Yong                                                              *
     *                                                                                  *
     * Date: 12/03/2016                                                                 *
     ************************************************************************************/
    public class JoinTrail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JoinTrailID { get; set; }

        public int TrailMeetupID { get; set; }

        public string UserID { get; set; }
    }
}