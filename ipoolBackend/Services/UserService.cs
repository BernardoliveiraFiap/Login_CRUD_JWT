using ipoolBackend.DTOs;
using ipoolBackend.Repositories;

namespace ipoolBackend.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user is null) return null;

        return new UserResponse(user.Id, user.Name, user.Email, user.CreatedAt);
    }
}
