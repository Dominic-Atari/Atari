using Nile;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Controllers + JSON options
builder.Services
    .AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Swagger/OpenAPI
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

// CORS (must be BEFORE Build())
var allowSpa = "_allowSpa";
builder.Services.AddCors(o => o.AddPolicy(allowSpa, p =>
    p.AllowAnyHeader()
     .AllowAnyMethod()
     .WithOrigins(
        "http://localhost:4200", // Angular default
        "http://localhost:4300", // our dev server
        "http://localhost:4301", // preview static server
        "http://localhost:5173"   // Vite default
     )));

// Pull in Nile registrations (DbContext, repos, services, etc.)
builder.Services.AddNile(builder.Configuration);

var app = builder.Build();

// Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // enable if you want HTTPS locally
// app.UseAuthentication();   // add when you wire JWT or other auth
app.UseAuthorization();

app.UseCors(allowSpa);

app.MapControllers();

app.Run();
