﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TripsServiceDAL.Entities;
using TripsServiceDAL.Interfaces;

namespace TripsServiceDAL.Infrastructure;

public class TripsDBContext : DbContext
{
	public TripsDBContext(DbContextOptions options) : base(options)
	{
		Database.EnsureCreated();
	}

	public DbSet<Trip> Trips { get; set; }
	public DbSet<User> Users { get; set; }
	public DbSet<Comment> Comments { get; set; }
	public DbSet<Image> Images { get; set; }
	public DbSet<RoutePoint> RoutePoints { get; set; }
	public DbSet<Driver> Drivers { get; set; }
	public DbSet<DriverPhoto> DriverPhotos { get; set; }
	public DbSet<Feedback> Feedbacks { get; set; }
	public DbSet<Role> Roles { get; set; }
	public DbSet<Chat> Chats { get; set; }
	public DbSet<ChatParticipation> ChatParticipations { get; set; }
	public DbSet<ChatMessage> ChatMessages { get; set; }

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		HandleSoftDelete();
		return await base.SaveChangesAsync(cancellationToken);
	}

	private void HandleSoftDelete()
	{
		IEnumerable<EntityEntry> entities = ChangeTracker.Entries();
		foreach (EntityEntry entity in entities)
		{
			if (entity.Entity is ISoftDelete itemToDelete && entity.State == EntityState.Deleted)
			{
				entity.State = EntityState.Modified;
				itemToDelete.IsDeleted = true;
			}
		}
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Trip>()
			.HasOne(t => t.User)
			.WithMany(u => u.Trips)
			.HasForeignKey(t => t.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<RoutePoint>()
			.HasOne(t => t.Trip)
			.WithMany(u => u.RoutePoints)
			.HasForeignKey(t => t.TripId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Comment>()
			.HasOne(c => c.User)
			.WithMany(u => u.Comments)
			.HasForeignKey(c => c.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Comment>()
			.HasOne(c => c.Trip)
			.WithMany(t => t.Comments)
			.HasForeignKey(c => c.TripId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Image>()
			.HasOne(i => i.Trip)
			.WithMany(t => t.Images)
			.HasForeignKey(i => i.TripId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Feedback>()
			.HasOne(f => f.Trip)
			.WithOne(t => t.Feedback)
			.HasForeignKey<Feedback>(f => f.TripId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Trip>()
			.HasOne(t => t.Driver)
			.WithMany(u => u.Trips)
			.HasForeignKey(t => t.DriverId)
			.OnDelete(DeleteBehavior.SetNull);

		modelBuilder.Entity<DriverPhoto>()
			.HasOne(dp => dp.Driver)
			.WithMany(d => d.Images)
			.HasForeignKey(dp => dp.DriverId)
			.OnDelete(DeleteBehavior.Restrict);
		
		modelBuilder.Entity<User>()
			.HasOne(u => u.Role)
			.WithMany(r => r.Users)
			.OnDelete(DeleteBehavior.Restrict);
		modelBuilder.Entity<ChatParticipation>()
			.HasOne(chp => chp.Chat)
			.WithMany(ch => ch.ChatParticipations)
			.OnDelete(DeleteBehavior.Restrict);
		modelBuilder.Entity<ChatMessage>()
			.HasOne(chm => chm.ChatParticipation)
			.WithMany(chp => chp.ChatMessages)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<Comment>().HasQueryFilter(item => !item.IsDeleted);
		modelBuilder.Entity<Driver>().HasQueryFilter(item => !item.IsDeleted);
		modelBuilder.Entity<DriverPhoto>().HasQueryFilter(item => !item.IsDeleted);
		modelBuilder.Entity<Image>().HasQueryFilter(item => !item.IsDeleted);
		modelBuilder.Entity<RoutePoint>().HasQueryFilter(item => !item.IsDeleted);
		modelBuilder.Entity<Trip>().HasQueryFilter(item => !item.IsDeleted);
		modelBuilder.Entity<User>().HasQueryFilter(item => !item.IsDeleted);
		modelBuilder.Entity<Chat>().HasQueryFilter(item => !item.IsDeleted);
		modelBuilder.Entity<ChatParticipation>().HasQueryFilter(item => !item.IsDeleted);
		modelBuilder.Entity<ChatMessage>().HasQueryFilter(item => !item.IsDeleted);
	}
}
