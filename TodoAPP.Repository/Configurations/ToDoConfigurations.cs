using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoAPP.Core.Models;

namespace TodoAPP.Repository.Configurations;

public class ToDoConfigurations:IEntityTypeConfiguration<ToDoEntity>
{
    public void Configure(EntityTypeBuilder<ToDoEntity> builder)
    {
        
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.TodoContext).IsRequired();
        builder.ToTable("ToDos");
    }
}