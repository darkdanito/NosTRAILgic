using System.ComponentModel.DataAnnotations;

namespace NosTRAILgic.Models
{
    public class Area
    {
        [Key]
        public int AreaCode { get; set; }

        public string AreaName { get; set; }
    }
}