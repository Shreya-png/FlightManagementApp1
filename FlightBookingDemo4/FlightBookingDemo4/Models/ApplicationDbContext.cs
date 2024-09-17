using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace FlightBookingDemo4.Models
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<Users> Users { get; set; } = default!;
        public DbSet<Booking> Bookings { get; set; } = default!;
        public DbSet<Checkin> Checkins { get; set; } = default!;

        public DbSet<Payment> Payments { get; set; } = default!;
        //public DbSet<Payment> Payments { get; set; }

        public DbSet<Passenger> Passengers { get; set; }
        
        // Configure model relationships and other settings
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var hasher = new PasswordHasher<IdentityUser>();

            builder.Entity<Payment>()
            .Property(p => p.PaymentId)
            .ValueGeneratedOnAdd();  // Configure PaymentId as identity (auto-increment)

            builder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithMany(b => b.Payments)
                .HasForeignKey(p => p.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

                

            //// Seed Admin User
            //builder.Entity<IdentityUser>().HasData(
            //    new IdentityUser
            //    {
            //        Id = "a1c6a71f-743e-4a2a-b317-8d62b1b4b66f", // Ensure this GUID is unique
            //        UserName = "Olly123",
            //        Email = "olly@gmail.com",
            //        PasswordHash = hasher.HashPassword(null, "Olly@123") // Ensure this password meets your requirements
            //    }
            //);

            //// Seed Admin Role
            //builder.Entity<IdentityRole>().HasData(
            //    new IdentityRole
            //    {
            //        Id = "9d3f6b68-49eb-40fc-a5a2-947bde9d3e22", // Ensure this GUID is unique
            //        Name = "Admin",
            //        NormalizedName = "ADMIN"
            //    }
            //);

            //// Seed Admin Role Assignment
            //builder.Entity<IdentityUserRole<string>>().HasData(
            //    new IdentityUserRole<string>
            //    {
            //        UserId = "a1c6a71f-743e-4a2a-b317-8d62b1b4b66f", // Must match the UserId seeded above
            //        RoleId = "9d3f6b68-49eb-40fc-a5a2-947bde9d3e22"  // Must match the RoleId seeded above
            //    }
            //);




        }
    }
}
