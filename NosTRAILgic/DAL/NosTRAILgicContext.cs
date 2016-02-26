﻿using NosTRAILgic.Models;
using System.Data.Entity;

namespace NosTRAILgic.DAL
{
    public class NosTRAILgicContext : DbContext
    {
        public NosTRAILgicContext() : base("NosTRAILgicDB")
        {

        }

        public DbSet<Trail> Trails { get; set; }
    }
}