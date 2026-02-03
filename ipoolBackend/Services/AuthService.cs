using ipoolBackend.DTOs;
using ipoolBackend.Models;
using ipoolBackend.Repositories;
using ipoolBackend.Helpers;

namespace ipoolBackend.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var email = request.Email.Trim();
        var normalizedEmail = EmailNormalizer.Normalize(email);
        var existing = await _userRepository.GetByNormalizedEmailAsync(normalizedEmail, cancellationToken);
        if (existing is not null)
        {
            throw new InvalidOperationException("E-mail já cadastrado.");
        }

        var user = new User
        {
            Name = request.Name.Trim(),
            Email = email,
            NormalizedEmail = normalizedEmail,
            PasswordHash = _passwordHasher.Hash(request.Password),
            IsActive = true
        };

        await _userRepository.AddAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        var (token, expiresAt) = _tokenService.GenerateToken(user);
        var userResponse = new UserResponse(user.Id, user.Name, user.Email, user.IsActive, user.CreatedAt);
        return new AuthResponse(token, expiresAt, "Bearer", userResponse);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = EmailNormalizer.Normalize(request.Email);
        var user = await _userRepository.GetByNormalizedEmailAsync(normalizedEmail, cancellationToken);
        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Credenciais inválidas.");
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("Usuário inativo.");
        }

        var (token, expiresAt) = _tokenService.GenerateToken(user);
        var userResponse = new UserResponse(user.Id, user.Name, user.Email, user.IsActive, user.CreatedAt);
        return new AuthResponse(token, expiresAt, "Bearer", userResponse);
    }
}
