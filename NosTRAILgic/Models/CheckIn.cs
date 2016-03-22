using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NosTRAILgic.Models
{
    public class CheckIn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CheckInID { get; set; }

        public int LocationID { get; set; }

        public DateTime Date { get; set; }
    }
}