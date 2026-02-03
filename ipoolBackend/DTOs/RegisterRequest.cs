using System.ComponentModel.DataAnnotations;

namespace ipoolBackend.DTOs;

public class RegisterRequest
{
	[Required]
	[MinLength(3)]
	[MaxLength(120)]
	public string Name { get; set; } = string.Empty;

	[Required]
	[EmailAddress]
	[MaxLength(150)]
	public string Email { get; set; } = string.Empty;

	[Required]
	[MinLength(6)]
	[MaxLength(100)]
	public string Password { get; set; } = string.Empty;

	public bool IsActive { get; set; } = true;
}
