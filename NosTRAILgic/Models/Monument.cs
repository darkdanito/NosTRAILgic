using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NosTRAILgic.Models
{
    public class Monument
    {
        public int LocationId { get; set; }
        public string Description { get; set; }
        public string ImageLink { get; set; }
    }
}