using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace ExploreCalifornia.Models
{
    public class Trail
    {
        public int Id { get; set; }

        [Required]
        public string Location { get; set; }


        [Required]
        [Display(Name = "Trail Cover")]
        public string TrailCover { get; set; }

        [Required]
        [Display(Name = "Trail Name")]
        public string TrailName { get; set; }

        [Required]
        public string Description { get; set; }

    }
}