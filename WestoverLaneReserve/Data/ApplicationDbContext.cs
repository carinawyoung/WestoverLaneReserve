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

        public DbSet<TimeSlotAvailability> TimeSlotAvailabilities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LaneReservation>().ToTable("LaneReservation");
            modelBuilder.Entity<Customer>().ToTable("Customer");

            // Configure composite key for TimeSlotAvailability
            modelBuilder.Entity<TimeSlotAvailability>()
                .HasKey(t => new { t.Date, t.Time });
            // base.OnModelCreating(modelBuilder);

            // // Ensure Date column stores only the date part
            // modelBuilder.Entity<TimeSlotAvailability>()
            //     .Property(t => t.Date)
            //     .HasColumnType("Date");

            // // Ensure Time column stores only the time part (hours and minutes)
            // modelBuilder.Entity<TimeSlotAvailability>()
            //     .Property(t => t.Time)
            //     .HasColumnType("Time");
        }
    }
}