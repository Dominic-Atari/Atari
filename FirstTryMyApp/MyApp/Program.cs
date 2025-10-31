using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using MyApp.Data;
using MyApp.Services;
using MyApp.Service;
using MyApp.Service.Sirvices;
using Microsoft.OpenApi.Models;
using Pomelo.EntityFrameworkCore.MySql;

var builder = WebApplication.CreateBuilder(args);

// MySQL connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.Parse("8.0.33-mysql")));


// Register services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddAuthorization();
builder.Services.AddControllers(); // <-- This line is required
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Automatically create database and apply migrations at startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    //db.Database.EnsureDeleted();   // This will drop the database
    db.Database.Migrate();         // This will recreate it and apply migrations
}

app.Run();
