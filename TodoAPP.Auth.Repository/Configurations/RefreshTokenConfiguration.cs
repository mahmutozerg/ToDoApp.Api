using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoAPP.Auth.Core.Models;

namespace TodoAPP.Auth.Repository.Configurations;

public class RefreshTokenConfiguration:IEntityTypeConfiguration<UserRefreshToken>
{
    public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
    {
        builder.HasKey(t => t.UserId);
        builder.Property(t => t.Token);
        
    }
}