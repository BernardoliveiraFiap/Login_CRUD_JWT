using System.ComponentModel.DataAnnotations;

namespace ipoolBackend.DTOs;

public class LoginRequest
{
	[Required]
	[EmailAddress]
	[MaxLength(150)]
	public string Email { get; set; } = string.Empty;

	[Required]
	[MinLength(6)]
	public string Password { get; set; } = string.Empty;
}
