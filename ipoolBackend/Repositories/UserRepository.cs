using Microsoft.EntityFrameworkCore;
using ipoolBackend.Data;
using ipoolBackend.Models;

namespace ipoolBackend.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();
        return _dbContext.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, cancellationToken);
    }

    public Task<User?> GetByNormalizedEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
    {
        var normalized = normalizedEmail.Trim().ToLowerInvariant();
        return _dbContext.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.NormalizedEmail == normalized, cancellationToken);
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
