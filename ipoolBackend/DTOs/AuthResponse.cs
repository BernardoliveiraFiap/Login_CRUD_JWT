namespace ipoolBackend.DTOs;

public record AuthResponse(string Token, DateTime ExpiresAt, string TokenType, UserResponse User);
