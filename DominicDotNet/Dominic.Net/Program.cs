

using Dominic.Net.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

//builder.Services.AddScoped<ICategoryRepository, MockCategoryRepository>();
//builder.Services.AddScoped<IPieRepository, MockPieRepository>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPieRepository, PieRepository>();

builder.Services.AddScoped<IShoppingCart, ShoppingCart>(sp => ShoppingCart.GetCart(sp));
builder.Services.AddSession(); 
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<DominicShopDbContext>(options => {
    options.UseSqlServer(
        builder.Configuration["ConnectionStrings:DominicsShopDbContextConnectionString"]);
});

var app = builder.Build();

<<<<<<< HEAD
// Configure the HTTP request pipeline.
=======
//app.MapGet("/", () => "Hello World!");
>>>>>>> 3c226fbd1d1f2d6ef1cea529e12ea67e156fce0d
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

<<<<<<< HEAD
// Configure middleware to serve static files.
app.UseStaticFiles(); // Enable serving static files from wwwroot folder.
app.UseSession(); // Enable session middleware.

// Enable routing.
app.UseRouting();

// Map controller routes with default routing
app.MapDefaultControllerRoute();
// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");

//DbInitializer.Seed(app); // Seed the database with initial data if database is empty.

app.Run();
=======
app.UseStaticFiles();
app.UseSession();

//app.MapDefaultControllerRoute();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

DbInitializer.Seed(app);

app.Run();
>>>>>>> 3c226fbd1d1f2d6ef1cea529e12ea67e156fce0d
