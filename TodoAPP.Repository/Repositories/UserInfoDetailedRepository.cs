using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TodoAPP.Core.Models;
using TodoAPP.Core.Repositories;
using TodoAPP.Core.Services;

namespace TodoAPP.Repository.Repositories;

public class UserInfoDetailedRepository : GenericRepository<UserInfoDetailed>,IUserInfoDetailedRepository
{
    private readonly AppDbContext _context;

    
    public void UpdateOnLogin(UserInfoDetailed userInfoDetailed)
    {
        userInfoDetailed.IsUserActive = true;
        userInfoDetailed.LastLogin = DateTime.Now;
        userInfoDetailed.SessionGUID = Guid.NewGuid().ToString();
    }

    public UserInfoDetailedRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}