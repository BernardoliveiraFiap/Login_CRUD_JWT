using Scalar.AspNetCore;
using ipoolBackend.Data;
using ipoolBackend.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("iPool API");
    });

    await app.EnsureDatabaseAsync();
}

// app.UseHttpsRedirection(); // Comentado para desenvolvimento

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
