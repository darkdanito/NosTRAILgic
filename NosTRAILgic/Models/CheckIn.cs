﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NosTRAILgic.Models
{
    /************************************************************************************
     * Description: This handle the storing of the information related to CheckIn       *
     *                                                                                  *
     ************************************************************************************/
    public class CheckIn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CheckInID { get; set; }

        public string LocationName { get; set; }

        public string UserName { get; set; }

        public DateTime Date { get; set; }
    }
}