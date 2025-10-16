using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Gym.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }


        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=GymDb;Trusted_Connection=True;Trust Server Certificate=True");
        }
    }
}
