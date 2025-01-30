using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ParafiaApk.Data;
using ParafiaApk.Models;

var builder = WebApplication.CreateBuilder(args);

// Dodaj us³ugê DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dodaj Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add services to the container
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Role
var scope = app.Services.CreateScope();
await SeedData.InitializeAsync(scope.ServiceProvider);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Dodanie ról, jeœli nie istniej¹
        if (!await roleManager.RoleExistsAsync("Parafianin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Parafianin"));
        }

        if (!await roleManager.RoleExistsAsync("Ksiadz"))
        {
            await roleManager.CreateAsync(new IdentityRole("Ksiadz"));
        }
    }
}
