using ipoolBackend.Models;

namespace ipoolBackend.Services;

public interface ITokenService
{
    (string Token, DateTime ExpiresAt) GenerateToken(User user);
}
