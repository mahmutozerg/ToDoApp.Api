using System.Xml.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoAPP.Core.Models;

namespace TodoAPP.Repository.Configurations;

public class UserInfoConfigurations:IEntityTypeConfiguration<UserInfo>
{
    public void Configure(EntityTypeBuilder<UserInfo> builder)
    {
        builder.HasOne(x => x.UserInfoDetailed).WithOne()
            .HasForeignKey<UserInfoDetailed>(x=> x.Id);
        builder.HasMany(x => x.ToDos).WithOne().HasForeignKey(x => x.UserInfoId);
    }
}