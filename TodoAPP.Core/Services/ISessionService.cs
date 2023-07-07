using TodoAPP.Core.Models;

namespace TodoAPP.Core.Services;

public interface ISessionService:IService<SessionEntity>
{
     Task<SessionEntity> CreateSession( string id);
     void UpdateOnLogin(SessionEntity sessionEntity);
     Task<SessionEntity?> GetSessionWithUserBySessionIdAsync(string sessionId);
     Task RemoveSessions(List<SessionEntity?> sessions);

}