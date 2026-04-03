using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder;

public static class Seed
{
    public static async Task SeedAdmin(UserManager<User> userManager,
        RoleManager<IdentityRole<int>> roleManager)
    {
        if (!roleManager.RoleExistsAsync(Role.Admin.ToString()).Result)
        {
            await roleManager.CreateAsync(new IdentityRole<int>("Admin"));
        }
        var user = userManager.Users.FirstOrDefault(x=>x.UserName == "Admin");
        if (user == null)
        {
            var newUser = new User()
            {
                UserName = "Admin",
                PhoneNumber = "123080206",
                Email = "admin@gmail.com",
                About = "Admin Mix_Tj",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            var result = await userManager.CreateAsync(newUser, "1234Qwerty$");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, Role.Admin.ToString());
            }
        }
    }

    public static async Task<bool> SeedRole(RoleManager<IdentityRole<int>> roleManager)
    {
        var newRole = new List<IdentityRole<int>>()
        {
            new(Role.Admin.ToString()),
            new(Role.User.ToString()),
        };
        var roles = await roleManager.Roles.ToListAsync();
        foreach (var role in newRole)
        {
            if(roles.Any(x => x.Name == role.Name)) 
                continue;
            await roleManager.CreateAsync(role);
        }
        return true;
    }
}