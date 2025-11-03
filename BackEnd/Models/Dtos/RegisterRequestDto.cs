using System.ComponentModel.DataAnnotations;
using itsc_dotnet_practice.Utilities;

namespace itsc_dotnet_practice.Models.Dtos
{
    public class RegisterRequestDto
    {
        [Required]
        public string Username { get; set; } = "";

        [Required]
        public string FullName { get; set; } = "";

        [Required]
        [Phone]
        public string Phone { get; set; } = "";

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = "";

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = "";

        public string Role { get; set; } = "User";
    }
}
