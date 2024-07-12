using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WestoverLaneReserve.Models;

namespace WestoverLaneReserve.Data
{
    public class ApplicationDbContext : IdentityDbContext<CustomerApplicationUser>   // Application is my custom user class now
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<LaneReservation> LaneReservations { get; set; }
        public DbSet<TimeSlotAvailability> TimeSlotAvailabilities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);  // Important to configure Identity models correctly
            modelBuilder.Entity<LaneReservation>().ToTable("LaneReservation");

            // Configure composite key for TimeSlotAvailability
            modelBuilder.Entity<TimeSlotAvailability>()
                .HasKey(t => new { t.Date, t.Time });
        }
    }
}