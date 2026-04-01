using System.Text;
using Domain.DTOs.EmailDto;
using Domain.Entities;
using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure.Background;
using Infrastructure.Data;
using Infrastructure.Data.Seeder;
using Infrastructure.ExtensionMethod;
using Infrastructure.FileStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog Configuration
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug)
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
    .Enrich.FromLogContext()
    .MinimumLevel.Debug()
    .CreateLogger();

builder.Host.UseSerilog();

//File
builder.Services.AddScoped<IFileStorage>(
    sp => new FileStorage(builder.Environment.ContentRootPath));

//DataContext
builder.Services.RegisterDbContext(builder.Configuration);

builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("EmailSetting"));

//Services
builder.Services.RegisterServices();

builder.Services.AddHttpContextAccessor();

//Identity
builder.Services.RegisterIdentity();

//Swagger
builder.Services.RegisterSwagger();

//HangFire
builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

builder.Services.AddJwt(builder.Configuration);

builder.Services.AddAuthorization(opt => { opt.AddPolicy("AdminOnly", p => p.RequireRole("Admin")); });


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.UseStaticFiles();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
        var data =  services.GetRequiredService<DataContext>();
        await data.Database.MigrateAsync();
        await Seed.SeedRole(roleManager);
        await Seed.SeedAdmin(userManager, roleManager);
    }
    catch(Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}


try
{
    using var scope = app.Services.CreateScope();
    var serviceProvider = scope.ServiceProvider;
    var dataContext = serviceProvider.GetRequiredService<DataContext>();
    await dataContext.Database.MigrateAsync();
    //hangfire
    var recurringJobManager = serviceProvider.GetRequiredService<IRecurringJobManager>();

    recurringJobManager.AddOrUpdate<DeleteEmailBackground>(
        "hangfire-service",
        service => service.DeleteEmail(),
        "0 1 * * *"
    );
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

app.Run();