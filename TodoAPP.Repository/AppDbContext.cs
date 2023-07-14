using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TodoAPP.Core.Models;

namespace TodoAPP.Repository;

public class AppDbContext:DbContext
{

    public Task<List<ToDo>> ToDos { get; set; }

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