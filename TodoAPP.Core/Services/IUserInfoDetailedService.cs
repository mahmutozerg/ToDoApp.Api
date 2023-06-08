using TodoAPP.Core.Models;

namespace TodoAPP.Core.Services;

public interface IUserInfoDetailedService:IService<UserInfoDetailed>
{
    public Task<UserInfoDetailed> CreateInitial( );
    public void UpdateOnLogin(UserInfoDetailed userInfoDetailed);
    
}