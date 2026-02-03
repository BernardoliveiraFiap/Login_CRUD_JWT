using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ipoolBackend.Data;
using ipoolBackend.Options;
using ipoolBackend.Repositories;
using ipoolBackend.Services;

namespace ipoolBackend.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection("Jwt"))
            .Validate(options => !string.IsNullOrWhiteSpace(options.Key), "Jwt:Key is required.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.Issuer), "Jwt:Issuer is required.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.Audience), "Jwt:Audience is required.")
            .ValidateOnStart();

        services.AddOptions<PasswordHashOptions>()
            .Bind(configuration.GetSection("PasswordHash"))
            .Validate(options => options.Iterations >= 10_000, "PasswordHash:Iterations must be >= 10000.")
            .Validate(options => options.SaltSize >= 16, "PasswordHash:SaltSize must be >= 16.")
            .Validate(options => options.KeySize >= 32, "PasswordHash:KeySize must be >= 32.")
            .ValidateOnStart();

        services.AddOptions<AppInfoOptions>()
            .Bind(configuration.GetSection("AppInfo"))
            .ValidateOnStart();

        var jwtSection = configuration.GetSection("Jwt");
        var jwtOptions = jwtSection.Get<JwtOptions>() ?? new JwtOptions();
        var jwtKey = jwtSection["Key"] ?? string.Empty;
        var jwtIssuer = jwtSection["Issuer"];
        var jwtAudience = jwtSection["Audience"];

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    ClockSkew = TimeSpan.FromSeconds(jwtOptions.ClockSkewSeconds),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });

        services.AddAuthorization();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }
}
