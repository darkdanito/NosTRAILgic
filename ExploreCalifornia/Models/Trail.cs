﻿using System.ComponentModel.DataAnnotations;

namespace ExploreCalifornia.Models
{
    public class Trail
    {
        public int Id { get; set; }

        [Required]
        public string Location { get; set; }

        [Display(Name = "Trail Cover")]
        public string TrailCover { get; set; }

        [Required]
        [Display(Name = "Trail Name")]
        public string TrailName { get; set; }

        [Required]
        public string Description { get; set; }

    }
}