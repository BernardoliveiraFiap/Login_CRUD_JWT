using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using ipoolBackend.Options;

namespace ipoolBackend.Services;

public class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHashOptions _options;

    public PasswordHasher(IOptions<PasswordHashOptions> options)
    {
        _options = options.Value;
    }

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(_options.SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, _options.Iterations, HashAlgorithmName.SHA256, _options.KeySize);
        return string.Join('.', Convert.ToBase64String(salt), Convert.ToBase64String(hash), _options.Iterations.ToString());
    }

    public bool Verify(string password, string passwordHash)
    {
        var parts = passwordHash.Split('.', 3);
        if (parts.Length != 3) return false;

        var salt = Convert.FromBase64String(parts[0]);
        var hash = Convert.FromBase64String(parts[1]);
        var iterations = int.Parse(parts[2]);

        var inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, hash.Length);
        return CryptographicOperations.FixedTimeEquals(hash, inputHash);
    }
}
