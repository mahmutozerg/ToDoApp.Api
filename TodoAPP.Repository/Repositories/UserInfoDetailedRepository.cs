using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TodoAPP.Core.Models;
using TodoAPP.Core.Repositories;
using TodoAPP.Core.Services;

namespace TodoAPP.Repository.Repositories;

public class UserInfoDetailedRepository : GenericRepository<UserInfoDetailed>,IUserInfoDetailedRepository
{
    public UserInfoDetailedRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<UserInfoDetailed> Create( )
    {
        var entity = new UserInfoDetailed()
        {
            LastLogin = DateTime.Today,
            IsUserActive = true,
            CreatedAt = DateTime.Now,
        };

        return entity;
    }

   
}