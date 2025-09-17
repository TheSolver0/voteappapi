using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using VoteApp.API.Data;
using NSwag.AspNetCore;
using VoteApp.API.Middlewares;
using Microsoft.Extensions.Logging;
// using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=VoteAppDb.db")); // Utilisation d'une base de données en mémoire pour le développement et les tests
builder.Services.AddOpenApiDocument(config =>
{
    config.Title = "VoteApp API";
    config.Version = "v1";
    config.Description = "API pour la gestion des campagnes, candidats et votes.";
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        /*policy.WithOrigins("http://localhost:3000") // ton frontend local
              .AllowAnyHeader()
              .AllowAnyMethod();*/
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();       // Expose /swagger/v1/swagger.json
    app.UseSwaggerUi();    // Interface Swagger UI v3
}

var logger = app.Services.GetRequiredService<ILogger<Program>>();

// Logging de requêtes
app.Use(async (context, next) =>
{
    logger.LogInformation("Request: {method} {path}", context.Request.Method, context.Request.Path);
    await next.Invoke();
});

app.MapGet("/", () => "Welcome to VoteApp API!").WithOpenApi();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<ErrorHandlingMiddleware>();
/*using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    Console.WriteLine("Avant seeding : " + context.Candidates.Count());
    await DbInitializer.SeedAsync(context);
    Console.WriteLine("Après seeding : " + context.Candidates.Count());
}*/
app.UseCors("AllowFrontend");
app.Run();