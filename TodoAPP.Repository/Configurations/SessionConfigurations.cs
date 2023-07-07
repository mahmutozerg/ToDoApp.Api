using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoAPP.Core.Models;

namespace TodoAPP.Repository.Configurations;

public class SessionConfigurations:IEntityTypeConfiguration<SessionEntity>
{
    
    public void Configure(EntityTypeBuilder<SessionEntity> builder)
    {
        
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedOnAdd();
        builder.HasOne<UserEntity>(s => s.UserEntity).WithMany().HasForeignKey(s=> s.UserId);
        builder.ToTable("Sessions");
    }
}