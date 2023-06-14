using TodoAPP.Core.Models;

namespace TodoAPP.Core.Repositories;

public interface ISessionRepository:IGenericRepository<Session>
{
    public void UpdateOnLogin(Session session);
    public Task<Session?> GetSessionWithUserBySessionId(string sessionId);

}