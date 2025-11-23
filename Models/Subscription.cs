using Gym.Models;
using System.ComponentModel.DataAnnotations;

namespace Gym.Models
{
    public class Subscription 
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Package name is required")]
        [StringLength(50, ErrorMessage = "Package name cannot exceed 50 characters")]
        public string PackageName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        public string Duration { get; set; } = string.Empty; // e.g., "1 Month", "3 Months", "1 Year"
        
        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "Trainee is required")]
        public int TraineeId { get; set; }
        public Trainee Trainee { get; set; } = default!;

    }
}


