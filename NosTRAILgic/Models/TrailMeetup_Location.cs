using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NosTRAILgic.Models
{
    public class TrailMeetup_Location
    {
        [Key]
        public int TrailMeetupID { get; set; }

        public int LocationID { get; set; }
    }
}