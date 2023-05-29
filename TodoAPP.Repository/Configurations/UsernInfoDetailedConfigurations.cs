using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoAPP.Core.Models;

namespace TodoAPP.Repository.Configurations;

public class UsernInfoDetailedConfigurations:IEntityTypeConfiguration<UserInfoDetailed>
{
    
    public void Configure(EntityTypeBuilder<UserInfoDetailed> builder)
    {
        builder.HasKey(x => x.Id);

        
    
    }
}