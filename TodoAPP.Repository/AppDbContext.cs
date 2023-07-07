using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TodoAPP.Core.Models;

namespace TodoAPP.Repository;

public class AppDbContext:DbContext
{
    public DbSet<SessionEntity> UserInfoDetaileds { get; set; }
    public DbSet<UserEntity> UserInfos { get; set; }
    public Task<List<ToDoEntity>> ToDos { get; set; }

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