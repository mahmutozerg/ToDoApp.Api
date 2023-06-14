using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoAPP.Core.Models;

namespace TodoAPP.Repository.Configurations;

public class SessionConfigurations:IEntityTypeConfiguration<Session>
{
    
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedOnAdd();
        builder.HasOne<User>(s => s.User).WithMany().HasForeignKey(s=> s.UserId);
        builder.ToTable("Sessions");
    }
}