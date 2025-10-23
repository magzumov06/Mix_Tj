using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Data;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<User, IdentityRole<int>, int>(options)
{
    public DbSet<Like> Likes { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Video> Videos { get; set; }
    public DbSet<News> Newses { get; set; }
    public DbSet<Files>  Files { get; set; }
}