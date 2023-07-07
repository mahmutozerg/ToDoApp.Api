using TodoAPP.Core.Models;
using TodoAPP.Core.Repositories;
using TodoAPP.Core.Services;
using TodoAPP.Core.UnitOfWorks;

namespace TodoAPP.Service.Services;

public class SessionService:Service<SessionEntity>,ISessionService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IUnitOfWork _unitOfWork;
    public async Task<SessionEntity> CreateSession(string id)
    { 
        var entity = new SessionEntity()
        {
            LastLogin = DateTime.Now,
            IsSessionActive = true,
            CreatedAt = DateTime.Now,
            SessionGUID =  Guid.NewGuid().ToString(),
            UserId = id

        };
        await _sessionRepository.AddAsync(entity);
        await _unitOfWork.CommitAsync();
        return entity;
    }
    public void UpdateOnLogin(SessionEntity sessionEntity)
    {
        _sessionRepository.UpdateOnLogin(sessionEntity);
    }

    public async Task<SessionEntity?> GetSessionWithUserBySessionIdAsync(string sessionId)
    {
        return await _sessionRepository.GetSessionWithUserBySessionId(sessionId);
    }

    public async Task RemoveSessions(List<SessionEntity?> sessions)
    {
                
        foreach (var session in sessions)
        {
            session.IsSessionActive = false;
            session.EndedAt = DateTime.Now;
            await Update(session);
        }
    }

    public SessionService(IGenericRepository<SessionEntity?> repository, ISessionRepository sessionRepository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
    {
        _sessionRepository = sessionRepository;
        _unitOfWork = unitOfWork;
    }
}