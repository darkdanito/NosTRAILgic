using Microsoft.AspNet.Identity.EntityFramework;
using NosTRAILgic.Models;
using System.Data.Entity;

namespace NosTRAILgic.DAL
{
    public class NosTRAILgicContext : IdentityDbContext<ApplicationUser>
    {
        public NosTRAILgicContext() : base("NosTRAILgicDB")
        {

        }

        public DbSet<TrailMeetup> Trails { get; set; }
        public DbSet<JoinTrail> JoinTrails { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<TrailMeetup_Location> TrailMeetup_Location { get; set; }

        public DbSet<Area> Areas { get; set; }

        public DbSet<Search> Searchs { get; set; }
        public DbSet<CheckIn> CheckIns { get; set; }

        //public DbSet<UploadLocation> UploadLocations { get; set; }
        //public DbSet<Museum> Museums { get; set; }
        //public DbSet<HistoricSite> HistoricSites { get; set; }
        //public DbSet<Monument> Monuments { get; set; }
    }
}