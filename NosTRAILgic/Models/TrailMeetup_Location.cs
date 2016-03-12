using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NosTRAILgic.Models
{
    public class TrailMeetup_Location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TrailMeetup_LocationID { get; set; }

        public int TrailMeetupID { get; set; }

        public int LocationID { get; set; }
    }
}