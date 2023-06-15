﻿using Microsoft.EntityFrameworkCore;
using TripsServiceDAL.Entities;

namespace TripsServiceDAL.Infrastructure
{
	public class TripsDBContext : DbContext
	{
		public DbSet<Trip> Trips { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Image> Images { get; set; }
		public DbSet<RoutePoint> RoutePoints { get; set; }
		public DbSet<Driver> Drivers { get; set; }
		public DbSet<DriverPhoto> DriverPhotos { get; set; }
		public DbSet<Feedback> Feedbacks { get; set; }

		public TripsDBContext(DbContextOptions options) : base(options)
		{
			_ = Database.EnsureCreated();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			_ = modelBuilder.Entity<Trip>()
				.HasOne(t => t.User)
				.WithMany(u => u.Trips)
				.HasForeignKey(t => t.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			_ = modelBuilder.Entity<RoutePoint>()
				.HasOne(t => t.Trip)
				.WithMany(u => u.RoutePoints)
				.HasForeignKey(t => t.TripId)
				.OnDelete(DeleteBehavior.Cascade);

			_ = modelBuilder.Entity<Comment>()
				.HasOne(c => c.User)
				.WithMany(u => u.Comments)
			.HasForeignKey(c => c.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			_ = modelBuilder.Entity<Comment>()
				.HasOne(c => c.Trip)
				.WithMany(t => t.Comments)
				.HasForeignKey(c => c.TripId)
				.OnDelete(DeleteBehavior.Cascade);

			_ = modelBuilder.Entity<Image>()
				.HasOne(i => i.Trip)
				.WithMany(t => t.Images)
				.HasForeignKey(i => i.TripId)
				.OnDelete(DeleteBehavior.Cascade);

			_ = modelBuilder.Entity<Feedback>()
				.HasOne(f => f.Trip)
				.WithOne(t => t.Feedback)
				.HasForeignKey<Feedback>(f => f.TripId)
				.OnDelete(DeleteBehavior.Cascade);

			_ = modelBuilder.Entity<Trip>()
			   .HasOne(t => t.Driver)
			   .WithMany(u => u.Trips)
			   .HasForeignKey(t => t.DriverId)
			   .OnDelete(DeleteBehavior.SetNull);

			_ = modelBuilder.Entity<DriverPhoto>()
			   .HasOne(dp => dp.Driver)
			   .WithMany(d => d.Photos)
			   .HasForeignKey(dp => dp.DriverId)
			   .OnDelete(DeleteBehavior.Cascade);
		}
	}
}
