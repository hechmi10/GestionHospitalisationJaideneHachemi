using Microsoft.EntityFrameworkCore;
using GestionHospitalisation.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using GestionHospitalisation.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext with retry policy for resiliency
builder.Services.AddDbContext<GestionHospitalisationContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("GestionHospitalisationContext")
            ?? throw new InvalidOperationException("Connection string 'GestionHospitalisationContext' not found."),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<GestionHospitalisationContext>()
    .AddDefaultTokenProviders();

// Configure Authentication Cookies with security enhancements
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "HospitalAuth";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/Home/SignIn";
    options.LogoutPath = "/Home/SignOut";
    options.AccessDeniedPath = "/Home/AccessDenied";
    options.SlidingExpiration = true;
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;

    // Security settings
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
        ? CookieSecurePolicy.SameAsRequest
        : CookieSecurePolicy.Always;
});

// Add session support (if needed)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
});

// Add antiforgery with additional security
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.SuppressXFrameOptionsHeader = false;
});

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

var app = builder.Build();





app.UseHttpsRedirection();
app.UseStaticFiles();

// Security headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    await next();
});

app.UseRouting();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Session (if added)
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=SignIn}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Seeding failed");
    }
}

app.Run();