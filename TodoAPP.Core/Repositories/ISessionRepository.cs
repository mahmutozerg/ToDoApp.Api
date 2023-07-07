using TodoAPP.Core.Models;

namespace TodoAPP.Core.Repositories;

public interface ISessionRepository:IGenericRepository<SessionEntity>
{
    public void UpdateOnLogin(SessionEntity sessionEntity);
    public Task<SessionEntity?> GetSessionWithUserBySessionId(string sessionId);

}