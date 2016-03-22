using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NosTRAILgic.Models
{
    /************************************************************************************
     * Description: Model for the Location                                              *
     *                                                                                  *
     * Developer: Elson                                                                 *
     *                                                                                  *
     * Date: 12/03/2016                                                                 *
     ************************************************************************************/
    public class Location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocationId { get; set; }

        public string Name { get; set; }
        public int AreaCode { get; set; }
        public int PostalCode { get; set; }
        public string Description { get; set; }
        public string HyperLink { get; set; }
        public string ImageLink { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Category { get; set; }
    }
}