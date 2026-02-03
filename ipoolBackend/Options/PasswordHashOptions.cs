namespace ipoolBackend.Options;

public class PasswordHashOptions
{
    public int Iterations { get; set; } = 100_000;
    public int SaltSize { get; set; } = 16;
    public int KeySize { get; set; } = 32;
}
