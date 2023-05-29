using TodoAPP.Core.Models;
using TodoAPP.Core.Repositories;
using TodoAPP.Core.Services;
using TodoAPP.Core.UnitOfWorks;

namespace TodoAPP.Service.Services;

public class UserInfoDetailedService:Service<UserInfoDetailed>,IUserInfoDetailedService
{
    private readonly IUserInfoDetailedRepository _userInfoDetailedRepository;
    public async Task<UserInfoDetailed> CreateInitial()
    { 
        var entity=  await _userInfoDetailedRepository.Create();
        return entity;
    }

    public UserInfoDetailedService(IGenericRepository<UserInfoDetailed> repository, IUserInfoDetailedRepository userInfoDetailedRepository) : base(repository, unitOfWork)
    {
        _userInfoDetailedRepository = userInfoDetailedRepository;
    }
}