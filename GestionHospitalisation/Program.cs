using Microsoft.EntityFrameworkCore;
using GestionHospitalisation.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using GestionHospitalisation.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<GestionHospitalisationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GestionHospitalisationContext")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<GestionHospitalisationContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();


var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<GestionHospitalisationContext>();
        await context.Database.EnsureCreatedAsync(); // Creates DB if not exists

        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Ensure roles exist
        foreach (var roleName in Enum.GetNames(typeof(Role)))
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Database creation/initialization failed");
    }
    await SeedData.Initialize(services);
}

// Middleware pipeline
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

// Updated default route to start with SignIn
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=SignIn}");

app.Run();