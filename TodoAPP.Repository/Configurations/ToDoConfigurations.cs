using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoAPP.Core.Models;

namespace TodoAPP.Repository.Configurations;

public class ToDoConfigurations:IEntityTypeConfiguration<ToDo>
{
    public void Configure(EntityTypeBuilder<ToDo> builder)
    {
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UserInfoId).IsRequired();
        builder.Property(x => x.TodoContext).IsRequired();
    }
}