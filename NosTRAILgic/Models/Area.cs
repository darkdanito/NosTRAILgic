using System.ComponentModel.DataAnnotations;

namespace NosTRAILgic.Models
{
    /************************************************************************************
     * Description: This model handle the creation of the Area DB in the database.      *
     *              It store the Area Code that is link the the Area name               *
     *                                                                                  *
     ************************************************************************************/
    public class Area
    {
        [Key]
        public int AreaCode { get; set; }

        public string AreaName { get; set; }
    }
}