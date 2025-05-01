using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GestionHospitalisation.Models;
using GestionHospitalisation.Data;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        foreach (Role role in Enum.GetValues(typeof(Role)))
        {
            string roleName = role.ToString();
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}