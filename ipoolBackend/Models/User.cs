using System.ComponentModel.DataAnnotations;

namespace ipoolBackend.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string NormalizedEmail { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
