using DbMigrations;
using MessagingMicroservice.Application;
using MessagingMicroservice.Extensions;
using Microsoft.EntityFrameworkCore;
using MessagingMicroservice.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add Swagger Generator
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMessagingSwagger();

builder.Services.AddDbContext<MessagingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddApplicationDependencies();
builder.Services.AddInfrastructureDependencies();
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Enable Swagger JSON endpoint and UI
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Messaging Microservice API v1");
        c.RoutePrefix = "swagger"; // Serves UI at /swagger/index.html
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();