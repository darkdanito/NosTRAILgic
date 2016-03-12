using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NosTRAILgic.Models
{
    public class JoinTrail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JoinTrailID { get; set; }

        public int TrailMeetupID { get; set; }

        public string UserID { get; set; }
    }
}