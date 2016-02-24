using ExploreCalifornia.Models;
using System.Data.Entity;

namespace ExploreCalifornia.DAL
{
    public class NosTRAILgicContext : DbContext
    {
        public NosTRAILgicContext() : base("ExploreCaliforniaDB")
        {

        }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Trail> Trails { get; set; }
    }
}