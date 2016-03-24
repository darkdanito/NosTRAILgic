using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NosTRAILgic.Models
{
    /************************************************************************************
     * Description: Model for the Weather                                              *
     *                                                                                  *
     * Developer: Elson                                                                 *
     *                                                                                  *
     * Date: 24/03/2016                                                                 *
     ************************************************************************************/
    public class Weather
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WeatherId { get; set; }

        public string Region { get; set; }
        public string Area { get; set; }
        public string Forecast { get; set; }
        public string Icon { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}