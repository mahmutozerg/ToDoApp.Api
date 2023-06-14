using System.Xml.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoAPP.Core.Models;

namespace TodoAPP.Repository.Configurations;

public class UserConfigurations:IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Id).UseIdentityColumn();
        builder.Property(u => u.Email).IsRequired();
        builder.Property(u => u.UserName).IsRequired().HasMaxLength(50);
        
        builder.HasMany(u => u.ToDos).WithOne(u=> u.User).HasForeignKey(x => x.UserId);
       
        builder.ToTable("Users");
    }
}