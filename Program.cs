using DogShelterMvc.Data;
using DogShelterMvc.Models;
using DogShelterMvc.Helpers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure SQLite database
builder.Services.AddDbContext<DogShelterDbContext>(options =>
    options.UseSqlite("Data Source=dogshelter.db"));

// Configure session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DogShelterDbContext>();
    dbContext.Database.EnsureCreated();
    
    // Create default admin user if no users exist
    if (!dbContext.Users.Any(u => u.Uname == "admin"))
    {
        var defaultUser = new DogShelterMvc.Models.User
        {
            Uname = "admin",
            Hash = DogShelterMvc.Helpers.PasswordHelper.HashPassword("admin"),
            Perms = 999 // High permissions for admin
        };
        dbContext.Users.Add(defaultUser);
        dbContext.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

