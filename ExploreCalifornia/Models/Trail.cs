using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace ExploreCalifornia.Models
{
    public class Trail
    {
        public int Id { get; set; }

        public string Location { get; set; }

        public string TrailCover { get; set; }

        public string TrailName { get; set; }

        public string Description { get; set; }

    }
}