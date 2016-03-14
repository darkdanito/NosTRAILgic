namespace NosTRAILgic.Models
{
    public class Location
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        public int AreaCode { get; set; }
        public int PostalCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}