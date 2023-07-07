using Microsoft.EntityFrameworkCore;
using TodoAPP.Core.Models;
using TodoAPP.Core.Repositories;

namespace TodoAPP.Repository.Repositories;

public class UserRepository:GenericRepository<UserEntity>,IUserRepository
{
    private readonly DbSet<UserEntity> _dbSet;

    public UserRepository(AppDbContext context) : base(context)
    {
        _dbSet = context.Set<UserEntity>();
    }

    public async Task<UserEntity?> GetUserInfoByEmail(string email)
    {
        var user = await _dbSet.Where(u=>u.Email==email&& !u.IsDeleted).FirstOrDefaultAsync();
        return user;
    }

    public new  void  Remove(UserEntity userEntity)
    {
        userEntity.IsDeleted = true;
        Update(userEntity);
        
    }
}