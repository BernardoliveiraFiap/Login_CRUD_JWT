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

        return new UserResponse(user.Id, user.Name, user.Email, user.IsActive, user.CreatedAt);
    }

    public async Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);
        return users.Select(user => new UserResponse(user.Id, user.Name, user.Email, user.IsActive, user.CreatedAt));
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return false;
        }

        _userRepository.Remove(user);
        await _userRepository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
