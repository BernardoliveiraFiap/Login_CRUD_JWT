using ipoolBackend.DTOs;

namespace ipoolBackend.Services;

public interface IUserService
{
    Task<UserResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
