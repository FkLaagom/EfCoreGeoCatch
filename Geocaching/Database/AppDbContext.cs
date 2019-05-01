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

            var personEntity = modelBuilder.Entity<Person>();
            personEntity.OwnsOne(
                                o => o.Location,
                                nestedProp =>
                                {
                                    nestedProp.Property(p => p.Latitude).HasColumnName("Latitude").HasColumnType("FLOAT");
                                    nestedProp.Property(p => p.Longitude).HasColumnName("Longitude").HasColumnType("FLOAT");
                                    nestedProp.Ignore(p => p.Altitude);
                                    nestedProp.Ignore(p => p.AltitudeReference);
                                });

            var geoEntity = modelBuilder.Entity<Geocashe>();
            geoEntity.OwnsOne(
                                o => o.Location,
                                nestedProp =>
                                {
                                    nestedProp.Property(p => p.Latitude).HasColumnName("Latitude").HasColumnType("FLOAT");
                                    nestedProp.Property(p => p.Longitude).HasColumnName("Longitude").HasColumnType("FLOAT");
                                    nestedProp.Ignore(p => p.Altitude);
                                    nestedProp.Ignore(p => p.AltitudeReference);
                                });

            modelBuilder.Entity<Geocashe>().HasOne(x => x.Person).WithMany(x => x.Geocashes).OnDelete(DeleteBehavior.Restrict);
               
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["GeocasheDatabase"].ConnectionString);
        }
    }
}
