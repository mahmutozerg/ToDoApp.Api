using System.Xml.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoAPP.Core.Models;

namespace TodoAPP.Repository.Configurations;

public class UserConfigurations:IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Email).IsRequired();
        builder.Property(u => u.UserName).IsRequired().HasMaxLength(50);
        
        builder.HasMany(u => u.ToDos).WithOne(u=> u.UserEntity).HasForeignKey(x => x.UserId);
       
        builder.ToTable("Users");
    }
}