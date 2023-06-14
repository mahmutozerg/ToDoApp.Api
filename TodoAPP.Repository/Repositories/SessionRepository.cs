using Microsoft.EntityFrameworkCore;
using TodoAPP.Core.Models;
using TodoAPP.Core.Repositories;

namespace TodoAPP.Repository.Repositories;

public class SessionRepository : GenericRepository<Session>,ISessionRepository
{
    private readonly DbSet<Session> _dbSet;
    
    public void UpdateOnLogin(Session session)
    {
        session.IsSessionActive = true;
        session.LastLogin = DateTime.Now;
        session.SessionGUID = Guid.NewGuid().ToString();
    }

    public async Task<Session?> GetSessionWithUserBySessionId(string sessionId)
    {
        return await _dbSet.Include(s => s.User).
                FirstOrDefaultAsync(s => s.SessionGUID == sessionId && s.IsSessionActive);
    }
    

    public SessionRepository(AppDbContext context) : base(context)
    {
        _dbSet = context.Set<Session>();

    }
}