using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NosTRAILgic.Models
{
    public class TrailMeetup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TrailMeetupID { get; set; }

        public int CreatorID { get; set; }

        [Required]
        [Display(Name = "Trail Name")]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Trail Cover")]
        public string ImageLink { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Date { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm tt}")]
        [DataType(DataType.Time)]
        public DateTime? TimeFrom { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm tt}")]
        [DataType(DataType.Time)]
        public DateTime? TimeTo { get; set; }

        [Display(Name = "Maximum People")]
        public int Limit { get; set; }
    }
}