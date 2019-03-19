using Geocaching.Models;
using Microsoft.EntityFrameworkCore;

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
                .HasKey(c => new { c.GeocasheID, c.PersonID});
        }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer(SqlConnection.ConnectionString);
            }
        }
}
