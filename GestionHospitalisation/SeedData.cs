using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GestionHospitalisation.Models;
using GestionHospitalisation.Data;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new GestionHospitalisationContext(
            serviceProvider.GetRequiredService<DbContextOptions<GestionHospitalisationContext>>()))
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // Create roles if they don't exist
            string[] roleNames = { "Admin", "User" }; // Basic roles

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create admin user (username only)
            await CreateUserWithRole(userManager, "admin", "Admin1234", "Admin");

            // Create normal user (username only)
            await CreateUserWithRole(userManager, "user", "User1234", "User");
        }
    }

    private static async Task CreateUserWithRole(
        UserManager<IdentityUser> userManager,
        string username,
        string password,
        string role)
    {
        var user = await userManager.FindByNameAsync(username);
        if (user == null)
        {
            user = new IdentityUser
            {
                UserName = username // Only username is required
            };

            var createResult = await userManager.CreateAsync(user, password);
            if (createResult.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
            }
            else
            {
                // Log errors if user creation fails
                foreach (var error in createResult.Errors)
                {
                    Console.WriteLine($"Error creating user: {error.Description}");
                }
            }
        }
        else
        {
            // Ensure existing user has the correct role
            if (!await userManager.IsInRoleAsync(user, role))
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}