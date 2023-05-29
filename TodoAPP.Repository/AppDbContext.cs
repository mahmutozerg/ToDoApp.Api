using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TodoAPP.Core.Models;

namespace TodoAPP.Repository;

public class AppDbContext:DbContext
{
    public DbSet<UserInfoDetailed> UserInfoDetaileds { get; set; }
    public DbSet<UserInfo> UserInfos { get; set; }
    public DbSet<ToDo> ToDos { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {   
        //fill it
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    
}