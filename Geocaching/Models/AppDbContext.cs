using Geocaching.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Geocaching
{
    public class AppDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Geocashe> Geocashes { get; set; }
        public DbSet<FoundGeocache> FoundGeocaches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoundGeocache>()
                .HasKey(c => new { c.GeocasheID, c.PersonID });

            var clas1Entity = modelBuilder.Entity<Person>();
            clas1Entity.OwnsOne(
                                o => o.Locations,
                                nestedProp =>
                                {
                                    nestedProp.Property(p => p.Latitude).HasColumnName("Latitude").HasColumnType("FLOAT");
                                    nestedProp.Property(p => p.Longitude).HasColumnName("Longitude").HasColumnType("FLOAT");
                                    nestedProp.Ignore(p => p.Altitude);
                                    nestedProp.Ignore(p => p.AltitudeReference);
                                });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["GeocasheDatabase"].ConnectionString);
        }
    }
}
