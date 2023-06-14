using Microsoft.EntityFrameworkCore;
using TodoAPP.Core.Models;
using TodoAPP.Core.Repositories;

namespace TodoAPP.Repository.Repositories;

public class UserRepository:GenericRepository<User>,IUserRepository
{
    private readonly DbSet<User> _dbSet;

    public UserRepository(AppDbContext context) : base(context)
    {
        _dbSet = context.Set<User>();
    }

    public async Task<User?> GetUserInfoByEmail(string email)
    {
        var user = await _dbSet.Where(u=>u.Email==email&& !u.IsDeleted).FirstOrDefaultAsync();
        return user;
    }

    public new  void  Remove(User user)
    {
        user.IsDeleted = true;
        Update(user);
        
    }
}