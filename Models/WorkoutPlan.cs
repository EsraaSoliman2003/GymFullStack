using System.ComponentModel.DataAnnotations;

namespace Gym.Models
{
    public class WorkoutPlan
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Day of the week is required")]
        [Display(Name = "Day of the Week")]
        [StringLength(20, ErrorMessage = "Day name cannot exceed 20 characters")]
        public string DayOfWeek { get; set; } = string.Empty;
        [Required(ErrorMessage = "Exercise name is required")]
        [Display(Name = "Exercise Name")]
        [StringLength(100, ErrorMessage = "Exercise name cannot exceed 100 characters")]
        public string ExerciseName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Sets count is required")]
        public int Sets { get; set; }

        [Required(ErrorMessage = "Reps count is required")]
        public int Reps { get; set; }
        [Required(ErrorMessage = "Trainee ID is required")]
        [Display(Name = "Trainee")]
        public int TraineeId { get; set; }

        public Trainee? Trainee { get; set; }
    }
}
