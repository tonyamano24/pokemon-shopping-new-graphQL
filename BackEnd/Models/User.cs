// User.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace itsc_dotnet_practice.Models;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required] public string Username { get; set; } = "";
    [Required] public string FullName { get; set; } = "";
    [Required] public string Password { get; set; } = "";
    [Required] public string Phone { get; set; } = "";
    public string Role { get; set; } = "User";
}
