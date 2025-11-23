using System.ComponentModel.DataAnnotations;

namespace Gym.Models
{
    public class Trainee
    {
        [Key]
        public int Id { get; set; }

        public int ? UserId { get; set; }  // FK to AspNetUsers

        [Required(ErrorMessage = "Please select your goal")]
        [StringLength(50, ErrorMessage = "Goal cannot exceed 50 characters")]
        public string Goal { get; set; } = string.Empty; // (Lose weight / Build muscle / Fitness)
        public DateTime? DateOfBirth { get; set; }

        public User? User { get; set; }



        // Navigation
        public ICollection<WorkoutPlan> WorkoutPlans { get; set; } = new List<WorkoutPlan>();
        public Subscription? Subscription { get; set; }
        public ICollection<DietPlan> DietPlans { get; set; } = new List<DietPlan>();

    }
}
