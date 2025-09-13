using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using VoteApp.API.Data;
using NSwag.AspNetCore;
// using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.Title = "VoteApp API";
    config.Version = "v1";
    config.Description = "API pour la gestion des campagnes, candidats et votes.";
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
app.Run();