using Microsoft.OpenApi;
using Microsoft.Extensions.FileProviders;
using Scalar.AspNetCore;
using ipoolBackend.Data;
using ipoolBackend.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Name = "Authorization"
        };

        document.Security ??= new List<OpenApiSecurityRequirement>();
        document.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer")] = new List<string>()
        });

        return Task.CompletedTask;
    });
});
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

var frontendPath = Path.GetFullPath(Path.Combine(app.Environment.ContentRootPath, "..", "frontend"));
if (Directory.Exists(frontendPath))
{
    var frontendProvider = new PhysicalFileProvider(frontendPath);
    app.UseDefaultFiles(new DefaultFilesOptions { FileProvider = frontendProvider });
    app.UseStaticFiles(new StaticFileOptions { FileProvider = frontendProvider });
}

// app.UseHttpsRedirection(); // Comentado para desenvolvimento

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
