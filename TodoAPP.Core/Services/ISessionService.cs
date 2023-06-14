using TodoAPP.Core.Models;

namespace TodoAPP.Core.Services;

public interface ISessionService:IService<Session>
{
     Task<Session> CreateSession( int id);
     void UpdateOnLogin(Session session);
     Task<Session?> GetSessionWithUserBySessionId(string sessionId);
     Task RemoveSessions(List<Session?> sessions);

}