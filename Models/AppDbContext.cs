using Microsoft.EntityFrameworkCore;

namespace Gym.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Trainee> Trainees { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<DietPlan> DietPlans { get; set; }
        public DbSet<WorkoutPlan> WorkoutPlans { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Trainee>()
                .HasOne(t => t.Subscription)
                .WithOne(s => s.Trainee)
                .HasForeignKey<Subscription>(s => s.TraineeId);


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
