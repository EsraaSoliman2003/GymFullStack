using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Gym.Models
{
    public class DietPlan
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Trainee ID is required")]
        public int TraineeId { get; set; }

        [Required(ErrorMessage = "Meal type is required")]
        [StringLength(30, ErrorMessage = "Meal type cannot exceed 30 characters")]
        public string MealType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(300, ErrorMessage = "Description cannot exceed 300 characters")]
        public string Description { get; set; } = string.Empty;

        [ValidateNever]              // 👈 مهم
        public Trainee? Trainee { get; set; }   // 👈 خليها nullable
    }
}
