using System.Security.Cryptography;

namespace ipoolBackend.Services;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 100_000;

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, KeySize);
        return string.Join('.', Convert.ToBase64String(salt), Convert.ToBase64String(hash), Iterations.ToString());
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
