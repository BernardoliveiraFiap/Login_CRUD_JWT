using Microsoft.EntityFrameworkCore;

namespace ipoolBackend.Data;

public static class DbInitializer
{
    public static async Task EnsureDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
    }
}
