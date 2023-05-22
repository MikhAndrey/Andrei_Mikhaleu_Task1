using Microsoft.EntityFrameworkCore;

namespace Andrei_Mikhaleu_Task1.Models.Entities
{
    public class TripsDBContext : DbContext
    {
        public DbSet<Trip> Trips { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<RoutePoint> RoutePoints { get; set; }

        public TripsDBContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\mssqllocaldb;Initial Catalog=Mikhaleu_Task_1;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trip>()
                .HasOne(t => t.User)
                .WithMany(u => u.Trips)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RoutePoint>()
                .HasOne(t => t.Trip)
                .WithMany(u => u.RoutePoints)
                .HasForeignKey(t => t.TripId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Trip)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TripId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Image>()
                .HasOne(i => i.Trip)
                .WithMany(t => t.Images)
                .HasForeignKey(i => i.TripId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
