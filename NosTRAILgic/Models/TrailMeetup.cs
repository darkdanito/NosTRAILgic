using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Foolproof;

namespace NosTRAILgic.Models
{
    /************************************************************************************
     * Description: Model for TrailMeetup                                               *
     *                                                                                  *
     * Developer: Yun Yong                                                              *
     *                                                                                  *
     * Date: 12/03/2016                                                                 *
     ************************************************************************************/
    public class TrailMeetup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TrailMeetupID { get; set; }

        public string CreatorID { get; set; }

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
        [Display(Name = "Start Time")]
        public DateTime? TimeFrom { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm tt}")]
        [DataType(DataType.Time)]
        [GreaterThan("TimeFrom")]
        [Display(Name = "End Time")]
        public DateTime? TimeTo { get; set; }

        [Display(Name = "Maximum People")]
        public int Limit { get; set; }
    }
}