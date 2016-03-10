using NosTRAILgic.Models;
using System.Data.Entity;

namespace NosTRAILgic.DAL
{
    public class NosTRAILgicContext : DbContext
    {
        public NosTRAILgicContext() : base("NosTRAILgicDB")
        {

        }

        public DbSet<TrailMeetup> Trails { get; set; }
        //public DbSet<Location> Locations { get; set; }
        //public DbSet<Museum> Museums { get; set; }
        //public DbSet<HistoricSite> HistoricSites { get; set; }
        //public DbSet<Monument> Monuments { get; set; }
    }
}