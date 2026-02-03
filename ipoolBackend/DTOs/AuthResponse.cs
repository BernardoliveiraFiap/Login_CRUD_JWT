namespace ipoolBackend.DTOs;

public record AuthResponse(string Token, DateTime ExpiresAt, UserResponse User);
