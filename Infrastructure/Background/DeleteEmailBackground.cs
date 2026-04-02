using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Domain.Enums;

namespace Infrastructure.Background;

public class DeleteEmailBackground(IServiceScopeFactory scopeFactory)
{
    public async Task DeleteEmail()
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();

        var users = await context.Users
            .Where(u => u.EmailConfirmed == false)
            .Where(u => !context.UserRoles
                .Any(ur => ur.UserId == u.Id && ur.RoleId == (int)Roles.Admin))
            .ToListAsync();

        if (users.Count != 0)
        {
            foreach (var user in users)
            {
                user.IsDeleted = true;
            }

            await context.SaveChangesAsync();
            Log.Information($"{users.Count} users deleted");
        }
        else
        {
            Log.Information("No deleted");
        }
    }
}