using Microsoft.EntityFrameworkCore;
using WestoverLaneReserve.Models;

namespace WestoverLaneReserve.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<LaneReservation> LaneReservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LaneReservation>().ToTable("LaneReservation");
            modelBuilder.Entity<Customer>().ToTable("Customer");
            // base.OnModelCreating(modelBuilder);
        }
    }
}