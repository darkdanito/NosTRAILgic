﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NosTRAILgic.Models
{
    public class Area
    {
        [Key]
        public int AreaCode { get; set; }

        public string AreaName { get; set; }
    }
}