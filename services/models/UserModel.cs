using System.ComponentModel.DataAnnotations;

namespace Services.Models;

public class User
{
    [Key]
    public int UserNID { get; set; }

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    // role: "Admin" or "Ranger"
    [Required]
    public string Role { get; set; } = "Ranger";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
