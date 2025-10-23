using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Infrastructure.Background;

public class DeleteEmailBackground(IServiceScopeFactory scopeFactory)
{
    public async Task DeleteEmail()
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        var users = await context.Users
            .Where(x => x.EmailConfirmed == false)
            .ToListAsync();
        var cnt = 0;
        foreach (var user in users)
        {
            context.Users.Remove(user);
            cnt++;
        }

        if (cnt > 0)
        {
            await context.SaveChangesAsync();
            Log.Information($"{cnt} users deleted");
        }
        else
        {
            Log.Information("No users deleted");
        }
    }
}