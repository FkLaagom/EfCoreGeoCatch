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
            class1 => class1.Locations,
            nestedProp =>
            {
                nestedProp.Property(p => p.Latitude)
                      .HasColumnName("LatitudeHEHE") // here you could add a custom name like I did or remove it and you get a generated one
                        .HasColumnType("FLOAT");
                //nestedProp.Property(p => p.Longitude)
                //      .HasColumnName("LongitudeHEHE");
            });
        }

      

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
             {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["GeocasheDatabase"].ConnectionString);
             }
        }
}
