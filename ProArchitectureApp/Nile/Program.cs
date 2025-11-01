using Nile;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
var builder = WebApplication.CreateBuilder(args);

// Add controllers (MVC API)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // This tells System.Text.Json to ignore cycles
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Nile API",
        Version = "v1",
        Description = "A social media platform API"
    });
});

// Pull in all Nile services, repos, and DbContext
builder.Services.AddNile(builder.Configuration);

var app = builder.Build();

// Configure Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
