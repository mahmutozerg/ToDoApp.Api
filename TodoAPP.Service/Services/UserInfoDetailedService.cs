using TodoAPP.Core.Models;
using TodoAPP.Core.Repositories;
using TodoAPP.Core.Services;
using TodoAPP.Core.UnitOfWorks;

namespace TodoAPP.Service.Services;

public class UserInfoDetailedService:Service<UserInfoDetailed>,IUserInfoDetailedService
{
    private readonly IUserInfoDetailedRepository _userInfoDetailedRepository;
    private readonly IUnitOfWork _unitOfWork;
    public async Task<UserInfoDetailed> CreateInitial()
    { 
        var entity = new UserInfoDetailed()
        {
            LastLogin = DateTime.Today,
            IsUserActive = true,
            CreatedAt = DateTime.Now,
            SessionGUID =  Guid.NewGuid().ToString()

        };
        return entity;
    }

    public void UpdateOnLogin(UserInfoDetailed userInfoDetailed)
    {
        _userInfoDetailedRepository.UpdateOnLogin(userInfoDetailed);
    }

    public UserInfoDetailedService(IGenericRepository<UserInfoDetailed?> repository, IUserInfoDetailedRepository userInfoDetailedRepository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
    {
        _userInfoDetailedRepository = userInfoDetailedRepository;
        _unitOfWork = unitOfWork;
    }
}