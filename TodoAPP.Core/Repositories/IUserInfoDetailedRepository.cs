using TodoAPP.Core.Models;

namespace TodoAPP.Core.Repositories;

public interface IUserInfoDetailedRepository:IGenericRepository<UserInfoDetailed>
{
    public void UpdateOnLogin(UserInfoDetailed userInfoDetailed);

}