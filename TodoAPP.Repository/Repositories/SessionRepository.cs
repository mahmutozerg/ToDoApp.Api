using Microsoft.EntityFrameworkCore;
using TodoAPP.Core.Models;
using TodoAPP.Core.Repositories;

namespace TodoAPP.Repository.Repositories;

public class SessionRepository : GenericRepository<SessionEntity>,ISessionRepository
{
    private readonly DbSet<SessionEntity> _dbSet;
    
    public void UpdateOnLogin(SessionEntity sessionEntity)
    {
        sessionEntity.IsSessionActive = true;
        sessionEntity.LastLogin = DateTime.Now;
        sessionEntity.SessionGUID = Guid.NewGuid().ToString();
    }

    public async Task<SessionEntity?> GetSessionWithUserBySessionId(string sessionId)
    {
        return await _dbSet.Include(s => s.UserEntity).
                FirstOrDefaultAsync(s => s.SessionGUID == sessionId && s.IsSessionActive);
    }
    

    public SessionRepository(AppDbContext context) : base(context)
    {
        _dbSet = context.Set<SessionEntity>();

    }
}