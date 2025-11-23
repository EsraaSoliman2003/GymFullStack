using System.ComponentModel.DataAnnotations;

namespace Gym.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } = "Trainee";

      

    }
}
