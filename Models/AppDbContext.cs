﻿using Microsoft.EntityFrameworkCore;

namespace Gym.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Email = "admin@gym.com",
                    Password = "admin123",
                    Role = "Admin"
                },
                new User
                {
                    Id = 2,
                    Username = "trainer",
                    Email = "trainer@gym.com",
                    Password = "trainer123",
                    Role = "Trainer"
                },
                new User
                {
                    Id = 3,
                    Username = "trainee",
                    Email = "trainee@gym.com",
                    Password = "trainee123",
                    Role = "Trainee"
                }
            );
        }
    }
}
